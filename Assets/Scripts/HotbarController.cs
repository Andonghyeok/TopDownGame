using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    public GameObject hotbarPannel;
    public GameObject slotPrefab;
    public int slotcount = 10;

    private ItemDictionary itemDictionary;

    private Key[] hotbarkeys;

    private void Awake()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        hotbarkeys = new Key[slotcount];
        for(int i = 0; i < slotcount; i++)
        {
            hotbarkeys[i] = i < 9 ? (Key)((int)Key.Digit0 + i): Key.Digit0;
        }
    }
    private void Update()
    {
        for (int i = 0; i < slotcount; i++)
        {
            if (Keyboard.current[hotbarkeys[i]].wasPressedThisFrame)
            {
                UsedItemInSlot(i);
            }
        }
    }

    void UsedItemInSlot(int index)
    {
        Slot slot = hotbarPannel.transform.GetChild(index).GetComponent<Slot>();
        // 현재 항목에 아이템이 등록되어 있다면 
        if(slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.useItem();
        }
    }
    public List<InventorySaveData> GetHotbarItem()
    {
        List<InventorySaveData> hotbarData = new List<InventorySaveData>();

        foreach (Transform slotTransfom in hotbarPannel.transform)
        {
            Slot slot = slotTransfom.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarData.Add(new InventorySaveData { itemId = item.Id, slotIndex = slotTransfom.GetSiblingIndex() });
            }
        }
        return hotbarData;
    }

    public void SetHotbarItem(List<InventorySaveData> inventorysaveData)
    {
        foreach (Transform child in hotbarPannel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < slotcount; i++)
        {
            Instantiate(slotPrefab, hotbarPannel.transform);
        }
        foreach (InventorySaveData data in inventorysaveData)
        {
            Slot slot = hotbarPannel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
            GameObject ItemPrefab = itemDictionary.GetItemPrefab(data.itemId);
            if (ItemPrefab != null)
            {
                GameObject item = Instantiate(ItemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }
}
