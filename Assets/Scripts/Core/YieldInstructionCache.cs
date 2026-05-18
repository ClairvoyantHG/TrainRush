using System.Collections.Generic;
using UnityEngine;

internal static class YieldInstructionCache
{
    // WaitForEndOfFrame, WaitForFixedUpdate 캐싱
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    // 캐싱을 하기위한 Dictionary 선언
    private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>();

    // WaitForSeconds 캐싱 함수
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        // second 값이 캐싱된 적이 있는 지 확인
        if (!_timeInterval.TryGetValue(seconds, out WaitForSeconds waitForSeconds))
        {
            // 캐싱된 적이 없을 때 WaitForSeconds를 new로 생성하여 Dictionary에 추가
            _timeInterval.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
        }

        return waitForSeconds;
    }
}