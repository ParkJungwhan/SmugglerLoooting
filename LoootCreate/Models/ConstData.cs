namespace LoootCreate.Models;

public static class LOTTERYCONSTANTS
{
    // 2025까지 사용 api
    //"http://www.dhlottery.co.kr/common.do?method=getLottoNumber&drwNo=";

    /// <summary>
    /// https://www.dhlottery.co.kr/lt645/selectPstLt645Info.do?srchLtEpsd=1200 // 1204 : 회차번호
    /// </summary>
    public static string KR2026_LOTTERY_URL =
    "https://www.dhlottery.co.kr/lt645/selectPstLt645Info.do?srchLtEpsd={0}";   //{0} : 1204 : 회차번호

    /// <summary>
    /// https://www.dhlottery.co.kr/lt645/selectPstLt645Info.do?srchStrLtEpsd=1200&srchEndLtEpsd=1209 // 1200 ~ 1209 회차 조회. 1부터 지금까지 전부 다 가능
    /// </summary>
    public static string KR2026_LOTTERY_RANGE_URL =
        "https://www.dhlottery.co.kr/lt645/selectPstLt645Info.do?srchStrLtEpsd={0}&srchEndLtEpsd={1}"; //0 : start, 1: End
}