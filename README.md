# 유니티 좀비 서바이벌
=======================================

## 목록

=======================================

### GIF
![Alt text](/Images/Image_1.gif)
![Alt text](/Images/Image_2.gif)
![Alt text](/Images/Image_3.gif)
![Alt text](/Images/Image_4.gif)
![Alt text](/Images/Image_5.gif)

=======================================

### 무기
|     무기 이름     |  데미지  |   넉백   | 반동 | 총알 | 폭발 범위 | 최소 퍼짐 | 최대 퍼짐 | 속도 | 반자동 |
|:-----------------:|:--------:|:--------:|:----:|:----:|:---------:|:---------:|:---------:|:----:|:------:|
|      Rifle_1      |    7.5   |     1    |  0.5 |  150 |           |    0.1    |     1     | 0.05 |        |
|     Shotgun_1     | 2.5 * 12 | 0.3 * 12 |  0.3 |  40  |           |    0.5    |    7.5    |  0.5 |    O   |
|      Pistol_1     |    4.5   |     1    |  1.5 |  120 |           |    0.25   |     1     |  0.1 |    O   |
|       SMG_1       |    5.5   |    1.5   |  0.5 |  250 |           |    0.25   |     2     | 0.11 |        |
|       SMG_2       |     5    |     1    | 0.75 |  300 |           |    0.1    |    1.5    | 0.06 |        |
|      Rifle_2      |   12.5   |     2    |   2  |  60  |           |    0.05   |    0.5    |  0.1 |    O   |
|       SMG_3       |     4    |     1    | 0.75 |  350 |           |    0.25   |    2.5    | 0.05 |        |
| GrenadeLauncher_1 |    25    |    10    |  10  |  10  |     3     |    0.1    |    0.5    |   2  |    O   |
|  RocketLauncher_1 |    35    |    15    |  15  |   5  |     5     |    0.1    |    0.5    |   2  |    O   |

=======================================

### 상자
![Alt text](/Images/Image_Box_1.png)
![Alt text](/Images/Image_Box_2.png)
![Alt text](/Images/Image_Box_3.png)
![Alt text](/Images/Image_Box_4.png)

맵에 배치 되어있는 상자입니다
상자 근처에 가서 e를 누르면 무기 / 업그레이드를 구입할 수 있습니다
구입을 하면 상자가 밝게 빛나고 랜덤한 무기가 계속 바뀌면서 보여줍니다
시간이 지나면 무기가 바뀌는게 멈춥니다, 이때 e를 눌러 무기 / 업그레이드를 가져갈 수 있습니다
빨리 가져가지 않으면 무기는 상자 속으로 들어가서 사라집니다

무기 상자
- 랜덤한 무기를 줍니다
- 가격 : 750

데미지 증가 상자
- 현재 가지고있는 무기중 하나의 데미지를 증가 시킵니다
- 가격 : 500

탄창 증가 상자
- 현재 가지고있는 무기중 하나의 탄창 크기를 증가 시킵니다
- 가격 : 500

레이저사이트 상자
- 현재 가지고있는 무기중 하나에 레이저사이트를 착용시킵니다
- 가격 : 500

![Alt text](/Images/Image_Crate_1.png)
![Alt text](/Images/Image_Crate_2.png)

웨이브가 끝나면 플레이어 근처로 떨어지는 상자입니다
상자와 같이 e를 구입을 할수 있습니다
상자는 생성된뒤 시간이 지나면 자동으로 사라집니다
생성될 때 마다 다른 가격을 가집니다

보급 상자
- 랜덤한 무기를 줍니다
- 가격 : 무기상자는 500 ~ 1000, 그 외 350 ~ 500

탄약 상자
- 현재 가지고있는 무기중 하나에 총알을 넣어줍니다
- 탄창 증가 상자에서 업그레이드를 했으면 더 많이 줍니다
- 가격 : 150 ~ 250

=======================================
