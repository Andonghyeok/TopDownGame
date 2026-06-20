using UnityEngine;

public class Chest : MonoBehaviour, IInteracterable
{
    public bool isOpened {  get; private set; }
    public string chestID { get; private set; }
    public GameObject itemPrefab;   // 상자에서 나오는 아이템 프리펩
    public Sprite openedSprite;     // 오픈된 상자 스프라이트

    void Start()
    {
        chestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        
    }
    public bool CanInteract()
    {
        return !isOpened;
    }

    public void Inteact()
    {
        if(!CanInteract()) return;
        openChest();
    }
    private void openChest()
    {
        SetOpened(true);
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down,Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }
    public void SetOpened(bool opened)
    {
    
        if (isOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite= openedSprite;

        }
    }
}
