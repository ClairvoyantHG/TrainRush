using System.Collections;
using UnityEngine;
using TMPro; 

public class CountdownUI : UIBase
{
    [SerializeField] private TextMeshProUGUI countdownText; 

    public override void OnOpen()
    {
        base.OnOpen();
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        int count = 3;
        WaitForSecondsRealtime waitOneSecond = new WaitForSecondsRealtime(1f);

        while (count > 0)
        {
            if (countdownText != null) countdownText.text = count.ToString();
            yield return waitOneSecond;
            count--;
        }

        if (countdownText != null) countdownText.text = "START!";
        yield return waitOneSecond;

        UIManager.Instance.CloseUI(UIType.CountdownUI);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }
}