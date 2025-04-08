using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

/// <summary>
/// Generates the initial (unseen by player) prompt for the LLMCharacter to use
///  Maybe change this to create a txt file so we can use RAG.
/// </summary>

public class NPCInitialPromptGenerator : MonoBehaviour {

    [SerializeField] NPC Npc;
    [SerializeField] Identity NPCIdentity;

    [TextAreaAttribute(10, 15)]
    public string GeneratedPrompt;
    [TextAreaAttribute(10, 15)]
    [SerializeField] string Prompt;
    public Dictionary<Identity.Locations,string> LocationPropositions;
    public Dictionary<Identity.Occupations,string> OccupationDeterminer;

    void Awake() {
        OccupationDeterminer = new Dictionary<Identity.Occupations,string>();
        LocationPropositions = new Dictionary<Identity.Locations, string>
        {
            { Identity.Locations.Kitchen, "in the " },
            { Identity.Locations.Ballroom, "in the " },
            { Identity.Locations.Hallway, "in the " },
            { Identity.Locations.Bar, "at the " },
            { Identity.Locations.Pool, "at the " },
            { Identity.Locations.Cabin, "in their own " },
            { Identity.Locations.None, "blank" }
        };
        OccupationDeterminer.Add(Identity.Occupations.Chef, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Janitor, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Waiter, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Bartender, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Painter, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Doctor, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Lawyer, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Magician, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Oil_Tycoon, "an ");
        OccupationDeterminer.Add(Identity.Occupations.Admiral, "an ");
        OccupationDeterminer.Add(Identity.Occupations.Priest, "a ");
        OccupationDeterminer.Add(Identity.Occupations.Saxophonist, "a ");
    }
    string GetRelationText(){
        string text = "";
        foreach(KeyValuePair<NPC,Identity.RelationTypes> relation in NPCIdentity.Relations) {
            if (relation.Value != Identity.RelationTypes.None) {
                text += NPCIdentity.name + " knows " + GetStringName(relation.Key.NPCIdentity.Name) +
                    ", who is " + OccupationDeterminer[relation.Key.NPCIdentity.Occupation] + relation.Key.NPCIdentity.Occupation.ToString().Replace("_", " ") +
                    ", the two of them are " + relation.Value.ToString().Replace("_", " ") + ". \n";
            }
        }
        return text;
    }
    

    

    string GenerateScheduleText(){
        string text = "";
        for (int i = 0; i < NPCIdentity.Schedule.Count; i++) {
            text += "The detective will question " + NPCIdentity.name + " about where " + NPCIdentity.name + " was, and " + 
                NPCIdentity.name + " will answer that " + NPCIdentity.name + " was " + LocationPropositions[NPCIdentity.Schedule[i]] + NPCIdentity.Schedule[i] +
                " between " + (i+GameStats.INSTANCE.ScheduleOffset) + " and " + (i+GameStats.INSTANCE.ScheduleOffset+1);
            if (NPCIdentity.SchedulePairings[i].Count > 0){ 
                text += ", and " + NPCIdentity.name + " saw ";
                for (int j = 0; j < NPCIdentity.SchedulePairings[i].Count; j++) {
                    text += NPCIdentity.SchedulePairings[i][j].NPCIdentity.name;
                    if (j == NPCIdentity.SchedulePairings[i].Count-1) {
                        text += ". ";
                    } else if (j == NPCIdentity.SchedulePairings[i].Count-2) {
                        text += ", and ";
                    } else {
                        text += ", ";
                    }
                }
            }
            text += "\n";
        }
        return text;
    }

    public string GeneratePrompt(){
        Npc = GetComponent<NPC>();
        NPCIdentity = GetComponent<Identity>();
        GeneratedPrompt = string.Format(Prompt, 
            GetStringName(NPCIdentity.Name), //Name
            OccupationDeterminer[NPCIdentity.Occupation] + NPCIdentity.Occupation.ToString().Replace("_", " "), //Occupation
            GetRelationText(), //RelationText
            GameObject.FindGameObjectWithTag("NPCGenerator").GetComponent<NPCGenerator>().GetVictim().NPCIdentity.Name, //Victim
            GenerateScheduleText() //schedule
        );
        return GeneratedPrompt;
    }
    public string GetStringName(Identity.Names name){
        return System.Enum.GetName(typeof(Identity.Names),name);
    }
}