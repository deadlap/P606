using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates the initial (unseen by player) prompt for the LLMCharacter to use
///  Maybe change this to create a txt file so we can use RAG.
/// </summary>

public class NPCInitialPromptGenerator : MonoBehaviour {

    [SerializeField] NPC Npc;
    [SerializeField] Identity NPCIdentity;
    
    string InitialPrompt;

    [TextAreaAttribute(20, 30)]
    public string GeneratedPrompt;
    
    string GetRelationText(){
        string text = "";
        string startText = "You know: ";
        string endText = ", your relationship to them is: ";
        foreach(KeyValuePair<NPC,Identity.RelationTypes> relation in NPCIdentity.Relations) {
            if (relation.Value != Identity.RelationTypes.None) {
                text += startText + GetStringName(relation.Key.NPCIdentity.Name) + endText + relation.Value + ". ";
            }
        }
        return text;
    }
    public string GeneratePrompt(){
        Npc = GetComponent<NPC>();
        NPCIdentity = GetComponent<Identity>();
        InitialPrompt = "" + 
            "Your Name is: " + GetStringName(NPCIdentity.Name) + ". Your Occupation is: " + NPCIdentity.Occupation + 
            ". Your personality trait is: " + NPCIdentity.Trait + ". Your role in this game is: " + NPCIdentity.PrimaryRole + ". ";
        GeneratedPrompt = InitialPrompt + GetRelationText();
        return GeneratedPrompt;
    }
    public string GetStringName(Identity.Names name){
        return System.Enum.GetName(typeof(Identity.Names),name);
    }
}
