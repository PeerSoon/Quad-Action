Quarter View FPS Game

'골드메탈'님의 강의를 듣고 개발한 게임을 Customizing 및 버그 개선을 하는 레포지토리입니다.

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
			
- 장애발견: 몬스터와 충돌 시 미끄러짐 현상 발생
			-------------------------------------
			Meterial 생성 후 Dynamic Friction, Static Friction 1으로 설정 후 적용 ㅡㅡ> 
			ㅡ> 효과없음.
			-------------------------------------------
					/*void OnAttacked()
			{
				currentState = PlayerState.Attacked;
				// 움직임 멈추기 (예: 리지드바디의 속도를 0으로 설정)
				moveVec = Vector3.zero;
		
				회복 코루틴 실행 (예: 2초 후에 회복)
				StartCoroutine(RecoverFromAttack(0.1f));
			}
		
			IEnumerator RecoverFromAttack(float recoveryTime)
			{
				yield return new WaitForSeconds(recoveryTime);
				currentState = PlayerState.Idle;
			}*/
			
			공격 받을 시 벡터값을 0으로 조절하는 스크립트 적용
			ㅡ> 효과없음. 대신 몬스터와 접촉시 경직이라는 핸디캡 적용 (1f초)
			-------------------------------------------------
			/*void OnCollisionStay(Collision collision)
			{
				if (collision.gameObject.CompareTag("Enemy"))
				{
					Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
					float distance = Vector3.Distance(transform.position, collision.transform.position);
		
					if (distance < minDistance)
					{
						GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Force);
						collision.gameObject.GetComponent<Rigidbody>().AddForce(-pushDirection * pushForce, ForceMode.Force);
					}
				}
			}*/
			
			적과 나의 거리를 받아와 최소 거리보다 가까워지면 밀어내는 로직 구현.
			ㅡ> 효과없음
			-------------------------------------------------------------
			void OnCollisionEnter(Collision collision) 내부 로직에
			
			StartCoroutine(PreventSliding()); 추가함.
			
			private IEnumerator PreventSliding()    // 몬스터에게 닿으면 rigidbody 비활성화
			{
				GetComponent<Rigidbody>().isKinematic = true;
				yield return new WaitForSeconds(0.5f);
				GetComponent<Rigidbody>().isKinematic = false;
			}
			
			ㅡ> rigidbody의 물리연산을 잠시 해제함으로 이슈 해결 +  Friction Combin 값 조정 시 해결
			
