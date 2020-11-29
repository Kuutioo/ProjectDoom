using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        SmallHeal,
        MediumHeal
    }

    public ItemType itemType;
    public int amount;

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.SmallHeal:
            case ItemType.MediumHeal:
                return true;
        }
    }
}
