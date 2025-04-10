using UnityEngine;
using LLMUnity;
using UnityEngine.AI;
using Unity.VisualScripting;


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
        
        var ragText = Resources.Load<TextAsset>("Rags/"+NPCIdentity.Occupation.ToString());
        llmCharacter.gameObject.GetComponentInChildren<RAGData>().ragText = ragText;
        llmCharacter.gameObject.GetComponentInChildren<RAGData>().LoadRAG();

        // Body.GetComponent<Renderer>().material = Resources.Load<Material>("Outfits/"+NPCIdentity.Occupation.ToString());;
        // Instantiate(Resources.Load<GameObject>("Prefabs/Accessories/"+NPCIdentity.Occupation.ToString()), transform);

    }

    // Update is called once per frame
    void Update() {
        
    }
}
