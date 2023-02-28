using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;


    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;

    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    public Text curScoreText;
    public Text bestText;

    void Awake()
    {
        enemyList = new List<int>(); // Instant이기 때문에 초기화 필요
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore")); 
    }

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver()  // 게임오버 시, 게임 판넬은 비활성화 게임오버 판넬 활성화
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if(player.score > maxScore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0); // Scene이 하나밖에 없기 때문에 0을 넣으면 다시 초기로 돌아감.
    }

    public void StageStart()
    {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        player.transform.position = Vector3.up * 0.8f;

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;
    }

    IEnumerator InBattle()
    {
        if (stage % 5 == 0)
        {
            enemyCntD++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>(); //프리팹은 인게임 내 속성에 접근 x 고로 다시 타겟을 초기화 해주어야 함.
            enemy.target = player.transform;
            enemy.manager = this;
            boss = instantEnemy.GetComponent<Boss>();
        }
        else
        {
            for(int index=0; index < stage; index++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran) 
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;
               }
            }

                while(enemyList.Count > 0)
            {
                    int ranZone = Random.Range(0, 4);
                    GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                    Enemy enemy = instantEnemy.GetComponent<Enemy>(); //프리팹은 인게임 내 속성에 접근 x 고로 다시 타겟을 초기화 해주어야 함.
                    enemy.target = player.transform;
                    enemy.manager = this;
                    enemyList.RemoveAt(0); // 다 쓴 첫번째 데이터 삭제.
                    yield return new WaitForSeconds(4f); //while 문을 코루틴에 돌릴 때는 꼭 yield return을 포함해야함. 안그러면 1프레임에 다나옴
            }
        }

        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0) //모두 잡을때까지
        {
            yield return null; // 하나의 프레임 역할, 즉 업데이트처럼 된다.
        }

        yield return new WaitForSeconds(4f); // 4초 후 끝

        boss = null;
        StageEnd();
        
    }
    void Update()
    {
        if(isBattle)
        {
            playTime += Time.deltaTime;
        }
    }
    void LateUpdate() // Update()가 끝난 후 호출되는 생명주기, 즉 Update에서 처리된 정보를 받아와서 처리
    {
        //상단 UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE " + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - (hour * 3600)) / 60);
        int second = (int)(playTime % 60);

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second); // 배틀 중에만 타임 체크

        //플레이어 UI
        playerHealthTxt.text = player.health + "/" + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        if(player.equipWeapon == null)
        {
            playerAmmoTxt.text = "-/" + player.ammo;
        }
        else if(player.equipWeapon.type == Weapon.Type.Melee)
        {
            playerAmmoTxt.text = "-/" + player.ammo;
        }
        else 
        {
            playerAmmoTxt.text = player.equipWeapon.curAmmo + "/" + player.ammo;
        }

        //무기 UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0); // 무기 아이콘은 보유 상황에 따라 알파값만 변경 (참이면 1, 거짓이면 0)
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        //몬스터 숫자 UI
        enemyATxt.text = "x " + enemyCntA.ToString();
        enemyBTxt.text = "x " + enemyCntB.ToString();
        enemyCTxt.text = "x " + enemyCntC.ToString();

        //보스 UI
        if(boss != null)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
            bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1); //보스 체력 이미지의 Scale을 남은 체력 비율에 따라 변경, 앵커를 왼쪽으로 맞추어 놓아서 오른쪽에서 사라짐
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200;
        }    
    }
}
