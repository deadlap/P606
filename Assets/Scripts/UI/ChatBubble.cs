using LLMUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{
    public static ChatBubble Instance;

    public Color playerColor = new Color32(81, 164, 81, 255);
    public Color aiColor = new Color32(29, 29, 73, 255);
    public Color fontColor = Color.white;
    public TMP_FontAsset font;
    public int fontSize = 20;
    public int bubbleWidth = 350;
    public int bubbleHeight = 35;
    public Sprite sprite;
    GameObject chatBubblePrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreateChatBubble(string text, bool isPlayerMessage, Transform chatContainer)
    {
        string type = isPlayerMessage ? "Player" : "AI";
        GameObject chatBubble = new GameObject($"{type} Text Bubble");
        chatBubble.transform.SetParent(chatContainer);
        chatBubble.AddComponent<Image>().color = Color.clear;
        chatBubble.GetComponent<RectTransform>().sizeDelta = new Vector2(400, bubbleHeight);

        HorizontalLayoutGroup hlgChatBubble = chatBubble.AddComponent<HorizontalLayoutGroup>();
        hlgChatBubble.childAlignment = isPlayerMessage ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
        hlgChatBubble.childControlHeight = false;
        hlgChatBubble.childControlWidth = false;
        hlgChatBubble.childForceExpandHeight = false;
        hlgChatBubble.childForceExpandWidth = false;
        chatBubble.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        Image bubbleImage = new GameObject($"{type} Bubble").AddComponent<Image>();
        bubbleImage.transform.SetParent(chatBubble.transform);
        bubbleImage.GetComponent<Image>().color = isPlayerMessage ? playerColor : aiColor;
        bubbleImage.transform.localScale = isPlayerMessage ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1); // flips the image so the speach bubble points the right way
        bubbleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(bubbleWidth, bubbleHeight);
        bubbleImage.sprite = sprite;
        bubbleImage.type = Image.Type.Sliced;
        bubbleImage.AddComponent<HorizontalLayoutGroup>().padding = new RectOffset(30, 30, 25, 60);
        bubbleImage.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        TMP_Text bubbleText = new GameObject($"{type} Text").AddComponent<TextMeshProUGUI>();
        bubbleText.transform.SetParent(bubbleImage.transform);
        bubbleText.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 0);
        bubbleText.transform.localScale = isPlayerMessage ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1); // flips the text so it is the right way around
        bubbleText.text = text;
        bubbleText.color = fontColor;
        bubbleText.font = font;
        bubbleText.fontSize = fontSize;
        bubbleText.alignment = isPlayerMessage ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
        bubbleText.lineSpacing = 15;
        bubbleText.characterSpacing = 2;
        bubbleText.wordSpacing = 10;


        return chatBubble;
    }
}
