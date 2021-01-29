# Project Limbo


> ## 주 구현 기능

### [ AI ] 플레이어 감지 컨트롤러

- 플레이어를 감지할 몬스터의 중심 범위안에서 바라보는 방향을 기준으로 또 다른 범위를 만들어 플레이어를 감지할 수 있다.

- 관련 코드: [DetectionController.cs](/Scripts/AI/DetectionController.cs)

<img src = "https://github.com/ronick-grammer/PORTFOLIO_GAME_ProjectLimbo/blob/main/detectionController.gif" width="70%">



### [ AI ] 움직임 경로 컨트롤러

- 몬스터는 만들어진 path 를 따라서 움직인다.

- 공격이 끝나면 path 의 시작점으로 돌아가 다시 경로를 따라 움직인다.

- 관련 코드: [PathFollowingController.cs](/Scripts/AI/PathFollowingController.cs)

<img src = "https://github.com/ronick-grammer/PORTFOLIO_GAME_ProjectLimbo/blob/main/navigation.gif" width="70%">



### [ Animation ] 타임라인(Timeline) 컨트롤러

- 본 게임에서 많은 부분을 차지 하는 애니메이션 씬을 유니티의 타임라인 기능을 통해서 작업하였다.

- 타임라인을 보다 효과적으로 활용하기 위해 타임라인 컨트롤러 스크립트를 작업하였다.


### [ Dialogue ] 대화 시스템

- 본 게임은 스토리를 포함하고 있기에 대화 시스템을 구현하였다.

- 타임라인 컨트롤러와 서로 상호 연동을 할 수 있도록 하였다.
