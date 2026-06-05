using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int Id;
    public string Name;

    public virtual void useItem()
    {
        Debug.Log("Using item" + Name);
    }
    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}
