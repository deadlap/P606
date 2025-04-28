using System.Threading.Tasks;
using LLMUnity;
using UnityEngine;

public class RAGData : MonoBehaviour
{
    [SerializeField] RAG rag;
    [SerializeField] public TextAsset ragText;

    public async void LoadRAG()
    {
        if(ragText == null) return;
        print("rag loaded");
        await rag.Add(ragText.text);
    }

    public async Task<string> CheckRAG(string message, int k)
    {
        (string[] similarPhrases, float[] distances) = await rag.Search(message, k);
        var prompt = "Answer the user query based on the provided data. \n\n";
        prompt += $"User query: {message}\n\n";
        prompt += "Data:\n";
        foreach (string similarPhrase in similarPhrases) prompt += $"\n- {similarPhrase}";
        return await Task.FromResult(prompt);
    }
}
