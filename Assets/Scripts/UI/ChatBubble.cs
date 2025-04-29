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
    public int fontSize = 16;
    public int bubbleWidth = 350;
    public int bubbleHeight = 35;
    public Sprite sprite;

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
}
