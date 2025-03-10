using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

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
        return (string.Format(Prompt, 
            GameObject.FindGameObjectWithTag("NPCGenerator").GetComponent<NPCGenerator>().GetVictim().NPCIdentity.Name,
            GetStringName(NPCIdentity.Name),
            NPCIdentity.Occupation,
            NPCIdentity.PrimaryRole,
            GetRelationText(),
            NPCIdentity.Openness,
            NPCIdentity.Conscientiousness,
            NPCIdentity.Extraversion,
            NPCIdentity.Agreeableness,
            NPCIdentity.Neuroticism
        ));
    }
    public string GetStringName(Identity.Names name){
        return System.Enum.GetName(typeof(Identity.Names),name);
    }
}
