using UnityEngine;
using LLMUnity;


public class NPC : MonoBehaviour {
    public Identity NPCIdentity;
    public LLMCharacter llmCharacter;
    public NPCInitialPromptGenerator PromptGenerator;

    public void SetIdentity(Identity ID) {
        NPCIdentity = ID;
    }
    public void StartUp(){
        llmCharacter = GetComponentInChildren<LLMCharacter>();
        PromptGenerator = GetComponent<NPCInitialPromptGenerator>();
        llmCharacter.AIName =  System.Enum.GetName(typeof(Identity.Names), NPCIdentity.Name);
        var prompt = PromptGenerator.GeneratePrompt();
        //Set the initial prompt for the AI chatbot, with its personality etc.
        llmCharacter.SetPrompt(prompt, true);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
