using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type {A, B, C, D};
    public Type enemyType;
    public float maxHealth;
    public float curHealth;
    public int score;
    public GameManager manager;
    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public GameObject[] coins;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Animator anim;

    void Awake() 
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        if(enemyType != Type.D)
            Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
    void Update() 
    {
        if(nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void FreezeVelocity()
    {
        if(isChase)
        {        
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        if(!isDead && enemyType != Type.D)
        {
            float targetRadius = 0f;
            float targetRange =  0f;

        switch (enemyType)
        {
            case Type.A:
                targetRadius = 1.5f;
                targetRange =  3f;
                break;
            case Type.B:
                targetRadius = 1f;
                targetRange =  12f;
                break;
            case Type.C:
                targetRadius = 0.5f;
                targetRange =  25f;
                break;
        }

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player")); 

        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;

        if (anim != null)
        {
            anim.SetBool("isAttack", true);
        };

        switch (enemyType) 
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);

                if(meleeArea !=null)
                {
                    meleeArea.enabled = true;
                }

                yield return new WaitForSeconds(1f);

                if(meleeArea !=null)
                {
                    meleeArea.enabled = false;
                }

                yield return new WaitForSeconds(1f);
                break;

            case Type.B:
                yield return new WaitForSeconds(0.1f);

                if(rigid != null)
                {
                    rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                }

                if(meleeArea != null)
                {
                    meleeArea.enabled = true;
                }

                yield return new WaitForSeconds(0.5f);

                if(rigid != null)
                {
                    rigid.velocity = Vector3.zero;
                }

                if (meleeArea != null)
                {
                    meleeArea.enabled = false;
                }

                yield return new WaitForSeconds(2f);
                break;

            case Type.C:
                yield return new WaitForSeconds(0.5f);

                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);

                if(instantBullet != null)
                {
                    Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();

                    if (rigidBullet != null)
                    {
                        rigidBullet.velocity = transform.forward * 20;
                    }
                }

                yield return new WaitForSeconds(2f);
                break;
        }

        isChase = true;
        isAttack = false;

        if(anim != null)
        {
            anim.SetBool("isAttack", false);
        }
    }

    void FixedUpdate() 
    {
        Targeting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, false));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach(MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            foreach(MeshRenderer mesh in meshs)
                {
                    mesh.material.color = Color.white;
                }

            yield return new WaitForSeconds(0.1f);
        }
        else 
        {
            curHealth = 0;
            foreach(MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

            gameObject.layer = 13;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            Player player = target.GetComponent<Player>();
            player.score += score; // 플레이어 스코어 ++
            int ranCoin = Random.Range(0, 3);
            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);

            switch (enemyType)
            {
                case Type.A:
                    manager.enemyCntA--;
                    manager.enemyCntA = Mathf.Max(manager.enemyCntA, 0);
                    break;
                case Type.B:
                    manager.enemyCntB--;
                    manager.enemyCntB = Mathf.Max(manager.enemyCntB, 0);
                    break;
                case Type.C:
                    manager.enemyCntC--;
                    manager.enemyCntC = Mathf.Max(manager.enemyCntC, 0);
                    break;
                case Type.D:
                    manager.enemyCntD--;
                    manager.enemyCntD = Mathf.Max(manager.enemyCntD, 0);
                    break;
            }

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);

                Destroy(gameObject, 4);
        }
    }    
}