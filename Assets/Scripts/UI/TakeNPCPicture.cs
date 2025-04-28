using Cutscene;
using RoomFocusing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeNPCPicture : MonoBehaviour
{
    #region For book to take picture of all NPCs when it is first opened
    [HideInInspector] protected static List<TakeNPCPicture> npcSelfieCams { get; private set; } = new List<TakeNPCPicture>();

    private void OnEnable()
    {
        npcSelfieCams.Add(this);
    }

    private void OnDisable()
    {
        npcSelfieCams.Remove(this);
    }
    #endregion

    [SerializeField] private Camera m_Camera;
    [SerializeField] private RenderTexture basicTextureSetup;
    public RenderTexture outPutTexture { get; private set; }
#if UNITY_EDITOR
    [SerializeField] private RawImage testShowcase;
#endif
    [SerializeField] private GameObject model;

    [Header("NPC look info")]
    [SerializeField] private Material[] bodyMats;
    [SerializeField] private GameObject[] hats;
    [SerializeField] private GameObject[] carriables;

    public static void TakeAllPictures()
    {
        foreach(TakeNPCPicture selfieCam in npcSelfieCams)
        {
            selfieCam.TakePicture();
        }
    }

    public RenderTexture TakePicture()
    {
        int identityID = (int)GetComponent<Identity>().Occupation;
        Debug.Log($"{name} got Occupation #{identityID}, which means bodyMat {bodyMats[identityID].name} ({bodyMats[identityID]})");
        GetComponent<ShadowPerson>().GetLooks();
        Material headMat = GetComponent<ShadowPerson>().originalHead[1];
        model.GetComponent<PictureModel>().AssignLooks(headMat, bodyMats[identityID], hats[identityID], carriables[identityID]);
#if UNITY_EDITOR
        if (testShowcase != null)
            testShowcase.texture = outPutTexture;
#endif
        // Maybe delay the deactivation with a frame
        StartCoroutine(DelayDeath());
        return outPutTexture;
    }

    private IEnumerator DelayDeath()
    {
        // Wait a frame
        yield return null;

        // Kill the model
        model.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model = Instantiate(model);
        model.transform.position += Vector3.left * 10 * npcSelfieCams.IndexOf(this);
        outPutTexture = new RenderTexture(basicTextureSetup);
        m_Camera = model.GetComponentInChildren<Camera>();
        m_Camera.targetTexture = outPutTexture;
    }
}
