using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour {
    public static GameStats INSTANCE;

    public int TimeOfDeath;
    public NPC Victim;
    public NPC Murderer;

    public int ScheduleLength;
    public int ScheduleOffset; //Given the lists are from 0..schedulelength, in text and such needs to be offset by a value to give the actual times it represents
    public List<int> WorkingHours;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        INSTANCE = this;
    }
}
