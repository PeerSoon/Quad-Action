Quarter View FPS Game

'골드메탈'님의 강의를 듣고 Customizing 및 버그 개선을 하는 레포지토리입니다.

2023-02-27 현재 버그 

- 적 피격시 색깔이 빨간색으로 변하지 않음.                          *********** 해결

- 플레이어 사망시 재시작 패널에서 최고 점수 갱신 불가능                 ********* inspector 창에서 참조값 실수

- 보스 클리어 시 대기 화면에서 보스 미사일 잔재함.                  ******** 2023-03-10 해결

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

- 플레이어 사망 시 4초 후 캐릭터가 없어지도록 GameOver 함수에 Invoke를 통하여 제어함.

public void playerBye()
    {
        player.gameObject.SetActive(false);
    }
생성	

2023-03-03 업데이트 목록

- StageStart 사운드 및 StageEnd 사운드 추가.

- 해머, 핸드건, 서브머신건 사운드 추가
	switch 문으로 Melee를 제외한 Range 공격에서 equipWeaponIndex를 받아와서 if (현재 총알의 개수 > 0) 참일시 사운드 On
	
2023-03-04 업데이트 예정 목록

- 종료버튼, 게임 시작시 사운드 추가, 행동 사운드, 보스 분기별로 체력 증가 알고리즘
 
2023-03-09 업데이트

- StopBackgroundMusic.CS 추가하여 배경음 추가 및 StageZone 진입 시 CompareTag("Plaer")를 감지해 BGM off

2023-03-10 업데이트

- BossMissile 스크립트에 Time.time 함수를 받아와서 5초 후 보스 미사일을 삭제하게 함. <2023-02-27 이슈>

- <ESCape>를 통한 메뉴 UI 및 기능 추가

  input manager에 cancel이 지원되므로, 굳이 Escape 추가 필요 x
  esc 클릭 시, 게임종료 메뉴와 취소 메뉴 추가 ㅡ> 메뉴 클릭시 총알이 발사되는 이슈 발견
  Time.timeScale = 0f or 1f; 추가하여 해결. ㅡ> 무조건 한발은 발사가 됨
  Open과 Close 함수에 bool 함수 추가 후, Player 스크립트에서 Manager스크립트 접근 후, 행동에 직접 제어 ex: && !manager.escPanelOpen
