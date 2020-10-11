using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDoorHandler : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRadius = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(playerTransform.transform.position, interactRadius);

            foreach(Collider collider in colliderArray)
            {
                DoorInterface doorInterface = collider.GetComponent<DoorInterface>();
                if(doorInterface != null)
                {
                    StartCoroutine(CloseDoorWaitTimer(collider));
                    doorInterface.OpenDoor();
                }
            }
        }
    }

    private IEnumerator CloseDoorWaitTimer(Collider collider)
    {
        DoorInterface doorInterface = collider.GetComponent<DoorInterface>();
        yield return new WaitForSeconds(5.0f);
        doorInterface.CloseDoor();
    }
}
