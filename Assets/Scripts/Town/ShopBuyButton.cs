using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyButton : MonoBehaviour
{
    [SerializeField]
    Item.ItemType forSale;

    public void ClickBuy()
    {
        if (PlayerParty.gold < 25)
        {
            NotificationPopupSystem.PopupText("Not enough gold");
            return;
        }
        PlayerParty.gold -= 25;
        // Instantiate the item and add it to player inventory.
        new Item(forSale);
    }
}
