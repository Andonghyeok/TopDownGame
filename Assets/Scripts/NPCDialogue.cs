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
    public float typingSpeed = 0.05f;
    // 오디오 클립 추가 나중에 public AudioClip voiceSound; // NPC 음성 효과를 위한 오디오 클립
    // public float voicePitch = 1.0f; // 음성 효과의 피치 조절

}
