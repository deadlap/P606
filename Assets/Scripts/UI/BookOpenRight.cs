using UnityEngine;

public class BookOpenRight : MonoBehaviour
{
    [SerializeField] private GameObject[] toDeactivate;
    [SerializeField] private GameObject[] toActivate;

    private void Awake()
    {
        foreach (GameObject obj in toDeactivate)
            obj.SetActive(false);
        foreach (GameObject obj in toActivate)
            obj.SetActive(true);
    }
}
