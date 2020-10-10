using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGlobal : MonoBehaviour
{
    [SerializeField] private DoorLarge door;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            door.OpenDoor();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            door.CloseDoor();
        }
    }
}
