using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
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
    [SerializeField] Image warmupScreen;
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
        if (warmupScreen != null)
        {
            warmupScreen.color = Color.clear;
            warmupScreen.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        PlayerInputEvent.QuitToMainMenu += CancelWarmUp;
        GameStats.SetIntroPlayed += WaitingScreen;
    }

    void OnDisable()
    {
        PlayerInputEvent.QuitToMainMenu -= CancelWarmUp;
        GameStats.SetIntroPlayed -= WaitingScreen;
    }

    void Start()
    {
        warmupText = CreateText(warmupIndicator.transform, textPosition, "left", "Warming up LLM Characters");
        warmupPercentage = CreateText(warmupText.transform, percentagePosition, "right", "0%");
        StartCoroutine(TextAnimation());
    }

    TextMeshProUGUI CreateText(Transform parent, Vector3 position, string allignment, string text)
    {
        var textMesh = new GameObject().AddComponent<TextMeshProUGUI>();
        textMesh.font = font;
        textMesh.fontSize = fontSize;
        if (allignment.ToLower() == "right")
            textMesh.alignment = TextAlignmentOptions.MidlineRight;
        else
            textMesh.alignment = TextAlignmentOptions.MidlineLeft;
        textMesh.textWrappingMode = TextWrappingModes.NoWrap;
        textMesh.text = text;
        textMesh.transform.parent = parent;
        textMesh.transform.localPosition = position;
        return textMesh;
    }

    void Update()
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
                warmupText.text = "Please wait while LLM Characters are warming up";
                yield return new WaitForSeconds(0.5f);
                warmupText.text = "Please wait while LLM Characters are warming up.";
                yield return new WaitForSeconds(0.5f);
                warmupText.text = "Please wait while LLM Characters are warming up..";
                yield return new WaitForSeconds(0.5f);
                warmupText.text = "Please wait while LLM Characters are warming up...";
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
            Invoke(nameof(WarmupComplete), 1f);
            TimeSpan ts = stopwatch.Elapsed;
            Debug.Log($"Warmup time: {ts.Hours}:{ts.Minutes}:{ts.Seconds}");
            stopwatch.Stop();
        }
    }

    void WarmupComplete()
    {
        warmupText.text = "LLM warmup completed!";
        warmupPercentage.text = "";
        Debug.Log("LLM Warmup complete.");
        Invoke(nameof(RemoveUI), 2f);
    }

    void RemoveUI()
    {
        if (warmupText != null)
        {
            warmupText.text = "";
        }
        if (warmupIndicator != null)
        {
            warmupIndicator.color = Color.clear;
        }
        if (warmupScreen != null)
        {
            StartCoroutine(FadeScreen(1f));
        }
    }

    void WaitingScreen()
    {
        if (warmupScreen != null && isUpdating)
        {

            PlayerController.instance.FreezePlayer(true);
            warmupScreen.gameObject.SetActive(true);
            warmupScreen.color = Color.black;
        }
    }

    IEnumerator FadeScreen(float fadeTime)
    {
        float time = 0;
        if (warmupScreen == null) yield break;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, time / fadeTime);
            var color = warmupScreen.color;
            color.a = alpha;
            warmupScreen.color = color;
            yield return null;
        }
        PlayerController.instance.FreezePlayer(false);
        warmupScreen.gameObject.SetActive(false);
        GameTimer.OnToggleTimer(true);
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
            npc.llmCharacter.warmupCancellationTokenSource.Token.ThrowIfCancellationRequested();
        }
        Debug.Log("LLM Warmup cancelled.");
    }
}
