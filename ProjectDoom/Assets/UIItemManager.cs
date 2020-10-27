using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemManager : MonoBehaviour
{
    private Animator animator;

    public event EventHandler PistolShoot;

    private void Awake()
    {
        
        animator = this.GetComponent<Animator>();

        PistolShoot += UIItemManager_OnShoot;
    }

    private void UIItemManager_OnShoot(object sender, EventArgs e)
    {
        animator.SetTrigger("Shoot");
    }
}
