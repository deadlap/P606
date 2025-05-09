using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStats : MonoBehaviour {
    public static GameStats INSTANCE;
    //game time limit in seconds
    public float TimeLimit;
    public float EventTriggerTime;
    //When the murder took place the previous day
    public int TimeOfDeath;
    public NPC Victim;
    public NPC Murderer;
    public List<NPC> CivillianNPCs;
    public List<int> Times;
    public int ScheduleLength;
    public int ScheduleOffset; //Given the lists are from 0..schedulelength, in text and such needs to be offset by a value to give the actual times it represents
    public List<int> WorkingHours;
    public bool IntroPlayed;
    public bool CheckedNoteBook;
    public static event Action SetIntroPlayed;
    public static void OnSetIntroPlayed() => SetIntroPlayed?.Invoke();
    
    public static List<int> usedFaces = new List<int>();

    public int EvidenceToGather;

    // Data to be gathered for the log system
    public int EvidenceGathered;
    void OnEnable() {
        SetIntroPlayed += GameStarted;
        EvidenceDisplayManager.ShowEvidenceEvent += EvidenceFound;
    }
    void OnDisable() {
        SetIntroPlayed -= GameStarted;
        EvidenceDisplayManager.ShowEvidenceEvent -= EvidenceFound;
    }

    void GameStarted(){
        IntroPlayed = true;
    }
    void EvidenceFound(Evidence.EvidenceType _){
        EvidenceGathered++;
    }

    void Awake() {
        if(INSTANCE == null)
            INSTANCE = this;
        else 
            Destroy(gameObject);
        EvidenceGathered = 0;
        EvidenceToGather = 4;
        IntroPlayed = false;
        CivillianNPCs = new List<NPC>();
        usedFaces = new List<int>();
    }
}
