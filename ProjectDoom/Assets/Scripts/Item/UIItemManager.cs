using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController player;

    private Animator[] animator;

    public int selectedWeapon = 0;
    public int previousSelectedWeapon;

    private void Awake()
    {
        animator = GetComponentsInChildren<Animator>();

        SelectWeapon();

        player.SmallHealed += UIItemManager_OnSmallHeal;
        player.Shooted += UIItemManager_OnShoot;
    }

    private void UIItemManager_OnShoot()
    {
        foreach(Animator anim in animator)
        {
            anim.SetTrigger("Shoot");
        }
    }

    private void UIItemManager_OnSmallHeal()
    {
        previousSelectedWeapon = selectedWeapon;
        selectedWeapon = 1;

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
            foreach(Animator anim in animator)
            {
                player.canShoot = true;
                anim.SetTrigger("SmallHeal");
                float lengthOfAnim = 1.9f;
                Invoke("AfterAnimIsDone", lengthOfAnim);
            }
        }
    }

    private void AfterAnimIsDone()
    {
        selectedWeapon = previousSelectedWeapon;
        SelectWeapon();
        player.canShoot = false;
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
