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
    [SerializeField] public TMP_InputField inputField;
    [SerializeField] TMP_Text placeholder;

    GameObject playerTextBubble;
    GameObject npcTextBubble;
    string placeholderText = "Hold on...";
    string playerMessage;
    string npcMessage;
    public bool blockInput = true;
    public bool isChatActive;
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
        PlayerInputEvent.CloseUI += InputFieldDeselected;
        PlayerInputEvent.PlayerInteract += UpdateChat;
    }

    void OnDisable()
    {
        inputField.onSubmit.RemoveListener(OnInputFieldSubmit);
        inputField.onValueChanged.RemoveListener(OnValueChanged);
        inputField.onSelect.RemoveListener(InputFieldSelected);
        chatButton.onClick.RemoveListener(InputFieldDeselected);
        PlayerInputEvent.CloseUI -= InputFieldDeselected;
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
        Invoke(nameof(ActivateInputField), 1f);

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
        }
        _ = llmCharacter.Chat(playerMessage, SetText, AllowInputAgain);
        
        inputField.text = "";
    }

    void ActivateInputField()
    {
        inputField.ActivateInputField();
    }
    
    void InputFieldSelected(string arg0)
    {
        isChatActive = true;
        PlayerInputEvent.OnEnterDialog();
    }

    void InputFieldDeselected()
    {
        if (!canExitChat) return;
        PlayerInputEvent.isUIOpen = false;
        isChatActive = false;
        PlayerInputEvent.OnExitDialog();
        //if(piperTTS)
            //piperTTS.audioSource.Stop();
        if(phoneticSoundPlayer)
            phoneticSoundPlayer.audioSource.Stop();
        chatGraphics.SetActive(false);
        llmCharacter = null;
        //BookManager.instance.FreezeOrUnfreezeBook(false);
        CancelRequests();
    }


    void UpdateChat()
    {
        if(isChatActive) return;
        StartCoroutine(UpdateChatView());
    }

    IEnumerator UpdateChatView()
    {
        if(PlayerController.instance.currentInteractable.transform.parent == null) yield break;
        if(PlayerController.instance.currentInteractable.transform.parent.transform.tag != "NPC") yield break;

        //BookManager.instance.FreezeOrUnfreezeBook(true);

        if (chatContainer.childCount > 0)
        {
            for (int i = 0; i < chatContainer.childCount; i++)
            {
                Destroy(chatContainer.GetChild(i).gameObject);
            }
        }
        chatGraphics.SetActive(true);
        PlayerInputEvent.isUIOpen = true;
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
        RefreshContentSizeFitter();
        LogMaster.Instance.RememberConversationStart();
        Start();
    }

    public void WarmUpCallback()
    {
        placeholder.text = $"Write a question to {llmCharacter.AIName}...";
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
        RefreshContentSizeFitter();
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

    public void GetText(string text)
    {
        inputField.text = text;
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
        if (newText.Contains("\\"))
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

    void RefreshContentSizeFitter()
    {
        chatContainer.GetComponent<ContentSizeFitter>().enabled = false;
        Invoke(nameof(EnableContentSizeFitter), 0.1f);
    }

    private void EnableContentSizeFitter()
    {
        chatContainer.GetComponent<ContentSizeFitter>().enabled = true;
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
