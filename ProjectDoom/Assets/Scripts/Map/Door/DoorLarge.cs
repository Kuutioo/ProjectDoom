using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLarge : MonoBehaviour
{
    public event EventHandler OnOpenDoor;
    public event EventHandler OnCloseDoor;

    private Animator animator;
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
    }

    public void CloseDoor()
    {
        OnCloseDoor?.Invoke(this, EventArgs.Empty);
    }
}
