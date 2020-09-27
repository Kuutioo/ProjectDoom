using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController playerCharacterController;

    private Animator pistolAnimator;

    private void Awake()
    {
        pistolAnimator = GetComponent<Animator>();
    }

    public void Start()
    {
        playerCharacterController.OnStartMoving += PlayerCharacterController_OnStartMoving;
        playerCharacterController.OnStopMoving += PlayerCharacterController_OnStopMoving;
        playerCharacterController.OnStartMoving += PlayerCharacterController_OnShoot;
    }

    public void PlayerCharacterController_OnShoot(object sender, System.EventArgs e)
    {
        pistolAnimator.SetTrigger("Shoot");
    }

    private void PlayerCharacterController_OnStopMoving(object sender, System.EventArgs e)
    {
        pistolAnimator.SetBool("IsWalking", false);
    }

    private void PlayerCharacterController_OnStartMoving(object sender, System.EventArgs e)
    {
        pistolAnimator.SetBool("IsWalking", true);
    }
}
