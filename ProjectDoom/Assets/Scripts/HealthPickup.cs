﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
