using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    public float zLength = 1f; // 아이템의 Z축 판정 크기

    private GridPosition currentGrid;
    private float currentZ;

    public GridPosition CurrentGrid { get { return currentGrid; } }
    public float CurrentZ { get { return currentZ; } }

    public void Initialize(GridPosition startPos, float zPos)
    {
        currentGrid = startPos;
        currentZ = zPos;

        // 맵 생성 시 지정된 위치로 이동
        if (GridManager.Instance != null)
        {
            transform.position = GridManager.Instance.GetWorldPosition(currentGrid);
        }
    }

    private void OnEnable()
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RegisterItem(this);
        }
    }

    private void OnDisable()
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.UnregisterItem(this);
        }
    }

    // 플레이어가 획득했을 때 호출
    public void OnPickedUp()
    {
        // 파티클 사운드 재생 등을 여기에 추가할 수 있습니다.
        gameObject.SetActive(false); // 오브젝트 끄기 (또는 풀로 반환)
    }
}