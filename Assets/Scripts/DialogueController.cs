using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; } // 싱글톤 인스턴스
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }
    public void SetNPCInfo(string npcName , Sprite portrait)
    {
        nameText.text = npcName;
        portraitImage.sprite = portrait;
    }
    public void SetDialgueText(string text)
    {
        dialogueText.text = text;
    }
    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);
    }
    public void  CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onclick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onclick);
    }
}
