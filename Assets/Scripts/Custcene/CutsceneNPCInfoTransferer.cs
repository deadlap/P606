using UnityEngine;

namespace Cutscene
{
    public class CutsceneNPCInfoTransferer : MonoBehaviour
    {
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
            IntroCutsceneManager.instance.InformOfNPC(infoToTransfer);
        }
    }
}
