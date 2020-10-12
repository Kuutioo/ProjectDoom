using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTypeManager : MonoBehaviour, DoorInterface
{
    public event EventHandler OnOpenDoor;
    public event EventHandler OnCloseDoor;
    
    private Animator animator;

    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        OnOpenDoor += DoorLarge_OnOpenDoor;
        OnCloseDoor += DoorLarge_OnCloseDoor;
    }

    private void DoorLarge_OnOpenDoor(object sender, EventArgs e)
    {
        animator.SetBool("Open", true);
    }

    private void DoorLarge_OnCloseDoor(object sender, EventArgs e)
    {
        animator.SetBool("Open", false);
    }

    public void OpenDoor()
    {
        OnOpenDoor?.Invoke(this, EventArgs.Empty);
        isOpen = true;
    }

    public void CloseDoor()
    {
        OnCloseDoor?.Invoke(this, EventArgs.Empty);
        isOpen = false;
    }
    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
}
