using System;
using System.Collections.Generic;
using UnityEngine;
public class Evidence : MonoBehaviour
{
    public enum EvidenceType{
        Knife,
        Pocket_Watch,
        Diary,
        Note,
    }
    [Header("Evidence Info")]
    [TextArea] public string description;
    [TextArea] public string info;
    public string evidenceName;
    public EvidenceType type;
    public static List<int> NonDeathTimes;
    void Awake() {
        if (NonDeathTimes == null){
            NonDeathTimes = new List<int>(GameStats.INSTANCE.Times);
            NonDeathTimes.Remove(GameStats.INSTANCE.TimeOfDeath+GameStats.INSTANCE.ScheduleOffset);
            Debug.Log(string.Join(", ",NonDeathTimes));
        }
    }
    void Start() {

        switch(type) {
            case EvidenceType.Knife:
                GenerateSuspectsInfo();
                break;
            default:
                GenerateTimeBasedInfo();
                break;
        }
    }

    void GenerateSuspectsInfo(){
        List<NPC> suspects = new List<NPC>();
        var murderer = GameStats.INSTANCE.Murderer;
        suspects.Add(GameStats.INSTANCE.Murderer);
        
        var keys = new List<NPC>(GameStats.INSTANCE.Victim.NPCIdentity.Relations.Keys);
        NPC randomRelation;
        do {
            randomRelation = keys[UnityEngine.Random.Range(0, keys.Count)];
        } while (murderer == randomRelation);
        suspects.Add(randomRelation);
        
        NPC randomNPC = GameStats.INSTANCE.CivillianNPCs[UnityEngine.Random.Range(0, GameStats.INSTANCE.CivillianNPCs.Count)];
        do {
            randomNPC = GameStats.INSTANCE.CivillianNPCs[UnityEngine.Random.Range(0, GameStats.INSTANCE.CivillianNPCs.Count)];
        } while (suspects.Contains(randomNPC));
        suspects.Add(randomNPC);
        // int ndex = UnityEngine.Random.Range(0, GameStats.INSTANCE.CivillianNPCs.Count);
        // int secondIndex;
        // do {
        //     secondIndex = UnityEngine.Random.Range(0, GameStats.INSTANCE.CivillianNPCs.Count);
        // } while (secondIndex == firstIndex);
        // suspects.Add(GameStats.INSTANCE.CivillianNPCs[firstIndex]);
        // suspects.Add(GameStats.INSTANCE.CivillianNPCs[secondIndex]);
        // suspects.Add(GameStats.INSTANCE.CivillianNPCs[secondIndex]);
        suspects = ShuffleList(suspects);
        foreach (NPC npc in suspects) {
            info += "- " + npc.NPCIdentity.Name + "\n";
        }
    }
    void GenerateTimeBasedInfo(){
        info += " " + NonDeathTimes[0] + " and " + (NonDeathTimes[0]+1);
        NonDeathTimes.RemoveAt(0);
        // if (NonDeathTimes.Count <= 2){
        //     info += " " + NonDeathTimes[0] + " and " + (NonDeathTimes[0]+1)
        //         + " and not between " + NonDeathTimes[1] + " and " + (NonDeathTimes[1]+1);
        // } else {
        // }
    }
    void GenerateDiaryInfo(){
        foreach (var pair in GameStats.INSTANCE.Victim.NPCIdentity.Relations) {
            if (pair.Value != Identity.RelationTypes.None){
                info += "- " + pair.Key.NPCIdentity.Name + "\n";
            }
        }
    }
    public void OnClickEvidence()
    {
    ShowInfo();
    }   
    public void ShowInfo()
    {
        EvidenceDisplayManager.Instance.DisplayEvidence(this);
    }
    public List<NPC> ShuffleList(List<NPC> list) {
		var count = list.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}
        return list;
	}
}

