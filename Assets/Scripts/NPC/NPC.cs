using UnityEngine;
using LLMUnity;


public class NPC : MonoBehaviour {
    public Identity NPCIdentity;
    public int Index;
    public LLMCharacter llmCharacter;
    public NPCInitialPromptGenerator PromptGenerator;

    public void SetIdentity(Identity ID) {
        NPCIdentity = ID;
    }

    public void StartUp(){
        llmCharacter = GetComponent<LLMCharacter>();
        PromptGenerator = GetComponent<NPCInitialPromptGenerator>();
        llmCharacter.SetPrompt(PromptGenerator.GeneratePrompt(), true);
        llmCharacter.AIName = NPCIdentity.name.ToString();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
