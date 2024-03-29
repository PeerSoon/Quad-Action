using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenades;
    public GameObject grenadeObj;
    public Camera followCamera;
    public GameManager manager;

    public int ammo;
    public int coin;
    public int health;
    public int score;
    
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;
    public AudioSource MoveSound;
    //public AudioSource JumpSound;
    //public AudioSource DodgeSound;
    public AudioSource HammerSound;
    public AudioSource handgunSound;
    public AudioSource subgunSound;    

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDwon;
    bool fDown;
    bool gDown;
    bool rDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool eDown;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage;
    bool isShop;
    bool isDead;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;

    Animator anim;
    MeshRenderer[] meshs;

    GameObject nearObject;
    public Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;
    public enum PlayerState {Idle, Attacked}
    public PlayerState currentState = PlayerState.Idle;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        
        //Debug.Log(PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", 0);
    }

    void Update()
    {
       GetInput();
       //Move();
       Turn();
       Jump();
       Grenade();
       Attack();
       Reload();
       Dodge();
       Swap();
       Interation();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");  // GetAxisRaw()는 Axis값을 정수로 반환하는 함수임.
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDwon = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        eDown = Input.GetButtonDown("Cancel");
    }

    void Move()
    {
        if (currentState == PlayerState.Attacked) return;
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; // 대각선 방향으로 갈 때, 루트 2의 값으로 더 빠르므로, normalized 함수를 통하여 정규화 시킴(1)

        if(isDodge)
            {moveVec = dodgeVec;}

        if(isSwap || isReload || !isFireReady || isDead)
            {moveVec = Vector3.zero;}

        if(!isBorder)
        {
            float currentSpeed = speed * (wDown ? 0.6f : 1);
            Vector3 targetVelocity = moveVec * currentSpeed;
            
            rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(targetVelocity.x, rigid.velocity.y, targetVelocity.z), Time.fixedDeltaTime * 10f); 
            //시작 벡터, 목표벡터, 보간 값
        }
            //rigid.MovePosition(rigid.position + moveVec * (speed * (wDown ? 0.3f : 1)) * Time.fixedDeltaTime);
            //transform.position += moveVec * speed * (wDown ? 0.3f : 1) * Time.deltaTime; //!3항 연산자 사용법

        if (moveVec == Vector3.zero || isDodge || isJump || wDown)  
        {
            MoveSound.Stop();
        }

        /*if (!wDown && moveVec != Vector3.zero)
        {
            anim.ResetTrigger("isWalk");
            anim.ResetTrigger("idle");
            anim.SetTrigger("isRun");
        }
        else if (moveVec != Vector3.zero)
        {
            anim.ResetTrigger("isRun");
            anim.ResetTrigger("idle");
            anim.SetTrigger("isWalk");
        }
        else if (moveVec == Vector3.zero)
        {
            anim.ResetTrigger("isRun");
            anim.ResetTrigger("isWalk");
            anim.SetTrigger("idle");
        } */
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        //#1. 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec); // 바라보는 방향으로 회전시켜주는 함수. 즉 현재 포지션에서 moveVec으로 나아갈 방향으로 바라 봄.

        //#2. 마우스에 의한 회전
        if(fDown && !isDead && !manager.escPanelOpen){
        Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if(Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }
        }
    }

    void Jump()
    {
        if (jDwon && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap  && !isShop && !isDead)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

            //JumpSound.Play();
        }
    }
    void Grenade()
    {
        if(hasGrenades == 0)
        return;

        if(gDown && !isReload && !isSwap && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if(Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                anim.SetTrigger("doThrow");
                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }

    void Attack()
    {
        if(equipWeapon == null)
            return;
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay; // bool값 적용.

        if(fDown && isFireReady && !isDodge && !isSwap && !isShop && !isDead && !isJump && !manager.escPanelOpen)
        {
            // 스위치 문으로 웨폰 타입 별 공격 사운드 별도 적용.
            switch(equipWeaponIndex)
            {
                case 0:                   
                    HammerSound.Play();
                    break;
                case 1:
                    if(equipWeapon.curAmmo > 0)
                        handgunSound.Play();
                    break;
                case 2:
                    if(equipWeapon.curAmmo > 0)
                        subgunSound.Play();
                    break;               
            }          
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;            
        }
    }

    void Reload()
    {
        if(equipWeapon == null)
            return;
        
        if(equipWeapon.type == Weapon.Type.Melee)
            return;

        if(ammo == 0)
            return;

        if(rDown && !isJump && !isDodge && !isSwap && isFireReady  && !isShop && !isDead)     
        {
             anim.SetTrigger("doReload");
             isReload = true;

             Invoke("ReloadOut", 1f);
        }          
    }

    void ReloadOut()
    {
        // 현재 탄창에 빈 공간이 있는지 확인.
        int emptySpaceInCurAmmo = equipWeapon.maxAmmo - equipWeapon.curAmmo;

        // maxAmmo에서 충분한 탄약이 있는지 확인하고, 필요한 만큼만 가져온다. 더 작은값 반환
        int ammoToReload = Mathf.Min(emptySpaceInCurAmmo, ammo);

        // 탄창을 채우고 남은 탄약을 ammo에서 차감.
        equipWeapon.curAmmo += ammoToReload;
        ammo -= ammoToReload;

        isReload = false;
    }

    void Dodge()
    {
        if (jDwon && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap && !isShop && !isDead)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;
            //DodgeSound.Play();
            Invoke("DodgeOut", 0.6f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if(sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if(sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge && !isShop && !isDead)
        {
            if(equipWeapon != null)
                {equipWeapon.gameObject.SetActive(false);}

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        if(iDown && nearObject != null && !isJump && !isDodge && !isDead)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
            else if(nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                isShop = true;
            } 
        }
    }
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void StopToWall()
    {
        //Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall", "Statue"));
    }
    void FixedUpdate() 
    {
        FreezeRotation();
        StopToWall();
        Move();
    }

    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Stairs")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 몬스터와 충돌 시 공격을 받은 것으로 판단하고 처리
            StartCoroutine(PreventSliding());
            OnAttacked();
        }
        /*if (collision.gameObject.CompareTag("Statue"))
        {           
            StartCoroutine(PreventSliding());  
        }*/
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch(item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if(ammo > maxAmmo)
                       ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if(coin > maxCoin)
                       coin = maxCoin;                
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if(health > maxHealth)
                       health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    if(hasGrenades == maxHasGrenades)
                    return;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades  += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if(other.tag == "EnemyBullet")
        {
            if(!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                
                bool isBossAtk = other.name == "Boss Melee Area";
                StartCoroutine(OnDamage(isBossAtk));
            }

            if(other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);
        }
    }

    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }

        if(isBossAtk)
        {
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
        }

        if(health <= 0 && !isDead)
        {
            OnDie();
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }

        if(isBossAtk)
            rigid.velocity = Vector3.zero;    
        }

    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();
    }

    void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Weapon" || other.tag == "Shop")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Weapon"))
        {
            nearObject = null;
        }
        else if (other.CompareTag("Shop"))
        {
            if (nearObject != null)
            {
                Shop shop = nearObject.GetComponent<Shop>();
                if (shop != null)
                {
                    shop.Exit();
                }
                isShop = false;
                nearObject = null;
            }
        }
    }

    void OnAttacked()
    {
        currentState = PlayerState.Attacked;
        // 움직임 멈추기 (예: 리지드바디의 속도를 0으로 설정)
        moveVec = Vector3.zero;

         //회복 코루틴 실행 (예: 1초 후에 회복)
        StartCoroutine(RecoverFromAttack(0.3f));
    }

    IEnumerator RecoverFromAttack(float recoveryTime)
    {
        yield return new WaitForSeconds(recoveryTime);
        currentState = PlayerState.Idle;
    }

    private IEnumerator PreventSliding()    // 몬스터에게 닿으면 rigidbody 비활성화
    {
        GetComponent<Rigidbody>().isKinematic = true;
        
        yield return new WaitForSeconds(0.2f);

        GetComponent<Rigidbody>().isKinematic = false;
    }
}
