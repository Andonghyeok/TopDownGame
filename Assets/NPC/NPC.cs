using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteracterable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex = 0;
    private bool isTyping, isDialogueActive;

    public bool CanInteract()
    {
        return !isDialogueActive; // 대화 중이 아닐 때 상호작용 가능
    }

    public void Inteact()
    {
        if (dialoguePanel == null || (PauseController.IsGamePaused && isDialogueActive))
        {
            return; // 대화 패널이 없거나 게임이 일시정지 상태에서 대화 중이면 상호작용 불가
        }
        if(isDialogueActive)
        {
            if (!isTyping)
            {
                NextLine();
            }
        }
        else
        {
           StartDialogue();
        }
    }
    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());

    }
    void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if(++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }
    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }
    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }

}
