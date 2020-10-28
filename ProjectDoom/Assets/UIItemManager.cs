using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemManager : MonoBehaviour
{
    private Animator[] animator;

    public event EventHandler OnPistolShoot;
    public event EventHandler OnSmallHeal;

    private int selectedWeapon = 0;
    

    private void Awake()
    {
        animator = GetComponentsInChildren<Animator>();

        SelectWeapon();

        OnPistolShoot += UIItemManager_OnShoot;
        OnSmallHeal += UIItemManager_OnSmallHeal;
    }

    private void UIItemManager_OnShoot(object sender, EventArgs e)
    {
        foreach(Animator anim in animator)
        {
            anim.SetTrigger("Shoot");
        }
    }

    private void UIItemManager_OnSmallHeal(object sender, EventArgs e)
    {
        int previousSelectedWeapon = selectedWeapon;
        selectedWeapon++;

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
            foreach(Animator anim in animator)
            {
                anim.SetTrigger("SmallHeal");
                float lengthOfAnim = 1.9f;
                Invoke("AfterAnimIsDone", lengthOfAnim);
            }

        }
    }

    private void AfterAnimIsDone()
    {
        foreach (Animator anim in animator)
        {
            anim.SetBool("IsFinished", true);

            if (anim.GetBool("IsFinished") == true)
            {
                selectedWeapon--;
                SelectWeapon();
            }
        }
    }

    public void TriggerShoot()
    {
        OnPistolShoot?.Invoke(this, EventArgs.Empty);
    }

    public void TriggerSmallHeal()
    {
        OnSmallHeal?.Invoke(this, EventArgs.Empty);
    }

    private void SelectWeapon()
    {
        int i = 0;

        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            i++;
        }
    }
}
