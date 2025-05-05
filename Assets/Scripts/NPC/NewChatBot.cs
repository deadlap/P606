using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

// This class is a new version of the ChatBot class from the LLMUnitySamples namespace.
public class NewChatBot : MonoBehaviour
{
    public static NewChatBot instance;

    [SerializeField] RAGData ragData;
    [SerializeField] bool usingRagData;
    [SerializeField] LLMCharacter llmCharacter;
    //[SerializeField] PiperTTS piperTTS;
    [SerializeField] PhoneticSoundPlayer phoneticSoundPlayer;
    [SerializeField] GameObject chatGraphics;
    [SerializeField] Button chatButton;
    [SerializeField] Transform chatContainer;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text placeholder;

    GameObject playerTextBubble;
    GameObject npcTextBubble;
    string placeholderText = "Hold on...";
    string playerMessage;
    string npcMessage;
    bool blockInput = true;
    bool chatIsActive;
    bool canExitChat = true;

    void Awake()
    {
        chatGraphics.SetActive(false);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        inputField.onSubmit.AddListener(OnInputFieldSubmit);
        inputField.onValueChanged.AddListener(OnValueChanged);
        inputField.onSelect.AddListener(InputFieldSelected);
        chatButton.onClick.AddListener(InputFieldDeselected);
        PlayerInputEvent.PlayerInteract += UpdateChat;
    }

    void OnDisable()
    {
        inputField.onSubmit.RemoveListener(OnInputFieldSubmit);
        inputField.onValueChanged.RemoveListener(OnValueChanged);
        inputField.onSelect.RemoveListener(InputFieldSelected);
        chatButton.onClick.RemoveListener(InputFieldDeselected);
        PlayerInputEvent.PlayerInteract -= UpdateChat;
    }
    
    void Start()
    {
        placeholder.text = placeholderText;
        inputField.GetComponent<Image>().color = ChatBubble.Instance.playerColor;
        inputField.textComponent.color = ChatBubble.Instance.fontColor;
        placeholder.color = ChatBubble.Instance.fontColor;
        inputField.interactable = false;
        if (llmCharacter == null) return;
        _ = llmCharacter.Warmup(WarmUpCallback);
    }

    void OnInputFieldSubmit(string newText)
    {
        _ = InputFieldSubmit(newText);
    }

    async Task InputFieldSubmit(string newText)
    {
        canExitChat = false;
        inputField.ActivateInputField();

        if (blockInput || newText.Trim() == "" || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            StartCoroutine(BlockInteraction()); // Consider refactoring this into a Task-based async method if needed
            return;
        }

        blockInput = true;

        // replace vertical_tab
        playerMessage = inputField.text.Replace("\v", "\n");
        playerTextBubble = ChatBubble.Instance.CreateChatBubble(playerMessage, true, chatContainer);
        UpdateScrollView();
        npcTextBubble = ChatBubble.Instance.CreateChatBubble("Let me think...", false, chatContainer);

        if (usingRagData)
        {
            if (ragData == null)
            {
                Debug.LogError("No RAG Data is found. \"Disable Using Rag Data\" or add a RAG Data and try again.");
                return;
            }

            playerMessage = await ragData.CheckRAG(playerMessage, 1);
            _ = llmCharacter.Chat(playerMessage, SetText, AllowInputAgain);
        }
        else
        {
            _ = llmCharacter.Chat(playerMessage, SetText, AllowInputAgain);
        }

        inputField.text = "";
    }
    /*
    async void OnInputFieldSubmit(string newText)
    {
        canExitChat = false;
        inputField.ActivateInputField();
        if (blockInput || newText.Trim() == "" || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            StartCoroutine(BlockInteraction());
            return;
        }
        blockInput = true;
        // replace vertical_tab
        playerMessage = inputField.text.Replace("\v", "\n");
        playerTextBubble = CreateChatBubble(playerMessage, true);
        UpdateScrollView();
        npcTextBubble = CreateChatBubble("Let me think...", false);
        if (usingRagData)
        {
            if (ragData == null)
            {
                Debug.LogError("No RAG Data is found. \"Disable Using Rag Data\" or add a RAG Data and try again.");   
                return;
            }
            playerMessage = await ragData.CheckRAG(playerMessage, 1);
            _ = llmCharacter.Chat(playerMessage, SetText, AllowInputAgain);
        }
        else
        {
            _ = llmCharacter.Chat(playerMessage, SetText, AllowInputAgain);
        }
        inputField.text = "";
    }
    */
    
