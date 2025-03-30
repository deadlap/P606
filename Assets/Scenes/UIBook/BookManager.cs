using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BookManager : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed = 0.1f;
    [SerializeField] private Button scrollUpButton;
    [SerializeField] private Button scrollDownButton;

    private Vector3 targetScale = new Vector3(0.16f, 0.28f, 0.28f); // Set your desired target scale here

    private Vector3 targetScalebook = new Vector3(17.22f, 9.67f, 9.27f); // Set your desired target scale here
    private Vector3 targetScalebookShrink = new Vector3(3.44000006f,1.93372798f,1.85230792f);
    private float shrinkDuration = 0.5f;

    public bool test;
    private bool istransitioning = false;
    public GameObject waxStampTarget;

    public GameObject book;

    public GameObject page1;
    public GameObject page2;

    public GameObject page3;

    private void Start()
    {
        if (scrollUpButton != null)
            scrollUpButton.onClick.AddListener(ScrollUp);

        if (scrollDownButton != null)
            scrollDownButton.onClick.AddListener(ScrollDown);
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        

        if (scrollInput > 0f)
            ScrollUp();
        else if (scrollInput < 0f)
            ScrollDown();

        if (Input.GetKeyDown(KeyCode.E) && istransitioning == false)
        {
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
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollSpeed);
    }

    private void ScrollDown()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - scrollSpeed);
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

    // WaxStamp Things________________________________________________________________________________________________
    public void Accuse()
     {
        
        if (waxStampTarget != null && PersonIdentification.SomeoneIsSelected)
        {
            StartCoroutine(ShrinkStampCoroutine(waxStampTarget.transform, targetScale, shrinkDuration));
            waxStampTarget.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Wax stamp target not assigned in BookManager.");
        }
    }

      private IEnumerator ShrinkStampCoroutine(Transform target, Vector3 endScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = endScale; // Make sure it's exact
        
    }
}



    
