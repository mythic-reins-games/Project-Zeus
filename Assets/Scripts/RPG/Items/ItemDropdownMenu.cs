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

    // Start is called before the first frame update
    void Start()
    {
        PopulateOptions();
    }

    public void PopulateOptions()
    {
        List<string> options = new List<string>() { "Nothing" };
        CharacterSheet activeChar = CharsheetUIInitializer.characterBeingDisplayed;
        switch (slot)
        {
            case Item.ItemSlot.ringLeft:
                if (activeChar.ringLeftEquipped != null)
                    options.Add(activeChar.ringLeftEquipped.DisplayName());
                break;
            case Item.ItemSlot.ringRight:
                if (activeChar.ringRightEquipped != null)
                    options.Add(activeChar.ringRightEquipped.DisplayName());
                break;
        }
        foreach (Item item in PlayerParty.inventory)
        {
            options.Add(item.DisplayName());
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void DropdownValueChanged()
    {
        Debug.Log(dropdown.value);
        int itemIdx = dropdown.value;
        CharacterSheet activeChar = CharsheetUIInitializer.characterBeingDisplayed;
        if (itemIdx == -1)
        {
            activeChar.UnequipSlot(slot);
            return;
        }
        Item item = PlayerParty.inventory[itemIdx];
        item.EquipSelf(activeChar, slot);
        PopulateOptions();
    }
}
