using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
    public class CutscenePerson : MonoBehaviour
    {
        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Renderer headRenderer;
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
                Debug.Log("Telling manager");
                IntroCutsceneManager.instance.InformOfNPC(whoAmI);
            }
#endif
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AssignLooks(CutsceneNPCInfo npcInfo)
        {
            Debug.Log("I was told by manager");

            UpdateMaterial(bodyRenderer, npcInfo.bodyMaterial);
            UpdateMaterial(headRenderer, npcInfo.headMaterial);

            Transform spawnedHat = Instantiate(npcInfo.hat, headRenderer.transform.position, Quaternion.identity).transform;
            spawnedHat.SetParent(headRenderer.transform);
            myOccupation = npcInfo.occupation;
            myName = npcInfo.myName;
            name = $"NPC {myName}";

            Debug.Log(npcInfo);
        }

        private void UpdateMaterial(Renderer renderer, Material material)
        {
            List<Material> bodyMats = new List<Material>(renderer.materials);
            bodyMats[1] = material;
            renderer.SetMaterials(bodyMats);
        }
    }
}
