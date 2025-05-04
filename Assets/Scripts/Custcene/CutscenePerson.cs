using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Cutscene
{
    public class CutscenePerson : MonoBehaviour
    {
        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Renderer headRenderer;
        [SerializeField] private TextMeshPro nameField;
        private Identity.Occupations myOccupation;
        private string myName;

#if UNITY_EDITOR
        [SerializeField] private bool isTesting = false;
        [SerializeField] private CutsceneNPCInfo whoAmI;
#endif

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
#if UNITY_EDITOR
            if (isTesting)
            {
                Debug.Log($"Telling manager");
                IntroCutsceneManager.instance.InformOfNPC(whoAmI);
            }
#endif
        }

        public void AssignLooks(CutsceneNPCInfo npcInfo)
        {
            //Debug.Log("I was told by manager");

            UpdateMaterial(bodyRenderer, npcInfo.bodyMaterial);
            UpdateMaterial(headRenderer, npcInfo.headMaterial);

            if (npcInfo.hat != null)
            {
                Instantiate(npcInfo.hat, headRenderer.transform);
            }
            if (npcInfo.carriable != null)
            {
                Instantiate(npcInfo.carriable, bodyRenderer.transform);
            }

            myOccupation = npcInfo.occupation;
            myName = npcInfo.myName;
            //name = $"NPC {myName}";

            nameField.text = string.Format(nameField.text, myName, myOccupation.ToString().Replace('_', ' '));

            //Debug.Log(npcInfo);
        }

        private void UpdateMaterial(Renderer renderer, Material material)
        {
            List<Material> bodyMats = new List<Material>(renderer.materials);
            bodyMats[1] = material;
            renderer.SetMaterials(bodyMats);
        }

        public void ChangeOutfit(Material newOutfit)
        {
            UpdateMaterial(bodyRenderer, newOutfit);
        }
    }
}
