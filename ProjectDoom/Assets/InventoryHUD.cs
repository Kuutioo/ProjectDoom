using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUD : MonoBehaviour
{
    private Inventory inventory;
    private Transform hudImage;
    private Transform itemSlot;

    private void Awake()
    {
        hudImage = transform.Find("HUDImage");
        itemSlot = hudImage.Find("ItemSlot");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    
    private void RefreshInventoryItems()
    {
        foreach(Item item in inventory.GetItemList())
        {
            RectTransform itemslotRectTransform = Instantiate(hudImage, itemSlot).GetComponent<RectTransform>();
            itemslotRectTransform.gameObject.SetActive(true);
        }
    }
}
