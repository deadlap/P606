using UnityEngine;

namespace RoomFocusing
{
    public class ShadowPerson : MonoBehaviour
    {
        [SerializeField] private GameObject nameTag;

        [SerializeField] private MeshRenderer head;
        [SerializeField] private MeshRenderer body;
        [SerializeField] private Material shadowMaterial;
        [SerializeField, Tooltip("The accessories that cannot be seen when the NPCs are shadows")] private GameObject[] accessories;

        public Material[] originalHead;
        public Material[] originalBody;

        bool hasGottenLooks;
        private int playersCanSee = 0;

        public void AddPlayers(int howMany = 1)
        {
            GetLooks();
            playersCanSee += howMany;
            UpdateLooks();
        }

        public void RemovePlayers(int howMany = 1)
        {
            GetLooks();
            playersCanSee -= howMany;
            UpdateLooks();
        }

        private void UpdateLooks()
        {
            UpdateAccessories();
            nameTag.SetActive(playersCanSee > 0);
            if (playersCanSee > 0)
            {
                Material[] headMaterials = head.materials;
                Material[] bodyMaterials = body.materials;
                headMaterials = originalHead;
                bodyMaterials = originalBody;
                head.materials = headMaterials;
                body.materials = bodyMaterials;
            }
            if (playersCanSee < 1)
            {
                Material[] headMaterials = head.materials;
                Material[] bodyMaterials = body.materials;
                headMaterials[headMaterials.Length - 1] = shadowMaterial;
                bodyMaterials[bodyMaterials.Length - 1] = shadowMaterial;
                head.materials = headMaterials;
                body.materials = bodyMaterials;
            }
        }

        private void GetLooks()
        {
            if (hasGottenLooks) return;
            Material[] headMaterials = head.materials;
            Material[] bodyMaterials = body.materials;
            headMaterials[headMaterials.Length - 1] = headMaterials[1];
            bodyMaterials[bodyMaterials.Length - 1] = bodyMaterials[1];
            originalHead = headMaterials;
            originalBody = bodyMaterials;
            hasGottenLooks = true;
        }

        private void UpdateAccessories()
        {
            if (accessories == null) return;
            if (playersCanSee < 1)
            {
                foreach (var accessory in accessories)
                {
                    accessory.SetActive(false);
                }
            }
            else
            {
                foreach (var accessory in accessories)
                {
                    accessory.SetActive(true);
                }
            }
        }
    }
}
