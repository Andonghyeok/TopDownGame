using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int Id;
    public string Name;
    public int quantity = 1;

    private TMP_Text quantityText;

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityDisplay();
    }
    public void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }
    public void AddToStack(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityDisplay();
    }
    public int RemoveFromStack(int amount = 1)
    {
        int removedAmount = Mathf.Min(amount, quantity);
        quantity -= removedAmount;
        UpdateQuantityDisplay();
        return removedAmount;
    }
    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        Item clonItem = clone.GetComponent<Item>();
        clonItem.quantity = newQuantity;
        clonItem.UpdateQuantityDisplay();
        return clone;
    }
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
