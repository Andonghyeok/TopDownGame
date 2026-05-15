using UnityEngine;
using UnityEngine.UI;


public class TapController : MonoBehaviour
{
    public Image[] tapImgaes;
    public GameObject[]  pages;
    void Start()
    {
        ActivateTap(0);
    }

    public void ActivateTap(int tapNo)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tapImgaes[i].color = Color.gray;
    
        }
        pages[tapNo].SetActive(true);
        tapImgaes[tapNo].color= Color.white;    

    }
}
