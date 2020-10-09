using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDamage : MonoBehaviour
{
    public float damageTimer;

    private void OnTriggerStay(Collider collider)
    {
        damageTimer -= Time.deltaTime;

        if(damageTimer <= 0f)
        {
            damageTimer += 0.06f;

            PlayerCharacterController playerCharacterController = collider.GetComponent<PlayerCharacterController>();

            if(playerCharacterController != null)
            {
                playerCharacterController.Damage(1);
            }
        }
    }
}
