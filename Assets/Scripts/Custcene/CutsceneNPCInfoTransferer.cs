using RoomFocusing;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Cutscene
{
    public class CutsceneNPCInfoTransferer : MonoBehaviour
    {
        #region For intro cutscene manager to get the information of the NPC
        [HideInInspector] public static List<CutsceneNPCInfoTransferer> infoTransferers { get; private set; } = new List<CutsceneNPCInfoTransferer>();

        private void OnEnable()
        {
            infoTransferers.Add(this);
        }

        private void OnDisable()
        {
            infoTransferers.Remove(this);
        }
        #endregion

        /// <summary>
        /// Transfers info of this NPC to the CutsceneManager, so that the NPC can be rendered correctly and do the correct actions in the cutscene.
        /// </summary>
        /// <param name="bodyMat">The material on this NPC's body (Not Outline)</param>
        /// <param name="headMat">The material on this NPC's head (Not Outline)</param>
        /// <param name="extraClothing">The hat that this NPC is wearing</param>
        /// <param name="theOccupation">The occupation this NPC has</param>
        /// <param name="theOccupation">The name of this NPC</param>
        public void TransferInfo(Material bodyMat, Material headMat, GameObject extraClothing, Identity.Occupations theOccupation, string theName)
        {
            TransferInfo(new CutsceneNPCInfo(bodyMat, headMat, extraClothing, theOccupation, theName));
        }

        /// <summary>
        /// Transfers info of this NPC to the CutsceneManager, so that the NPC can be rendered correctly and do the correct actions in the cutscene.
        /// </summary>
        /// <param name="infoToTransfer">The info about this NPC's looks and occupation</param>
        public void TransferInfo(CutsceneNPCInfo infoToTransfer)
        {
            infoToTransfer = CheckNullValues(infoToTransfer);

            IntroCutsceneManager.instance.InformOfNPC(infoToTransfer);
        }

        /// <summary>
        /// Transfers the identity of this NPC to the CutsceneManager, so that the NPC can be rendered correctly and do the correct actions in the cutscene.
        /// </summary>
        /// <param name="npcIdentity">The Identity for the given NPC</param>
        /// <param name="headMat">The face material the given NPC has</param>
        public void TransferInfo(Identity npcIdentity = null, Material headMat = null)
        {
            if (npcIdentity == null)
            {
                npcIdentity = GetComponent<Identity>();
            }

            if (headMat == null)
            {
                GetComponent<ShadowPerson>().GetLooks();
                headMat = GetComponent<ShadowPerson>().originalHead[1];
            }

            IntroCutsceneManager.instance.InformOfNPC(npcIdentity, headMat);
        }

        private CutsceneNPCInfo CheckNullValues(CutsceneNPCInfo info)
        {
            if (info.bodyMaterial == null)
            {
                Debug.Log($"{name} had no body material assigned, trying to find one automatically");
                if (!TryGetMaterial("Body", out Material newBodyMat))
                {
                    Debug.LogWarning($"No body material found for {name}");
                }
                else
                {
                    info.bodyMaterial = newBodyMat;
                }
            }
            if (info.headMaterial == null)
            {
                Debug.Log($"{name} had no head material assigned, trying to find one automatically");
                if (!TryGetMaterial("Head", out Material newHeadMat))
                {
                    Debug.LogWarning($"No body material found for {name}");
                }
                else
                {
                    info.headMaterial = newHeadMat;
                }
            }
            if (info.hat == null)
            {
                Debug.Log($"{name} had no hat assigned, trying to find one automatically");
                if (!TryGetMaterial("Head", out Material newHeadMat))
                {
                    Debug.Log($"{name} has no hat, very sad ;(");
                }
                else
                {
                    info.hat = transform.Find("Head").GetChild(0).gameObject;
                }
            }
            if (info.occupation == Identity.Occupations.None) Debug.LogWarning($"{name} has no job?");
            if (info.myName == null)
            {
                Debug.Log($"{name} had no name assigned, automatically assigning it as {name}");
                info.myName = name;
            }

            return info;
        }

        private bool TryGetMaterial(string part, out Material partMat)
        {
            partMat = null;
            if (!transform.Find(part).TryGetComponent(out Renderer partRenderer))
            {
                return false;
            }

            partMat = partRenderer.materials[1];
            return true;
        }
    }
}
