using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController player;

    private Animator[] animator;

    public event EventHandler OnPistolShoot;
    public event EventHandler OnSmallHeal;

    public int selectedWeapon = 0;
    public int previousSelectedWeapon;

    private void Awake()
    {
        animator = GetComponentsInChildren<Animator>();

        SelectWeapon();

        player.Healed += UIItemManager_OnSmallHeal;
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
