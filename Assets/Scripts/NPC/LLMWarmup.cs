using UnityEngine;
using UnityEngine.UI;   

public class LLMWarmup : MonoBehaviour
{
    [SerializeField] Image warmupIndicator;
    [SerializeField] [Tooltip("Warms up all active LLM Characters, eliminating the initial wait time when starting a conversation." +
        "\n\nWARNING: This may result in pseudo-crashes if runtime is terminated before all LLM Characters are warmed up. " +
        "\nThis may take a long time, up to 10 minutes atleast.")] 
        bool warmUpOnStart;
    int warmupCount;

    private void Awake()
    {
        Invoke(nameof(WarmUp), 3f); // Should find a better way to do this, but this works for now
        warmupCount = 0;
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
        UpdateWarmupCount();
        Debug.Log($"LLMCharacter warmed up.");
    }

    void UpdateWarmupCount()
    {
        warmupCount++;
        if (warmupCount < (NPCGenerator.INSTANCE.NPCs.Count / 2))
        {
            Debug.Log("Less than half of LLM Characters warmed up.");
            if(warmupIndicator == null) return;
            warmupIndicator.color = Color.red;
        }
        if (warmupCount >= (NPCGenerator.INSTANCE.NPCs.Count / 2))
        {
            Debug.Log("Half of LLM Characters warmed up.");
            if (warmupIndicator == null) return;
            warmupIndicator.color = Color.yellow;
        }
        if (warmupCount >= NPCGenerator.INSTANCE.NPCs.Count)
        { 
            Debug.Log("All LLM Characters warmed up.");
            if(warmupIndicator == null) return;
            warmupIndicator.color = Color.green;
        }
    }
}
