using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

[CreateAssetMenu(menuName= "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public List<QuestObjectives> objectives;

    private void Onvalidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName = Guid.NewGuid().ToString();
        }
    }

    [Serializable]
    public class QuestObjectives
    {
        public string objextiveID; // 처치해야할 ID와 수집해야할 아이템 ID와 Match 시켜줄 ID
        public string description;
        public objectiveType type; 
        public int requiredAmount; // quest를 깨기 위한 용량
        public int currentAmount;  // quest 진행상태를 알려주는 용량

        public bool IsCompleted => currentAmount >= requiredAmount;
    }
    public enum objectiveType
    {
        CollectItem,
        Defeated,
        ReachLocation,
        TalkNPC,
        custom
    }
    [Serializable]
    public class QuestProgress
    {
        public Quest quest;
        public List<QuestObjectives> Objectives;

        public QuestProgress(Quest quest)
        {
            this.quest = quest;
            Objectives = new List<QuestObjectives>();

            // DeepCopy를 사용 (현재 용량은 0으로 초기화)
            foreach(var obj in quest.objectives)
            {
                Objectives.Add(new QuestObjectives
                {
                    objextiveID = obj.objextiveID,
                    description = obj.description,
                    type = obj.type,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }
        public bool IsCompleted => Objectives.TrueForAll(o => o.IsCompleted); // TrueForAll(조건) => List의 모든 요소가 조건에 만족하여야함
        public string questID => quest.questID;

    }
}
