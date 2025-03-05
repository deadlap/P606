using UnityEngine;
using System.Collections.Generic;

public class NPCInitialPromptGenerator : MonoBehaviour {

    [SerializeField] NPC Npc;
    [SerializeField] Identity NPCIdentity;
    
    string InitialPrompt;

    [TextAreaAttribute(20, 30)]
    [SerializeField] string GeneratedPrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Npc = GetComponent<NPC>();
        NPCIdentity = GetComponent<Identity>();
        InitialPrompt = "something something game setting text and rules for the ai. " + 
            "Your Name is: " + NPCIdentity.Name + ". Your Occupation is: " + NPCIdentity.Occupation + 
            ". Your personality trait is: " + NPCIdentity.Trait + ". Your role in this game is: " + NPCIdentity.PrimaryRole + ". ";
        GeneratedPrompt = InitialPrompt + GetRelationText();
    }

    // Update is called once per frame
    void Update() {
        
    }

    string GetRelationText(){
        string text = "";
        string startText = "You know: ";
        string endText = ", your relationship to them is: ";
        foreach(KeyValuePair<NPC,Identity.RelationTypes> relation in NPCIdentity.Relations) {
            if (relation.Value != Identity.RelationTypes.None) {
                text += startText + relation.Key.NPCIdentity.Name + endText + relation.Value + ". ";
            }
        }
        return text;
    }
}
