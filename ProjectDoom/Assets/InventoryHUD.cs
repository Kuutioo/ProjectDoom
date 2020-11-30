using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryHUD : MonoBehaviour
{
    private Inventory inventory;
    private Transform hudImage;
    private Transform itemSlot;
    private Transform itemTemplate;

    private void Awake()
    {
        hudImage = transform.Find("HUDImage");
        itemSlot = hudImage.Find("ItemSlot");
        itemTemplate = itemSlot.Find("ItemTemplate");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.ItemListUpdated += Inventory_ItemListUpdated;

        RefreshInventoryItems();
    }

    private void Inventory_ItemListUpdated()
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlot)
        {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }

        float x = 0.5f;
        float y = -1.75f;
        float itemSlotCellSize = 40f;

        foreach (Item item in inventory.GetItemList())
        {

            RectTransform itemSlotRectTransform = Instantiate(itemTemplate, itemSlot).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

            TextMeshProUGUI text = itemSlotRectTransform.Find("ItemTemplateText").GetComponent<TextMeshProUGUI>();
            text.SetText(item.amount.ToString());

            y++;
        }
    }
}
