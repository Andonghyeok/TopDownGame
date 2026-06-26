using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPannel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefab;

    public static InventoryController Instance { get; private set; }

    public void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();

    }
    public bool AddItem(GameObject itemPrefab) 
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        // Check if we have this item type in inventory
        foreach (Transform slotTransform in inventoryPannel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if(slotItem != null && slotItem.Id == itemToAdd.Id)
                {
                    slotItem.AddToStack();
                    return true;
                }
            }
        }

        // Look for empty slot
        foreach (Transform slotTransform in inventoryPannel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem= Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full");
        return false;
    }
    public List<InventorySaveData> GetInventoryItem()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransfom in inventoryPannel.transform)
        {
            Slot slot = slotTransfom.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData
                { 
                    itemId = item.Id,
                    slotIndex = slotTransfom.GetSiblingIndex() ,
                    quantity = item.quantity
                });
            }
        }
        return invData;
    }
    
    public void SetInventoryItem(List<InventorySaveData> inventorysaveData)
    {
        foreach(Transform child in inventoryPannel.transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPannel.transform);
        }
        foreach (InventorySaveData data in inventorysaveData)
        {
            Slot slot = inventoryPannel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
            GameObject ItemPrefab = itemDictionary.GetItemPrefab(data.itemId);
            if (ItemPrefab != null)
            {
                GameObject item = Instantiate(ItemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Item itemComponent = item.GetComponent<Item>();
                if(itemComponent != null && data.quantity > 1)
                {
                    itemComponent.quantity = data.quantity;
                    itemComponent.UpdateQuantityDisplay();
                }
                slot.currentItem = item;
            }
        }
    }
}
