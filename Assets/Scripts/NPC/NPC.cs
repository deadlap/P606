using UnityEngine;
using LLMUnity;
using UnityEngine.AI;
using Unity.VisualScripting;
using RoomFocusing;
using System.Collections.Generic;
using System;


public class NPC : MonoBehaviour {
    public Identity NPCIdentity;
    public LLMCharacter llmCharacter;
    public NPCInitialPromptGenerator PromptGenerator;
    public NPCSpawnPoint SpawnPoint;
    [SerializeField] GameObject Body;
    [SerializeField] GameObject Head;
    public void SetIdentity(Identity ID) {
        NPCIdentity = ID;
    }
    void Start() {
        Body = transform.Find("CharacterNormal/Body").gameObject;
        Head = transform.Find("CharacterNormal/Head").gameObject;

        llmCharacter = GetComponentInChildren<LLMCharacter>();
        PromptGenerator = GetComponent<NPCInitialPromptGenerator>();
        llmCharacter.AIName =  System.Enum.GetName(typeof(Identity.Names), NPCIdentity.Name);
        var prompt = PromptGenerator.GeneratePrompt();
        
        //Set the initial prompt for the AI chatbot, with its personality etc.
        llmCharacter.SetPrompt(prompt, true);
        SpawnPoint = NPCSpawnPointFinder.Instance.FindSpawnPoint(NPCIdentity.Occupation);
        transform.position = SpawnPoint.SpawnPosition();
        GetComponent<NavMeshAgent>().enabled = true;
        
        var ragText = Resources.Load<TextAsset>("RAGs/"+NPCIdentity.Occupation.ToString());
        llmCharacter.gameObject.GetComponentInChildren<RAGData>().ragText = ragText;
        llmCharacter.gameObject.GetComponentInChildren<RAGData>().LoadRAG();

        var bodymat = Resources.Load<Material>("Outfits/"+NPCIdentity.Occupation.ToString()) as Material;

        // if (bodymat != null)
        //    Body.GetComponent<Renderer>().materials[0] = bodymat;
        // var accessory = Resources.Load<GameObject>("Accessories/"+NPCIdentity.Occupation.ToString()) as GameObject;
        // if (accessory != null)
        //     Instantiate(accessory, transform);
        // var headMatArr = Head.GetComponent<Renderer>().materials;
        // gameObject.GetComponent<ShadowPerson>().GetLooks(Head.GetComponent<Renderer>().materials,Body.GetComponent<Renderer>().materials);
    }
}