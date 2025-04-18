using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class HoverQuestionMarks : MonoBehaviour
{
    [System.Serializable]
    public class HoverTooltipGroup
    {
        public GameObject hoverTarget1;
        public GameObject hoverTarget2;
        public GameObject tooltip;
    }

    [Tooltip("Groups of 2 hover targets each linked to a single tooltip.")]
    public List<HoverTooltipGroup> groups;

    [Tooltip("Delay before showing tooltip.")]
    public float delay = 0.5f;


    public GameObject Bookmark1;
    public GameObject Bookmark2;
    public GameObject Bookmark3;

    public Camera mainCamera; // Assign your main camera in Inspector

    public GameObject textMeshProObject; // The object to show/hide on hover


    private Dictionary<GameObject, HoverTooltipGroup> targetToGroup = new();
    private Dictionary<HoverTooltipGroup, Coroutine> activeCoroutines = new();
    private Dictionary<HoverTooltipGroup, int> hoverCounts = new();

    void Start()
    {
        foreach (var group in groups)
        {
            if (group.tooltip != null)
                group.tooltip.SetActive(false);

            if (group.hoverTarget1 != null)
            {
                targetToGroup[group.hoverTarget1] = group;
                AddEventTriggers(group.hoverTarget1);
            }

            if (group.hoverTarget2 != null)
            {
                targetToGroup[group.hoverTarget2] = group;
                AddEventTriggers(group.hoverTarget2);
            }

            hoverCounts[group] = 0;
        }
    }


    // Det her kan vel nok slettes.
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == Bookmark1 ||
                hit.collider.gameObject == Bookmark2 ||
                hit.collider.gameObject == Bookmark3)
            {
                textMeshProObject.SetActive(true);
                return;
            }
        }
        textMeshProObject.SetActive(false);
    }
  

    private void AddEventTriggers(GameObject target)
    {
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = target.AddComponent<EventTrigger>();

        // Pointer Enter
        EventTrigger.Entry enterEntry = new();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((eventData) => OnHoverEnter(target));
        trigger.triggers.Add(enterEntry);

        // Pointer Exit
        EventTrigger.Entry exitEntry = new();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((eventData) => OnHoverExit(target));
        trigger.triggers.Add(exitEntry);
    }

    private void OnHoverEnter(GameObject target)
    {
        if (!targetToGroup.TryGetValue(target, out var group)) return;

        hoverCounts[group]++;

        if (hoverCounts[group] == 1)
        {
            if (activeCoroutines.ContainsKey(group))
                StopCoroutine(activeCoroutines[group]);

            activeCoroutines[group] = StartCoroutine(ShowTooltipAfterDelay(group));
        }
    }

    private void OnHoverExit(GameObject target)
    {
        if (!targetToGroup.TryGetValue(target, out var group)) return;

        hoverCounts[group] = Mathf.Max(hoverCounts[group] - 1, 0);

        if (hoverCounts[group] == 0)
        {
            if (activeCoroutines.ContainsKey(group))
            {
                StopCoroutine(activeCoroutines[group]);
                activeCoroutines.Remove(group);
            }

            if (group.tooltip != null)
                group.tooltip.SetActive(false);
        }
    }

    private IEnumerator ShowTooltipAfterDelay(HoverTooltipGroup group)
    {
        yield return new WaitForSeconds(delay);

        if (hoverCounts[group] > 0 && group.tooltip != null)
        {
            group.tooltip.SetActive(true);
        }
    }
}