    void InputFieldSelected(string arg0)
    {
        chatIsActive = true;
        PlayerInputEvent.OnEnterDialog();
    }

    void InputFieldDeselected()
    {
        if (!canExitChat) return;
        chatIsActive = false;
        PlayerInputEvent.OnExitDialog();
        //if(piperTTS)
            //piperTTS.audioSource.Stop();
        if(phoneticSoundPlayer)
            phoneticSoundPlayer.audioSource.Stop();
        chatGraphics.SetActive(false);
        llmCharacter = null;
        CancelRequests();
    }


    void UpdateChat()
    {
        if(chatIsActive) return;
        StartCoroutine(UpdateChatView());
    }

    IEnumerator UpdateChatView()
    {
        if(PlayerController.instance.currentInteractable.transform.parent == null) yield break;
        if(PlayerController.instance.currentInteractable.transform.parent.transform.tag != "NPC") yield break;
        if (chatContainer.childCount > 0)
        {
            for (int i = 0; i < chatContainer.childCount; i++)
            {
                Destroy(chatContainer.GetChild(i).gameObject);
            }
        }
        chatGraphics.SetActive(true);
        placeholder.text = placeholderText;
        inputField.text = "";
        InputFieldSelected(null);
        yield return new WaitForSeconds(0.1f);
        llmCharacter = PlayerController.instance.currentInteractable.transform.parent.GetComponentInChildren<LLMCharacter>();
        //if (PlayerController.instance.currentInteractable.GetComponentInChildren<PiperTTS>())
            //piperTTS = PlayerController.instance.currentInteractable.GetComponentInChildren<PiperTTS>();
        if (PlayerController.instance.currentInteractable.transform.parent.GetComponentInChildren<PhoneticSoundPlayer>())
            phoneticSoundPlayer = PlayerController.instance.currentInteractable.transform.parent.GetComponentInChildren<PhoneticSoundPlayer>();
        ragData = PlayerController.instance.currentInteractable.transform.parent.GetComponentInChildren<RAGData>();
        for (int i = 0; i < PlayerController.instance.currentInteractable.transform.parent.GetComponent<ChatLog>().playerMessages.Count; i++)
        {
            playerTextBubble = ChatBubble.Instance.CreateChatBubble(PlayerController.instance.currentInteractable.transform.parent.GetComponent<ChatLog>().playerMessages[i], true, chatContainer);
            npcTextBubble = ChatBubble.Instance.CreateChatBubble(PlayerController.instance.currentInteractable.transform.parent.GetComponent<ChatLog>().npcMessages[i], false, chatContainer);
        }
        LogMaster.Instance.RememberConversationStart();
        Start();
    }

    public void WarmUpCallback()
    {
        placeholder.text = $"Ask {llmCharacter.AIName} something...";
        AllowInput();
    }

    void SetText(string text)
    {
        npcMessage = text;
        phoneticSoundPlayer?.StartSpeak(npcMessage);
        if(npcTextBubble.GetComponentInChildren<TMP_Text>() == null) return;
        npcTextBubble.GetComponentInChildren<TMP_Text>().text = npcMessage;
    }

    public void AllowInputAgain()
    {
        if (npcTextBubble)
        {
            npcTextBubble.GetComponentInChildren<TMP_Text>().text = npcMessage;
            AddTextToLog();
        }
        AllowInput();
        //if (piperTTS == null || npcMessage == "" || npcMessage == null) return;
        //piperTTS.OnInputSubmit(npcMessage);
        canExitChat = true;
    }

    void AddTextToLog()
    {
        PlayerController.instance.currentInteractable.transform.parent.GetComponent<ChatLog>().AddTextToLog(playerTextBubble.GetComponentInChildren<TMP_Text>().text, npcMessage);
        chatContainer.GetComponent<ContentSizeFitter>().enabled = false;
        chatContainer.GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void AllowInput()
    {
        blockInput = false;
        inputField.interactable = true;
        inputField.Select();
    }

    public void CancelRequests()
    {
        if(llmCharacter == null) return;
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
