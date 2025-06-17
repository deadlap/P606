using UnityEngine;

public class BugfixManager : MonoBehaviour
{
    public static BugfixManager Instance;
    [SerializeField] GameObject[] body1;
    [SerializeField] GameObject[] head1;
    [SerializeField] GameObject[] body2;
    [SerializeField] GameObject[] head2;
    int count;
    bool hasChanged = false;

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
        count = 0;
        hasChanged = false;
        for (int i = 0; i < body1.Length; i++)
        {
            body1[i].SetActive(true);
            head1[i].SetActive(true);
            body2[i].SetActive(false);
            head2[i].SetActive(false);
        }
    }

    public void CountBugfixes()
    {
        if(hasChanged) return;
        count++;
        if (count >= 9)
        {
            for (int i = 0; i < body1.Length; i++)
            {
                body1[i].SetActive(false);
                head1[i].SetActive(false);
                body2[i].SetActive(true);
                head2[i].SetActive(true);
            }
            hasChanged = true;
        }
    }
}
