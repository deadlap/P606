using UnityEngine;
using System.IO;

public class LogMaster : MonoBehaviour
{
    public static LogMaster Instance { get; private set; }

    [SerializeField] private string fileName = "ResearchLog";
    private string textToLog = string.Empty;

    private string filePath;

    private StreamWriter writer;

    private void Awake()
    {
        // Singleton this
        if (Instance != null)
        {
            Debug.LogWarning($"Multiple instances of LogMaster, selfdestructing {name}");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //Create file
        filePath = Path.Combine(Application.dataPath, fileName + ".txt");

        // If file already exists, create another one
        int logNum = 1;
        while (File.Exists(filePath))
        {
            filePath = Path.Combine(Application.dataPath, fileName + "_" + logNum + ".txt");
        }

        writer = File.CreateText(filePath);
    }

    public void AddLine(string line)
    {
        writer.WriteLine(line);
    }

    private void OnDisable()
    {
        SaveLogAsTxt();
    }

    private void SaveLogAsTxt()
    {
        writer.Close();
    }
}
