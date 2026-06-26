using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteracterable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;

    private int dialogueIndex = 0;
    private bool isTyping, isDialogueActive;

    private void Start()
    {
        dialogueUI = DialogueController.instance;
    }
    public bool CanInteract()
    {
        return !isDialogueActive; // 대화 중이 아닐 때 상호작용 가능
    }

    public void Inteact()
    {
        if (dialogueData == null || (PauseController.IsGamePaused && isDialogueActive))
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

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);
        PauseController.SetPause(true);

        DisplayCurrentLine();

    }
    void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialgueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        // Clear choices
        dialogueUI.ClearChoices();
        // Check endDiallogueLines
        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }
        // Check in Choices & display
        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if(dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }
        if(++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }
    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialgueText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialgueText(dialogueUI.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }
    void DisplayChoices(DialogueChoice choice)
    {
        for(int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogueUI.CreateChoiceButton(choice.choices[i] ,() => ChooseOption(nextIndex));

        }
    }
    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }
    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }
    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialgueText("");
        dialogueUI.ShowDialogueUI(false);
        PauseController.SetPause(false);
    }

}
