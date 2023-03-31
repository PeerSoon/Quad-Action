using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    
    Vector3 lookVec;
    Vector3 tauntVec;
    public bool isLook;

    void Awake()  // Enemy 스크립스 상속 시 기존 코드x 상속 부모의 Awake 코드는 자식 스크립트만 상속받아 사용 가능
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think()); // 프레임 시작 시 Think 코루틴 실행               
    }

    
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if(isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");  // GetAxis : -1.0f ~ 1.0f (부드러운 이동) < - > GetAxisRaw : -1, 0, 1 (즉시 반응, 정수)
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);     
    }

    IEnumerator Think()
    { 
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(0.1f); // 첫 보스 움직임 난이도 조절용

        int ranAction = Random.Range(0, 5);
        switch(ranAction)
        {
            case 0:
            case 1:  // 미사일패턴
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3: //돌 굴러가는 패턴
                StartCoroutine(RockShot());
                break;
            case 4: //점프 공격 패턴
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;

        yield return new WaitForSeconds(2f);

        //Destroy(instantMissileA);
        //Destroy(instantMissileB);

        StartCoroutine(Think()); // 패턴 후 새로운 패턴
    }

    IEnumerator RockShot()
    {
        isLook = false;
        
        anim.SetTrigger("doBigShot");

        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        StartCoroutine(Think()); // 패턴 후 새로운 패턴
    }

    IEnumerator Taunt()
    {
        tauntVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false; // 점프 도중 플레이어 밀기 방지
        anim.SetTrigger("doTaunt");
        
        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;

        StartCoroutine(Think()); // 패턴 후 새로운 패턴
    }  
}