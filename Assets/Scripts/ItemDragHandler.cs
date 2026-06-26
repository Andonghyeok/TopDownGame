using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    private InventoryController inventoryController;

    // 드래그를 시작할때
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) return;

        originalParent = transform.parent;  // 드래그를 시작할때 원래 슬롯의 위치를 기억해둔다.
        transform.SetParent(transform.root); // UI를 최상단의 Layer를 최상단으로 하여 UI에 가려지지 않고 보이게 한다.
        canvasGroup.blocksRaycasts = false;  // Raycasts를 잠시 꺼두어 아이템의 마우스 충돌을 막아둔다.
        canvasGroup.alpha = 0.6f;           // 아이템의 투명도를 낮추어 이 아이템이 drag중이라고 알려준다.

    }

    // 드래그 중일때
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) return;

        transform.position = eventData.position;    
    }

    // 드래그를 끝낼때
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) return;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        // pointerEnter는 지금 마우스가 올라가 있는 게임오브젝트
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();

        // 드래그가 끝난 상태에서 dropSlot이 없을 경우
        if(dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if(dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();
        // 드래그가 끝난 상태에서 dropSlot이 있을 경우
        if (dropSlot != null)
        {
            // dropSlot에 이미 아이템이 있을 경우 
            if (dropSlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetIteam = dropSlot.currentItem.GetComponent<Item>();  

                if(draggedItem.Id == targetIteam.Id)
                {
                    // 같은 아이템이면 합치기
                    targetIteam.AddToStack(draggedItem.quantity);
                    originalSlot.currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }

 
            }
            // dropSlot에 아이템이 없을 경우
            else
            {
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
        // 드래그가 끝난 상태에서 dropSlot이 없을 경우
        else
        {
            // 드래그가 끝난 상태에서 inventory 영역에 없을 경우 아이템을 drop한다.
            if (!IswithInventrory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                // 다시 원래 슬롯으로 돌아가는 연산
                transform.SetParent(originalParent);
            }
                
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;  
    }

   
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryController = InventoryController.Instance;

    }
    bool IswithInventrory(Vector2 mousePosition)
    {
        RectTransform inventoryRect =originalParent.parent.GetComponent<RectTransform>();
        // 인벤토리 패널에 마우스 포인터 위치가 포함되는지 확인한다.(true or false)
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition); 
 
    }
    void DropItem(Slot originalSlot)
    {
        Item item = GetComponent<Item>();
        int quantity = item.quantity;
        if (quantity > 0)
        {
            item.RemoveFromStack();
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            quantity = 1;
        }
        else
        {
            originalSlot.currentItem = null;    
        }

            originalSlot.currentItem = null;

        // Player 위치 찾기
        Transform playerTransfom = GameObject.FindGameObjectWithTag("Player")?.transform;
        if(playerTransfom == null)
        {
            Debug.Log("Missing 'Player' tag");
            return;
        }

        // Player 주변에 랜덤으로 아이템 드랍하기(원형 범위)
        //Random.insideUnitCircle 반지름 1이내의 원안의 임의에 점을 반환
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransfom.position + dropOffset;

        // drop Item을 인스턴스화 하기
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        Item droppedItem = dropItem.GetComponent<Item>();   
        droppedItem.quantity = 1;


        dropItem.GetComponent<BounceEffect>().StartBounce();
        // UI에 있는 drop Item 삭제
        if(quantity <= 1 && originalSlot.currentItem == null)
        {
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Right)
        {
            SplitStack();
        }
    }
    private void SplitStack()
    {
        Item item = GetComponent<Item>();
        if (item == null || item.quantity <= 1) return;

        int splitAmount = item.quantity / 2;
        if(splitAmount <= 0) return;

        item.RemoveFromStack(splitAmount);
        
        GameObject newItem = item.CloneItem(splitAmount);
        if(inventoryController == null || newItem == null) return;

        foreach(Transform slotTransform in inventoryController.inventoryPannel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem == null)
            {
;               slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }
        item.AddToStack(splitAmount);
        Destroy(newItem);
    }
}
    




