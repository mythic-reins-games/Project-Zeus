using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropdownMenu : MonoBehaviour
{

    [SerializeField]
    Item.ItemSlot slot;

    [SerializeField]
    Dropdown dropdown;

    [SerializeField]
    GameObject siblingMenu;

    List<string> options;
    List<Item> items;

    // Start is called before the first frame update
    void Start()
    {
        options = new List<string>() { };
        items = new List<Item>() { };
        PopulateOptions();
        dropdown.onValueChanged.AddListener(DropdownValueChanged);
    }

    public void PopulateOptions()
    {
        options.Clear(); // Options is a list of the display names of the items.
        items.Clear(); // Items is a list of references to the actual items.
        CharacterSheet activeChar = CharsheetUIInitializer.characterBeingDisplayed;
        switch (slot)
        {
            case Item.ItemSlot.ringLeft:
                if (activeChar.ringLeftEquipped != null)
                {
                    items.Add(activeChar.ringLeftEquipped);
                    options.Add(activeChar.ringLeftEquipped.DisplayName());
                }
                break;
            case Item.ItemSlot.ringRight:
                if (activeChar.ringRightEquipped != null)
                {
                    items.Add(activeChar.ringRightEquipped);
                    options.Add(activeChar.ringRightEquipped.DisplayName());
                }
                break;
        }
        options.Add("None");
        items.Add(null);
        foreach (Item item in PlayerParty.inventory)
        {
            items.Add(item);
            options.Add(item.DisplayName());
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void DropdownValueChanged(int optSelected)
    {
        Item picked = items[optSelected];
        CharacterSheet activeChar = CharsheetUIInitializer.characterBeingDisplayed;
        if (picked == null) // First option is "nothing" - unequips the slot.
        {
            switch (slot)
            {
                case Item.ItemSlot.ringLeft:
                    if (activeChar.ringLeftEquipped != null)
                        activeChar.ringLeftEquipped.UnequipSelf(activeChar, slot);
                    break;
                case Item.ItemSlot.ringRight:
                    if (activeChar.ringRightEquipped != null)
                        activeChar.ringRightEquipped.UnequipSelf(activeChar, slot);
                    break;
            }
        }
        else picked.EquipSelf(activeChar, slot);
        if (siblingMenu != null)
            siblingMenu.GetComponent<ItemDropdownMenu>().PopulateOptions();
        PopulateOptions();
    }
}
