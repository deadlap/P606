using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LogMaster : MonoBehaviour
{
    public static LogMaster Instance { get; private set; }

    [SerializeField] private string fileName = "ResearchLog";

    private string filePath;

    private StreamWriter writer;

    private List<Evidence.EvidenceType> evidencesPickedUp = new List<Evidence.EvidenceType>();

    private Dictionary<NPC, int> interactionsPerNpc;

    private bool alreadyFinishedLog = false;

    private float bookUseDuration;
    private float lastBookOpen = 0f;
    private bool bookIsOpen = false;
    private int bookOpenTimes = 0;

    private void Start()
    {
        // Singleton this
        if (Instance != null)
        {
            Debug.LogWarning($"Multiple instances of LogMaster, selfdestructing {name}");
            Destroy(gameObject);
            return;
        }
        Instance = this;

#if UNITY_EDITOR
        alreadyFinishedLog = true;
#endif
    }

    #region Subscribe to GameEnd and BookOpen
    private void OnEnable()
    {
        Ending.EndGameEvent += SaveLogAsTxt;
        BookManager.bookOpenClose += BookChangedState;
    }

    private void OnDisable()
    {
        Ending.EndGameEvent -= SaveLogAsTxt;
        BookManager.bookOpenClose -= BookChangedState;
    }
    #endregion

    public void RememberEvidenceForLog(Evidence.EvidenceType evidenceType)
    {
        evidencesPickedUp.Add(evidenceType);
    }

    public void RememberConversationStart()
    {
        // Create dictionary if first time
        if (interactionsPerNpc == null)
        {
            interactionsPerNpc = new Dictionary<NPC, int>();
            foreach (NPC lad in NPCGenerator.INSTANCE.NPCs)
            {
                interactionsPerNpc.Add(lad, 0);
            }
        }

        interactionsPerNpc[PlayerController.instance.currentInteractable.transform.parent.GetComponent<NPC>()]++;
    }

    private void BookChangedState(bool openNow)
    {
        Debug.Log($"Got told that book is {(openNow ? "open" : "closed")}");
        if (openNow)
        {
            lastBookOpen = Time.time;
            bookOpenTimes++;
        }
        else
            bookUseDuration += Time.time - lastBookOpen;
        bookIsOpen = openNow;
    }

    public void AddLine(string line)
    {
        writer.WriteLine(line);
    }

    public void SaveLogAsTxt()
    {
        if (alreadyFinishedLog) return;
        alreadyFinishedLog = true;

        //Create file
        int logNum = 0;

        filePath = Path.Combine(Application.dataPath, fileName + "_" + logNum + ".txt");

        // If file already exists, create another one
        while (File.Exists(filePath))
        {
            logNum++;
            filePath = Path.Combine(Application.dataPath, fileName + "_" + logNum + ".txt");
        }

        Debug.Log($"Logging at {filePath}");

        writer = File.CreateText(filePath);


        AddLine("--General--");
        AddLine($"Game with victim \"{GameStats.INSTANCE.Victim.NPCIdentity.Name}\" and murderer \"{GameStats.INSTANCE.Murderer.NPCIdentity.Name}\"");
        AddLine($"Player got ending: {(GameTimer.INSTANCE.IsTimeUp() ? "Time's up" : (Ending.instance.bookManager.SelectedNPC.NPCIdentity.PrimaryRole != Identity.PrimaryRoles.Murderer ? "Wrong killer" : "Correct killer"))}");
        AddLine($"Player accused NPC: {(GameTimer.INSTANCE.IsTimeUp() ? "No-one/Timed out ending" : Ending.instance.bookManager.SelectedNPC.NPCIdentity.Name)}");
        AddLine($"Time left: {GameTimer.INSTANCE.GetTimeLeft()}");

        AddLine("");
        AddLine("--Cast--");
        foreach (NPC lad in NPCGenerator.INSTANCE.NPCs)
        {
            AddLine($"{lad.NPCIdentity.Name} - {lad.NPCIdentity.Occupation}");
        }

        AddLine("");
        AddLine("--Evidence--");
        AddLine($"Player got {GameStats.INSTANCE.EvidenceGathered} pieces of evidence");
        string evidenceText = "No evidence gathered";
        if (GameStats.INSTANCE.EvidenceToGather > 0)
        {
            evidenceText = "Evidences gathered: ";
            // List out which pieces of evidence player had gotten
            for (int i = 0; i < evidencesPickedUp.Count; i++)
            {
                evidenceText += evidencesPickedUp[i].ToString();
                if (i < evidencesPickedUp.Count - 1)
                    evidenceText += ", ";
            }
        }
        AddLine(evidenceText);

        AddLine("");
        AddLine("--Interactions with NPCs--");
        foreach(NPC lad in NPCGenerator.INSTANCE.NPCs)
        {
            AddLine($"{lad.NPCIdentity.Name}: {interactionsPerNpc[lad]}");
        }

        AddLine("");
        AddLine("--Messages sent to NPCs--");
        foreach (ChatLog chat in ChatLog.chatLogs)
        {
            AddLine($"{chat.name} messages sent: {chat.playerMessages.Count}");
        }

        AddLine("");
        AddLine("--Book Usage--");
        AddLine($"Book was opened a total of {bookOpenTimes} times");
        if (bookIsOpen)
            BookChangedState(false);
        AddLine($"Book was open for {bookUseDuration} seconds");

        writer.Close();
    }
}
