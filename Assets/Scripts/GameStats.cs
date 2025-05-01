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
    public static event Action SetIntroPlayed;
    public static void OnSetIntroPlayed() => SetIntroPlayed?.Invoke();
    void OnEnable() {
        SetIntroPlayed += GameStarted;
    }
    void OnDisable() {
        SetIntroPlayed -= GameStarted;
    }

    void GameStarted(){
        IntroPlayed = true;
    }

    void Awake() {
        INSTANCE = this;
        IntroPlayed = false;
        CivillianNPCs = new List<NPC>();
    }
}
