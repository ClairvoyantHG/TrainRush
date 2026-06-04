using System;
using UnityEngine;

public class ChaserWarningUI : UIBase
{
    [SerializeField] private Animator warningAnimator;

    public void SetWarningHp(int currentHp)
    {
        if (warningAnimator != null)
        {
            warningAnimator.Play("Tentacles_Anim_" + currentHp);
        }
    }
}