using UnityEngine;

public class PlayerScoreTracker : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Playing)
        {
            GameManager.Instance.UpdateScoreByZPosition(transform.position.z);
        }
    }
}