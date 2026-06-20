using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteracterable iteracterableInRange = null;
    public GameObject interactrionIcon;


    void Start()
    {
        interactrionIcon.SetActive(false);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            iteracterableInRange?.Inteact();
            if (!iteracterableInRange.CanInteract())
            {
                interactrionIcon.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteracterable interacterable) && interacterable.CanInteract())
        {
            iteracterableInRange = interacterable;
            interactrionIcon.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteracterable interacterable) && interacterable == iteracterableInRange)
        {
            iteracterableInRange = null;
            interactrionIcon.SetActive(false);
        }
    }
}
