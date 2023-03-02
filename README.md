# Quad-Action
Quarter View FPS Game

'골드메탈'님의 강의를 듣고 Customizing 및 버그 개선을 하는 레포지토리입니다.

2023-02-27 현재 버그 

- 적 피격시 색깔이 빨간색으로 변하지 않음.                          *********** 해결

- 플레이어 사망시 재시작 패널에서 최고 점수 갱신 불가능                 ********* inspector 창에서 참조값 실수

- 보스 클리어 시 대기 화면에서 보스 미사일 잔재함.

- 웨폰 및 Enemy 데미지 조작 스크립트 찾아야함 //               ****** 해결 Weapon Damage는 플레이어 자식 오브젝트 오른팔에 있음.
															총은 총알 프리팹에 있음.
															Enemy 자식 오브젝트 Bullet 에서 조절 가능.
 
		
2023-03-02 버그 개선 및 현재 버그 

** 해결

- 피격시 색깔 변화 문제 ㅡ> 코루틴에서 피격 당할 시 칠드런 메쉬들 전부 빨강색 변경 후 코루틴 예일드 웨잇포세컨드 0.1f 추가시 버그 개선. 2023-02-27

- public으로 지정한 값에서 참조값이 잘못됨.

** 발견

- 

** 업데이트

-플레이어 사망 시 4초 후 캐릭터가 없어지도록 GameOver 함수에 Invoke를 통하여 제어함.

public void playerBye()
    {
        player.gameObject.SetActive(false);
    }
생성	

2023-03-03 업데이트 목록

-StageStart 사운드 및 StageEnd 사운드 추가.

-해머, 핸드건, 서브머신건 사운드 추가
	switch 문으로 Melee를 제외한 Range 공격에서 equipWeaponIndex를 받아와서 if (현재 총알의 개수 > 0) 참일시 사운드 On
	
