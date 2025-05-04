using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BookManager : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed = 0.1f;
    [SerializeField] private Button scrollUpButton;
    [SerializeField] private Button scrollDownButton;

    private Vector3 targetScale = Vector3.one; // Set your desired target scale here

    private Vector3 targetScalebook = Vector3.one; // Set your desired target scale here
    private Vector3 targetScalebookShrink = Vector3.zero;
    private float shrinkDuration = 0.5f;

    public bool selectedMurderer {get; private set;} = false;

    public bool test;
    private bool istransitioning = false;

    public GameObject areYouSurePopup;

    [HideInInspector, Tooltip("Whether the book opens when pressing Q or not")] public bool openWithQ = true;

    public GameObject book;

    public GameObject page1;
    public GameObject page2;

    public GameObject page3;

    [SerializeField] private string personNameMurderer;

    NPC npc;

    public static List<NPC> NPCSCheckForMurderer;

    private void Start()
    {
        book.transform.localScale = targetScalebookShrink;

        if (scrollUpButton != null)
            scrollUpButton.onClick.AddListener(ScrollUp);

        if (scrollDownButton != null)
            scrollDownButton.onClick.AddListener(ScrollDown);


        if (NPCSCheckForMurderer == null){
            NPCSCheckForMurderer = new List<NPC>(NPCGenerator.INSTANCE.NPCs);
        }
        npc = NPCSCheckForMurderer[0];
        NPCSCheckForMurderer.RemoveAt(0);
        personNameMurderer = npc.NPCIdentity.name;
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        

        if (scrollInput > 0f)
            ScrollUp();
        else if (scrollInput < 0f)
            ScrollDown();

        if (Input.GetKeyDown(KeyCode.Q) && openWithQ)
        {
            OpenOrCloseBook();
        }
    }

    public void OpenOrCloseBook()
    {
        if (istransitioning) return;

        if (!book.activeSelf)
        {
            book.SetActive(true);


            StartCoroutine(EnlargeBookCoroutine(book.transform, targetScalebook, shrinkDuration));
        }
        else
        {

            StartCoroutine(ShrinkBookCoroutine(book.transform, targetScalebookShrink, shrinkDuration));
        }

        istransitioning = true;
    }

    private IEnumerator EnlargeBookCoroutine(Transform bookTrans, Vector3 targetScalebook, float shrinkDuration)
    {
        Vector3 startScale = bookTrans.localScale;
        float elapsed = 0f;

        while (elapsed < shrinkDuration)
        {
            bookTrans.localScale = Vector3.Lerp(startScale, targetScalebook, elapsed / shrinkDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        bookTrans.localScale = targetScalebook; // Make sure it's exact
        istransitioning = false;



        
    }

    private IEnumerator ShrinkBookCoroutine(Transform bookTrans, Vector3 targetScalebook, float shrinkDuration)
    {
        Vector3 startScale = bookTrans.localScale;
        float elapsed = 0f;

        while (elapsed < shrinkDuration)
        {
            bookTrans.localScale = Vector3.Lerp(startScale, targetScalebook, elapsed / shrinkDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        bookTrans.localScale = targetScalebook; // Make sure it's exact
        book.SetActive(false);
        istransitioning = false;
    }
    private void ScrollUp()
    {
        // scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollSpeed);
    }

    private void ScrollDown()
    {
        // scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - scrollSpeed);
    }




    // BookMark Things________________________________________________________________________________________________

    public void BookMark1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
        GuideThing guideThingInstance = FindFirstObjectByType<GuideThing>();
        if (guideThingInstance != null)
        {
            guideThingInstance.NewPage();
            GuideThing.PageIsActive = false;
        }
        else
        {
            Debug.LogWarning("GuideThing instance not found in the scene.");
        }
    }

    public void BookMark2()
    {
        page2.SetActive(true);
        page1.SetActive(false);
        page3.SetActive(false);
        GuideThing guideThingInstance = FindFirstObjectByType<GuideThing>();
        if (guideThingInstance != null)
        {
            guideThingInstance.NewPage();
            GuideThing.PageIsActive = false;
        }
        else
        {
            Debug.LogWarning("GuideThing instance not found in the scene.");
        }
    }


    public void BookMark3()
    {
        page3.SetActive(true);
        page1.SetActive(false);
        page2.SetActive(false);
        GuideThing guideThingInstance = FindFirstObjectByType<GuideThing>();
        if (guideThingInstance != null)
        {
            guideThingInstance.NewPage();
            GuideThing.PageIsActive = false;
        }
        else
        {
            Debug.LogWarning("GuideThing instance not found in the scene.");
        }
    }



    public void CheckIfMurdererIsSelected()
    {
        foreach (var npc in NPCSCheckForMurderer)
        {
            if (npc.NPCIdentity.Name.ToString() != personNameMurderer)
            {
                continue;
            }

            selectedMurderer = npc.NPCIdentity.PrimaryRole == Identity.PrimaryRoles.Murderer;

        }
    }
      
}



    
