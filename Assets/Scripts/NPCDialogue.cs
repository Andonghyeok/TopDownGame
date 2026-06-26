using System;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public bool[] autoProgressLines; // 넘어가기 버튼을 누르지 않아도 자동으로 넘어가기 위해
    
    public float autoProgressDelay = 1.5f; // 자동으로 넘어가기 전 대기 시간
    public string[] dialogueLines;
    public bool[] endDialogueLines;
    public float typingSpeed = 0.05f;
    // 오디오 클립 추가 나중에 public AudioClip voiceSound; // NPC 음성 효과를 위한 오디오 클립
    // public float voicePitch = 1.0f; // 음성 효과의 피치 조절

    public DialogueChoice[] choices;
}
[Serializable]
public class DialogueChoice
{
    public int dialogueIndex;  // 선택지 Text line index
    public string[] choices;    // 선택지 Text
    public int[] nextDialogueIndexes;  // 응답 선택후 다음에 대화의 줄을 저장하는 인덱스
}
