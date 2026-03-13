# SmugglerLoooting

Smuggler 개인 툴 시리즈. 로또 번호를 생성해 텔레그램 봇으로 전달하는 .NET 기반 프로그램입니다.

- 주요 구성: 콘솔 앱, DB 조회, 텔레그램 봇 연동
- DB: PostgreSQL
- Visual Studio 2026(Community)

## NuGet Packages

현재 프로젝트에서 사용하는 주요 NuGet 패키지는 아래와 같습니다.

| Package | Usage |
| --- | --- |
| Dapper | PostgreSQL 대상 경량 ORM 및 쿼리 실행 |
| Npgsql | PostgreSQL 연결 드라이버 |
| Telegram.Bot | 텔레그램 봇 메시지 송수신 |

## Lotto Number Generator

로또 번호 생성은 아래 순서로 동작합니다.

1. `1`부터 `45`까지의 숫자 배열을 준비합니다.
2. `RandomNumberGenerator.Shuffle`로 전체 배열을 무작위로 섞습니다.
3. 섞인 숫자 중 앞의 6개를 선택한 뒤 오름차순으로 정렬합니다.
4. 생성된 6개 번호 조합을 과거 당첨 번호 이력과 비교합니다.
5. 기존 당첨 번호와 중복되면 다시 생성하고, 중복이 없으면 최종 번호로 사용합니다.

즉, 현재 알고리즘은 `무작위 셔플 + 6개 추출 + 정렬 + 과거 당첨 조합 중복 제거` 방식입니다.

## License

This project is licensed under the MIT License.
