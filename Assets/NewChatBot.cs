using LLMUnity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

// This class is a new version of the ChatBot class from the LLMUnitySamples namespace.
public class NewChatBot : MonoBehaviour
{
    [SerializeField] LLMCharacter llmCharacter;
    [SerializeField] PiperTTS piperTTS;
    [SerializeField] Transform chatContainer;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text placeholder;
    [SerializeField] Color playerColor = new Color32(81, 164, 81, 255);
    [SerializeField] Color aiColor = new Color32(29, 29, 73, 255);
    [SerializeField] Color fontColor = Color.white;
    [SerializeField] TMP_FontAsset font;
    [SerializeField] int fontSize = 16;
    [SerializeField] int bubbleWidth = 350;
    [SerializeField] int bubbleHeight = 35;
    [SerializeField] Sprite sprite;

    List<GameObject> chatBubbles = new List<GameObject>();
    GameObject playerTextBubble;
    GameObject aiTextBubble;
    string playerText;
    string aiText;
    bool blockInput = true;

    void OnEnable()
    {
        inputField.onSubmit.AddListener(OnInputFieldSubmit);
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    void OnDisable()
    {
        inputField.onSubmit.RemoveListener(OnInputFieldSubmit);
        inputField.onValueChanged.RemoveListener(OnValueChanged);
    }

    void Start()
    {
        if (font == null) font = Resources.GetBuiltinResource<TMP_FontAsset>("Arial SDF");
        placeholder.text = "Hold on...";
        inputField.GetComponent<Image>().color = playerColor;
        inputField.textComponent.color = fontColor;
        placeholder.color = fontColor;
        inputField.interactable = false;
        _ = llmCharacter.Warmup(WarmUpCallback);
    }

    void OnInputFieldSubmit(string newText)
    {
        inputField.ActivateInputField();
        if (blockInput || newText.Trim() == "" || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            StartCoroutine(BlockInteraction());
            return;
        }
        blockInput = true;
        // replace vertical_tab
        string message = inputField.text.Replace("\v", "\n");

        CreateChatBubble(message, true);
        UpdateScrollView();
        aiTextBubble = CreateChatBubble("Let me think...", false);
        Task chatTask = llmCharacter.Chat(message, SetText, AllowInput);
        inputField.text = "";
    }

    GameObject CreateChatBubble(string text, bool isPlayerMessage)
    {
        string type = isPlayerMessage ? "Player" : "AI";
        GameObject chatBubble = Instantiate(new GameObject($"{type} Text Bubble"), chatContainer);
        chatBubble.AddComponent<Image>().color = Color.clear;
        chatBubble.GetComponent<RectTransform>().sizeDelta = new Vector2(400, bubbleHeight);
       
        HorizontalLayoutGroup hlgChatBubble = chatBubble.AddComponent<HorizontalLayoutGroup>(); 
        hlgChatBubble.childAlignment = isPlayerMessage ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
        hlgChatBubble.GetComponent<HorizontalLayoutGroup>().childControlHeight = false;
        hlgChatBubble.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
        hlgChatBubble.GetComponent<HorizontalLayoutGroup>().childForceExpandHeight = false;
        hlgChatBubble.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;
        chatBubble.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        Image bubbleImage = Instantiate(new GameObject($"{type} Bubble"), chatBubble.transform).AddComponent<Image>();
        bubbleImage.GetComponent<Image>().color = isPlayerMessage ? playerColor : aiColor;
        bubbleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(bubbleWidth, bubbleHeight);
        bubbleImage.GetComponent<Image>().sprite = sprite;
        bubbleImage.GetComponent<Image>().type = Image.Type.Sliced;
        bubbleImage.AddComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 10, 10, 10);
        bubbleImage.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        TMP_Text bubbleText = Instantiate(new GameObject($"{type} Text"), bubbleImage.transform).AddComponent<TextMeshProUGUI>();
        bubbleText.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 0);
        bubbleText.text = text;
        bubbleText.color = fontColor;
        bubbleText.font = font;
        bubbleText.fontSize = fontSize;
        bubbleText.alignment = isPlayerMessage ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;

        return chatBubble;
    }
    public void WarmUpCallback()
    {
        placeholder.text = $"Ask {llmCharacter.AIName} something...";
        AllowInput();
    }

    void SetText(string text)
    {
        aiText = text;
        aiTextBubble.GetComponentInChildren<TMP_Text>().text = aiText;
    }

    public void AllowInput()
    {
        if(aiTextBubble)
        {
            aiTextBubble.GetComponentInChildren<TMP_Text>().text = aiText;
        }
        blockInput = false;
        inputField.interactable = true;
        inputField.Select();
        if (piperTTS == null || aiText == "" || aiText == null) return;
        print(aiText);
        piperTTS.OnInputSubmit(aiText);
    }

    public void CancelRequests()
    {
        llmCharacter.CancelRequests();
        AllowInput();
    }

    IEnumerator<string> BlockInteraction()
    {
        // prevent from change until next frame
        inputField.interactable = false;
        yield return null;
        inputField.interactable = true;
        // change the caret position to the end of the text
        inputField.MoveTextEnd(true);
    }

    void OnValueChanged(string newText)
    {
        if(newText.Contains("\\"))
        {
            inputField.text = newText.Replace("\\", "");
        }
        // Get rid of newline character added when we press enter
        if (Input.GetKey(KeyCode.Return))
        {
            if (inputField.text.Trim() == "")
                inputField.text = "";
        }
    }

    void UpdateScrollView()
    {
        if (blockInput)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
    void Update()
    {
        UpdateScrollView();
    }
}
