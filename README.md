Quarter View FPS Game

'골드메탈'님의 강의를 듣고 Customizing 및 버그 개선을 하는 레포지토리입니다.

2023-02-27 현재 버그 

- 적 피격시 색깔이 빨간색으로 변하지 않음.                          *********** 해결

- 플레이어 사망시 재시작 패널에서 최고 점수 갱신 불가능                 ********* inspector 창에서 참조값 수정

- 보스 클리어 시 대기 화면에서 보스 미사일 잔재함.                  ******** 2023-03-10 해결

 
		
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

- 종료버튼 - 생성완료 
  게임 시작시 사운드 추가 - 생성완료 
  행동 사운드 -  
  보스 분기별로 체력 증가 알고리즘 - 생성완료
 
2023-03-09 업데이트

- StopBackgroundMusic.CS 추가하여 배경음 추가 및 StageZone 진입 시 CompareTag("Plaer")를 감지해 BGM off

2023-03-10 업데이트

- BossMissile 스크립트에 Time.time 함수를 받아와서 5초 후 보스 미사일을 삭제하게 함. <2023-02-27 이슈>

- <ESC>를 통한 메뉴 UI 및 기능 추가

  input manager에 cancel이 지원되므로, 굳이 Escape 추가 필요 x
  esc 클릭 시, 게임종료 메뉴와 취소 메뉴 추가 ㅡ> 메뉴 클릭시 총알이 발사되는 이슈 발견
  Time.timeScale = 0f or 1f; 추가하여 해결. ㅡ> 무조건 한발은 발사가 됨
  Open과 Close 함수에 bool 함수 추가 후, Player 스크립트에서 Manager스크립트 접근 후, 행동에 직접 제어 ex: && !manager.escPanelOpen
  
2023-03-15 업데이트

- 등장별 체력 증가 중첩 알고리즘
	
	배틀 시작 후 등장 횟수를 확인하여 적용
	ㅡ> UI장애 발생
	
		bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
		
		값에서 음수가 발생하여 체력바가 이탈하는 현상 발생
		
		ㅡ> float healthPercentage = Mathf.Clamp((float)boss.curHealth / boss.maxHealth, 0, 1);
            bossHealthBar.localScale = new Vector3(healthPercentage, 1, 1); 
			
			변경 후 장애 해결.
		
	추후 장애 방지를 위하여 Enemy 스크립트의 IEnumerator OnDamage(Vector3 reactVec, bool isGrenade) 함수 내에
	데미지를 입어도 0 이하로 떨어지지 않는 코드 삽입.
	
2023-03-16 업데이트

- 하늘 추가
- 메뉴캠 에니메이션 변경
- 벽 알파값 조정

렌더링 이슈 발생 ㅡ> Zone의 파티클 및 Text가 사라지는 현상
                -> zone의 파티클 Order in Layer 변경
				-> Legacy Text제거 및 TMP layer 설정
				ㅡ> 장애 해결

총알 이슈 발생 ㅡ> ex)curAmmo의 최댓값 7, maxAmmo의 최댓값 30으로 설정된 총이 curAmmo 6인 상태에서 장전하게 되면,
				  7/23 으로 변경되는 이슈 발생.
				  
				  // 현재 탄창에 빈 공간이 있는지 확인.
					int emptySpaceInCurAmmo = equipWeapon.maxAmmo - equipWeapon.curAmmo;

					// maxAmmo에서 충분한 탄약이 있는지 확인하고, 필요한 만큼만 가져온다. 더 작은값 반환
					int ammoToReload = Mathf.Min(emptySpaceInCurAmmo, ammo);

					// 탄창을 채우고 남은 탄약을 ammo에서 차감합니다.
					equipWeapon.curAmmo += ammoToReload;
					ammo -= ammoToReload;
					
				ㅡ> 코드 변경 후 이슈 해결.
				
2023-03-17 업데이트

- 달리기사운드 추가
- 점프 사운드 추가
- 회피 사운드 추가   ㅡ> 3개 모두 객체 생성 후 오디오 클립으로 직접 연결 결과, 싱크 안 맞음.

				AnimationEventController 생성 후, Player Mesh Object에 추가.
				
				Audio Clip 별 Event 추가 후 Function 추가
				
				Audacity를 통하여 효과음 편집
			
- 장애발견: 점프중 공격시 미끄러짐현상 -> 점프중일때 공격 못하게 수정