2023-03-21 업데이트

 - 맵 업데이트
 
	사이즈가 더 큰 새로운 맵 생성 및 조형물 배치
	
 - 이슈
 
	1. *이슈해결 (물리연산 업데이트 보다 프레임당 진행 속도가 빠른것이 원인이라 추정중)
 
	-Player가 조형물에 막혀도 계속하여 돌진하면 뚫리는 버그
	
	조형물은 계단을 포함하고 있는 복잡한 구조로, Mesh Collider를 사용중. (Convex X)
	Physic Material 생성 후 Friction 올려준 후에 플레이어, 조형물에 적용해도 x 
	
	void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall", "Statue"));
    }
	
	Statue 추가 후 레이어 Statue 추가.
	
	계단이 있는 조형물의 경우, 계단 위로의 접근 자체가 불가능해짐
	
	기존 Player 움직임 제어 코드 'transform.position += moveVec * speed * (wDown ? 0.3f : 1) * Time.deltaTime;' 를 
	ㅡ> rigid.MovePosition(rigid.position + moveVec * (speed * (wDown ? 0.3f : 1)) * Time.deltaTime); 변경
	해결안됨.
	
	Move() 함수를 Update() ㅡ> FixedUpdate()로 변경
	해결안됨.
	
	RigidBody의 InterPlate를 None ㅡ> ExtraPolate 로 변경 (물리엔진을 1프레임 먼저 계산하기 때문)
	해결안됨.
	
	Collision Detection을 Continuous로 변경
	해결안됨.
	
	Edit -> Project Settings -> Time을 0.02 ㅡ> 0.002 변경
	해결안됨.
	
	float currentSpeed = speed * (wDown ? 0.3f : 1);
    Vector3 targetVelocity = moveVec * currentSpeed;
            
    rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(targetVelocity.x, rigid.velocity.y, targetVelocity.z), Time.fixedDeltaTime * 10f); 
	
	** velocity 값으로 플레이어 움직임 제어 후 이슈 해결 - 2023-03-22
	
	
	
	
	2. *이슈해결
	
	-다운로드한 모델과 현재 버전과의 차이때문에 제대로 렌더링이 되지 않음.
	
	렌더링 지식이 아직 부족하여 여러가지 시도중 ex) Material Shader 변경, Rendering Mode 변경 등
	
	** Rendering Mode: transparent ㅡ> cutout 을 사용하여 이슈 해결함
	
	- opacue: 		완전히 불투명한 물체를 렌더링하는 기본 모드
					투명성이 없음
					성능이 가장 좋음
					렌더링 순서에 덜 민감하고 정렬 문제가 적음
	- cutout:   	경계를 생성
					일반적으로 풀이지, 울타리, 리프 등의 물체에 사용
					알파 테스트를 사용하여 투명성을 처리하며, 부분적인 투명성은 지원하지 않음
					성능이 상대적으로 좋음
	- fade:     	물체의 투명도를 조절하여 부분적인 투명성을 지원
					투명도에 따른 블렌딩을 사용하여 물체 간의 투명 효과를 처리
					일반적으로 페이드 인/아웃 효과에 사용
					성능이 Cutout보다 약간 떨어지며, 정렬 문제에 민감할 수 있음
	- transparent:  완전한 투명 물체 또는 반투명 물체를 렌더링하는 데 사용
					투명도에 따른 블렌딩을 사용하여 물체 간의 투명 효과를 처리
					유리, 물, 입자 효과 등 투명 또는 반투명 물체에 적합
					성능이 상대적으로 낮으며, 정렬 문제에 매우 민감함
					
2023-03-22 업데이트

 - 21일 이슈 해결 진행 과정
 
 - 새로운 이슈 발견 
 
	- MissingReferenceException: 몬스터의 공격 코루틴에서 객체가 파괴된 후 여전히 해당 객체에 접근하려함.
	  ㅡ> 스크립트가 파괴된 객체를 참조하지 않도록 수정해야함.
	  
	  Enemy Script의 Attack() 코루틴 동작에 null Check 추가 후 이슈 해결
	  
	-1 새로운 맵의 조형물(계단을 포함한)의 경우, Nav AI BAKE 오류
	-2 계단 올라가기 불가능.
	-3 조형물의 위로 올라가서 총 발사 시, Y축 ray 오류로 맞지 않음. 
	  
	  
	
 - 맵 이동 로직
 
	- MapManager 스크립트 생성
	
		GameMagager.stage 값을 받아와서 11>= 이상일 시, 2번째 맵으로 변경
		
		기존의 Enemy zone Group, Start Zone, Item Shop, Weapon Shop, Player, Enemy Zone Group을 MapManager 오브젝트 하위에 관리.
		
		MapManager에 MapManager 스크립트 적용 - 전체적인 위치 이동
		세밀한 조정이 필요한 객체들에게 offset 값 조절 스크립트 ChildMapAdjuster 적용.
		
		Game Manager의 StageEnd()에 mapManager.StartNewRound(); 추가
		
		Work Flow: MapManager 이동 ㅡ> 자식 정밀 조정 ㅡ> Enemy zone 임시 활성화 후 조정
	
	new map position: 200, 0, -200
	
	추후 새로운 맵 개발 시, 확장성을 고려해 마이그레이션 고민중
	
2023-03-23 업데이트

	- 22일 이슈 해결 과정
		
	 1.	(해결) Unity's NavMesh Components GitHub repository에서 NavMesh Components를 다운로드
			  복잡한 구조물에 새로운 컴포넌트 추가: NavMeshSurface, NavMeshLink
	
	 2. (진행중)
	 
	 3. (진행중)
		
		
