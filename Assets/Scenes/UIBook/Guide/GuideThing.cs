using Unity.VisualScripting;
using UnityEngine;

public class GuideThing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public GameObject page1;
    public GameObject page2;
    public GameObject page3;

    [HideInInspector] public static bool PageIsActive = false;

    public GameObject GuidePage1;
    public GameObject GuidePage2;
    public GameObject GuidePage3;

    private Transform inFrontOfBook;
    private Transform behindBook;

    // Update is called once per frame
    void Start()
    {
        // ----- FELIX KOMMENTAR -----
        // Træt så lavede en scuff løsning der slap for at expose variabler
        inFrontOfBook = transform.parent.parent.GetChild(0);
        behindBook = transform.parent.parent.GetChild(1);
    }


    public void NewPage()
    {
        GuidePage1.SetActive(false);
        GuidePage2.SetActive(false);
        GuidePage3.SetActive(false);

        PageIsActive = false;

        transform.SetParent(behindBook);
    }

    public void WhenButtonPressed()
    {
        // ----- FELIX KOMMENTAR -----
        // Følgende gør at spørgsmålstegnet skifter mellem at være foran og bagved bogen ift om det er aktivt
        transform.SetParent(transform.parent == behindBook ? inFrontOfBook : behindBook);

        if (page1.activeSelf && PageIsActive == false)
        {
            GuidePage1.SetActive(true);
            GuidePage2.SetActive(false);
            GuidePage3.SetActive(false);
            PageIsActive = true;
            
        }
        else if (page2.activeSelf && PageIsActive == false)
        {
            GuidePage2.SetActive(true);
            GuidePage1.SetActive(false);
            GuidePage3.SetActive(false);
            PageIsActive = true;
        }
        else if (page3.activeSelf && PageIsActive == false)
        {
            GuidePage3.SetActive(true);
            GuidePage1.SetActive(false);
            GuidePage2.SetActive(false);
            PageIsActive = true;
        }
        else if (GuidePage1.activeSelf || GuidePage2.activeSelf || GuidePage3.activeSelf)
        {
            GuidePage1.SetActive(false);
            GuidePage2.SetActive(false);
            GuidePage3.SetActive(false);
            PageIsActive = false;
        }
    }
    
}
