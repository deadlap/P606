using LLMUnity;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   
using Debug = UnityEngine.Debug;
public class LLMWarmup : MonoBehaviour
{
    TextMeshProUGUI warmupText;
    TextMeshProUGUI warmupPercentage;
    [SerializeField] TMP_FontAsset font;
    [SerializeField] float fontSize = 24f;
    [SerializeField] Vector3 textPosition = new(120, 0, 0);
    [SerializeField] Vector3 percentagePosition = new(470, 0, 0);
    [SerializeField] Image warmupIndicator;
    [SerializeField] [Tooltip("Warms up all active LLM Characters, eliminating the initial wait time when starting a conversation." +
        "\n\nWARNING: This may result in pseudo-crashes if runtime is terminated before all LLM Characters are warmed up. " +
        "\nThis may take a long time, up to 10 minutes atleast.")] 
        bool warmUpOnStart;
    int warmupCount;
    Stopwatch stopwatch = new Stopwatch();
    float time;
    bool isUpdating;

    void Awake()
    {
        isUpdating = true;
        Invoke(nameof(WarmUp), 3f); // Should find a better way to do this, but this works for now
        warmupCount = 0;
        stopwatch.Start();
    }

    void OnEnable()
    {
        PlayerInputEvent.QuitToMainMenu += CancelWarmUp;
    }

    void OnDisable()
    {
        PlayerInputEvent.QuitToMainMenu -= CancelWarmUp;
    }

    void Start()
    {
        warmupText = CreateText(warmupIndicator.transform, textPosition, "Warming up LLM Characters");
        warmupPercentage = CreateText(warmupIndicator.transform, percentagePosition, "0%");
        StartCoroutine(TextAnimation());
    }

    TextMeshProUGUI CreateText(Transform parent, Vector3 position, string text)
    {
        var textMesh = new GameObject().AddComponent<TextMeshProUGUI>();
        textMesh.font = font;
        textMesh.fontSize = fontSize;
        textMesh.alignment = TextAlignmentOptions.MidlineLeft;
        textMesh.textWrappingMode = TextWrappingModes.NoWrap;
        textMesh.transform.parent = parent;
        textMesh.transform.localPosition = position;
        textMesh.text = text;
        return textMesh;
    }

    private void Update()
    {
        if (warmupText && warmupPercentage)
        {
            warmupText.transform.localPosition = textPosition;
            warmupPercentage.transform.localPosition = percentagePosition;
        }
    }

    /// <summary>
    /// Warms up the LLM character and load the RAG data. This must only be called once.
    /// </summary>
    void WarmUp()
    {
        Debug.Log("LLM Warmup started.");
        for (int i = 0; i < NPCGenerator.INSTANCE.NPCs.Count; i++)
        {
            var npc = NPCGenerator.INSTANCE.NPCs[i];
            //Debug.Log($"Warming up {npc.name}'s LLM character and loading RAG data...");
            if (npc == null) return;
            if (npc.llmCharacter == null) return;
            if (warmUpOnStart)
            {
                _ = npc.llmCharacter.Warmup(WarmedUp);
            }
            if (npc.GetComponentInChildren<RAGData>() == null) return;
            npc.GetComponentInChildren<RAGData>().LoadRAG();
            //Debug.Log($"{npc.name}'s RAG has loaded");
        }
    }
    

    IEnumerator TextAnimation()
    {
        if (warmupText != null)
        {
            while (isUpdating)
            {
                yield return new WaitForSeconds(0.5f);
                warmupText.text = "Warming up LLM Characters";
                yield return new WaitForSeconds(0.5f);
                warmupText.text = "Warming up LLM Characters.";
                yield return new WaitForSeconds(0.5f);
                warmupText.text = "Warming up LLM Characters..";
                yield return new WaitForSeconds(0.5f);
                warmupText.text = "Warming up LLM Characters...";
            }
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
        if (warmupText.gameObject && warmupPercentage.gameObject)
        {
            warmupPercentage.text = $"{Mathf.RoundToInt((warmupCount / (float)NPCGenerator.INSTANCE.NPCs.Count) * 100)}%";
        }
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
            if(warmupIndicator == null) return;
            warmupIndicator.color = Color.green;
            StopCoroutine(TextAnimation());
            isUpdating = false;
            warmupText.text = "LLM warmup completed!";
            warmupPercentage.text = "";
            Invoke(nameof(WarmupComplete), 5f);

            Debug.Log("All LLM Characters warmed up.");
            TimeSpan ts = stopwatch.Elapsed;
            Debug.Log($"Warmup time: {ts.Hours}:{ts.Minutes}:{ts.Seconds}");
            stopwatch.Stop();
        }
    }

    void WarmupComplete()
    {
        if (warmupText != null)
        {
            warmupText.text = "";
        }
        if (warmupIndicator != null)
        {
            warmupIndicator.color = Color.clear;
        }
        Debug.Log("LLM Warmup complete.");
    }

    void OnApplicationQuit()
    {
        CancelWarmUp();
    }

    
    public void CancelWarmUp()
    {
        if(!isUpdating) return;
        StopCoroutine(TextAnimation());
        isUpdating = false;
        CancelInvoke(nameof(WarmUp));
        
        for (int i = 0; i < NPCGenerator.INSTANCE.NPCs.Count; i++)
        {
            var npc = NPCGenerator.INSTANCE.NPCs[i];
            if (npc == null) return;
            if (npc.llmCharacter == null) return;
            npc.llmCharacter.warmupCancellationTokenSource.Cancel();
            
        }
        Debug.Log("LLM Warmup cancelled.");
    }
}
