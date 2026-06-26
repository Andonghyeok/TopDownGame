using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject meneCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       meneCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!meneCanvas.activeSelf && PauseController.IsGamePaused)
            {
                return;
            }
            meneCanvas.SetActive(!meneCanvas.activeSelf);
            PauseController.SetPause(meneCanvas.activeSelf);

        }
    }
}
