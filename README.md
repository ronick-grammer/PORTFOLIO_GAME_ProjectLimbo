# Project Limbo

<img src = "https://github.com/ronick-grammer/PORTFOLIO_GAME_ProjectLimbo/blob/main/thumbnail_animation.gif">

> ## 개발 환경 및 플랫폼 정보

- 게임 엔진: [Unity 3D](https://unity.com/)

- 스크립트 언어: [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) 

- 3D 모델링 프로그램: [Maya LT™ 3D](https://www.autodesk.co.kr/products/maya-lt/subscribe?plc=MAYALT&term=1-YEAR&support=ADVANCED&quantity=1)

- 플랫폼: PC
<br>

> ## 담당 역할

- 기획: 김영현

- 프로그래밍: 김영현

- 3D 모델링: 김영현
<br>


> ## 주 구현 기능

### [ AI ] 플레이어 감지 컨트롤러

- 플레이어를 감지할 몬스터의 중심 범위안에서 바라보는 방향을 기준으로 또 다른 범위를 만들어 플레이어를 감지할 수 있다.

- 관련 코드: [DetectionController.cs](/Scripts/AI/DetectionController.cs)

<img src = "https://github.com/ronick-grammer/PORTFOLIO_GAME_ProjectLimbo/blob/main/detectionController.gif" width="70%">
<br><br>


### [ AI ] 움직임 경로 컨트롤러

- 몬스터는 만들어진 path 를 따라서 움직인다.

- 공격이 끝나면 path 의 시작점으로 돌아가 다시 경로를 따라 움직인다.

- 관련 코드: [PathFollowingController.cs](/Scripts/AI/PathFollowingController.cs)

<img src = "https://github.com/ronick-grammer/PORTFOLIO_GAME_ProjectLimbo/blob/main/pathFollow.gif" width="70%">
<br><br>


### [ Animation, Dialogue ] 타임라인과 대화 시스템 컨트롤러

- 본 게임에서 많은 부분을 차지 하는 애니메이션 씬을 유니티의 타임라인 기능을 통해서 작업하였다.

- 타임라인을 보다 효과적으로 활용하기 위해 타임라인 컨트롤러 스크립트를 작성하였다.

- 본 게임은 스토리를 포함하고 있기에 대화 시스템을 구현하였다.

- 타임라인과 대화 시스템은 서로 상호 연동이 가능하도록 스크립트를 작성하였다.

- 관련 코드: [TimeLineController.cs](/Scripts/TimeLineControl/TimeLineController.cs)
- 관련 코드: [DialogueSystemContoller.cs](/Scripts/DialogueControl/DialogueSystemContoller.cs)

<img src = "https://github.com/ronick-grammer/PORTFOLIO_GAME_ProjectLimbo/blob/main/timelineNdialogue.gif" width="70%">
