using UnityEngine;

public class LLMWarmup : MonoBehaviour
{
    [SerializeField] [Tooltip("Warms up all active LLM Characters, eliminating the initial wait time when starting a conversation." +
        "\n\nWARNING: This may result in pseudo-crashes if runtime is terminated before all LLM Characters are warmed up. " +
        "\nThis may take a long time, up to 10 minutes atleast.")] 
        bool warmUpOnStart;

    private void Awake()
    {
        Invoke(nameof(WarmUp), 5f);
    }

    /// <summary>
    /// Warms up the LLM character and load the RAG data. This must only be called once.
    /// </summary>
    void WarmUp()
    {
        for (int i = 0; i < NPCGenerator.INSTANCE.NPCs.Count; i++)
        {
            var npc = NPCGenerator.INSTANCE.NPCs[i];
            Debug.Log($"Warming up {npc.name}'s LLM character and loading RAG data...");
            if (npc == null) return;
            if (npc.llmCharacter == null) return;
            if (warmUpOnStart)
                _ = npc.llmCharacter.Warmup(WarmedUp);
            if (npc.GetComponentInChildren<RAGData>() == null) return;
            npc.GetComponentInChildren<RAGData>().LoadRAG();
            Debug.Log($"{npc.name}'s RAG has loaded");
        }
    }

    void WarmedUp()
    {
        Debug.Log($"LLMCharacter warmed up.");
    }
}
