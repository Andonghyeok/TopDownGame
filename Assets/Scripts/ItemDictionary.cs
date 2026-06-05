using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].Id = i + 1;
            }
        }
        foreach(Item item in itemPrefabs)
        {
            itemDictionary[item.Id] = item.gameObject;
        }


    }
    public GameObject GetItemPrefab(int ItemId)
    {
        if(itemDictionary.TryGetValue(ItemId, out GameObject prefab) == false)
        {
            Debug.LogWarning($"Item with Id {ItemId} item not found in dictionary");
        }
        return prefab;
    }
}
