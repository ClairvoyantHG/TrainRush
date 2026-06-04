using System;
using System.Collections.Generic;
using UnityEngine;

// 중력 관리 매니저
public class GravityManager : SingletonBase<GravityManager>
{
    [SerializeField] private float activationDistance = 40f;            // 중력 적용 범위
    public float ActivationDistance { get { return activationDistance; } }

    private GravityDirection currentGravity = GravityDirection.Down;    // 현재 적용중인 중력 
    public GravityDirection CurrentGravity { get { return currentGravity; } }

    //private List<IGravityAffected> affectedObjects = new List<IGravityAffected>();    // 중력의 영향을 받을 객체 리스트

    private IInputProvider inputProvider;
    private event Action<GravityDirection> onGravityChanged;

    public void BindEventOnGravity(Action<GravityDirection> CallBack)
    {
        onGravityChanged += CallBack;
    }

    public void UnbindEventOnGravity(Action<GravityDirection> CallBack)
    {
        onGravityChanged -= CallBack;
    }
    private void Start()
    {
        inputProvider = PlayerInputManager.Instance;

        if (inputProvider == null)
        {
            this.enabled = false;
        }
    }

    // 객체가 중력의 영향을 받아야할 때 구독
    //public void Register(IGravityAffected affectedObject)
    //{
    //    if (!affectedObjects.Contains(affectedObject))
    //    {
    //        affectedObjects.Add(affectedObject);
    //    }
    //}

    //// 객체가 중력 영향을 안 받을 때 구독 해제
    //public void Unregister(IGravityAffected affectedObject)
    //{
    //    if (affectedObjects.Contains(affectedObject))
    //    {
    //        affectedObjects.Remove(affectedObject);
    //    }
    //}

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing) return;
        // 입력에 따라 중력 전환 실행
        if (inputProvider.GetGravityInput(GravityDirection.Up)) ChangeGravity(GravityDirection.Up);
        else if (inputProvider.GetGravityInput(GravityDirection.Down)) ChangeGravity(GravityDirection.Down);
        else if (inputProvider.GetGravityInput(GravityDirection.Left)) ChangeGravity(GravityDirection.Left);
        else if (inputProvider.GetGravityInput(GravityDirection.Right)) ChangeGravity(GravityDirection.Right);
    }

    // 중력 방향 전환 시 구독 중인 모든 객체들에게 새로운 방향을 일제히 통보
    private void ChangeGravity(GravityDirection newGravity)
    {

        //foreach (IGravityAffected affectedObject in affectedObjects)
        //{
        //    affectedObject.OnGravityChanged(currentGravity);
        //}

        if (currentGravity == newGravity) return;

        currentGravity = newGravity;
        onGravityChanged?.Invoke(currentGravity);
    }
}