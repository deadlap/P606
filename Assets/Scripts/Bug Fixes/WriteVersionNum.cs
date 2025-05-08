using UnityEngine;

public class WriteVersionNum : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TMPro.TextMeshPro>().text = string.Format(GetComponent<TMPro.TextMeshPro>().text, Application.version);
    }
}
