using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        ring2Str,
        ring2End,
        ring2Spe,
        ring2Agi,
        ring2Int
    }

    public enum ItemSlot
    {
        ringLeft,
        ringRight
    }

    private ItemType type;

    // Instantiates the item which then adds itself to the player inventory.
    public Item(ItemType itemType)
    {
        PlayerParty.inventory.Add(this);
        type = itemType;
    }

    public void UnequipSelf(CharacterSheet target, ItemSlot slot)
    {
        switch (type)
        {
            case ItemType.ring2Str:
                target.currentHealth -= 2 * Constants.HEALTH_PER_STRENGTH;
                target.bonusStrength -= 2;
                break;
            case ItemType.ring2End:
                target.currentHealth -= 2 * Constants.HEALTH_PER_STRENGTH;
                target.bonusEndurance -= 2;
                break;
            case ItemType.ring2Spe:
                target.bonusSpeed -= 2;
                break;
            case ItemType.ring2Agi:
                target.bonusAgility -= 2;
                break;
            case ItemType.ring2Int:
                target.bonusIntelligence -= 2;
                break;
        }
        // In case we were taken below 0 health by unequipping item, restore to 1.
        if (target.currentHealth < 0) target.currentHealth = 1;
        target.UnequipSlot(slot);
    }

    public string DisplayName()
    {
        switch (type)
        {
            case ItemType.ring2Str:
                return "+2 str ring";
            case ItemType.ring2End:
                return "+2 end ring";
            case ItemType.ring2Spe:
                return "+2 spe ring";
            case ItemType.ring2Agi:
                return "+2 agi ring";
            case ItemType.ring2Int:
                return "+2 int ring";
        }
        return "Unknown item";
    }

    public void EquipSelf(CharacterSheet target, ItemSlot slot)
    {
        switch (type)
        {
            case ItemType.ring2Str:
                target.currentHealth += 2 * Constants.HEALTH_PER_STRENGTH;
                target.bonusStrength += 2;
                break;
            case ItemType.ring2End:
                target.currentHealth += 2 * Constants.HEALTH_PER_STRENGTH;
                target.bonusEndurance += 2;
                break;
            case ItemType.ring2Spe:
                target.bonusSpeed += 2;
                break;
            case ItemType.ring2Agi:
                target.bonusAgility += 2;
                break;
            case ItemType.ring2Int:
                target.bonusIntelligence += 2;
                break;
        }
        switch (slot)
        {
            case ItemSlot.ringLeft:
                if (target.ringLeftEquipped != null)
                    target.ringLeftEquipped.UnequipSelf(target, slot);
                target.ringLeftEquipped = this;
                break;
            case ItemSlot.ringRight:
                if (target.ringLeftEquipped != null)
                    target.ringRightEquipped.UnequipSelf(target, slot);
                target.ringLeftEquipped = this;
                break;
        }
    }
}
