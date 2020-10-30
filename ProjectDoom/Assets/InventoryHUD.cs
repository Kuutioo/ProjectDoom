using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController player;

    private Text smallHealText;

    private void Awake()
    {
        smallHealText = transform.Find("SmallHealText").GetComponent<Text>();
        player.SmallHealCountUpdated += InventoryHUD_SmallHealCountUpdated;

        Refresh();
    }

    private void InventoryHUD_SmallHealCountUpdated()
    {
        Refresh();
    }

    private void Refresh()
    {
        player.AddValue();
        smallHealText.text = player.GetSmallHealCount().ToString();
    }
}
