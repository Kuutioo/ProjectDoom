using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemManager : MonoBehaviour
{
    private Animator animator;

    public event EventHandler OnPistolShoot;
    public event EventHandler OnSmallHeal;


    private void Awake()
    {
        animator = this.GetComponent<Animator>();

        OnPistolShoot += UIItemManager_OnShoot;
        OnSmallHeal += UIItemManager_OnSmallHeal;
    }

    private void UIItemManager_OnShoot(object sender, EventArgs e)
    {
        animator.SetTrigger("Shoot");
    }

    private void UIItemManager_OnSmallHeal(object sender, EventArgs e)
    {
        animator.SetTrigger("SmallHeal");
    }

    public void TriggerShoot()
    {
        OnPistolShoot?.Invoke(this, EventArgs.Empty);
    }

    public void TriggerSmallHeal()
    {
        OnSmallHeal?.Invoke(this, EventArgs.Empty);
    }
}
