using System.Collections.Generic;
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

        private int hasTriedUpdateLooks = 0;

        private static List<int> usedFaces = new List<int>();

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

        // Made GetLooks() public so that the Manager can make sure the NPC they're cosplaying actually has a face material assigned
        public void GetLooks()
        {
            if (hasGottenLooks) return;
            Material[] headMaterials = head.materials;
            Material[] bodyMaterials = body.materials;
            if (GetComponent<NPC>().NPCIdentity.PrimaryRole == Identity.PrimaryRoles.Victim){
                headMaterials[headMaterials.Length - 1] = Resources.Load<Material>("Faces/Victim") as Material;
            } else {
                
                int rand = Random.Range(1,13 - usedFaces.Count);
                foreach (int usedFace in usedFaces)
                {
                    if (rand >= usedFace)
                        rand++;
                }
                usedFaces.Add(rand);
                usedFaces.Sort();
                headMaterials[headMaterials.Length - 1] = Resources.Load<Material>("Faces/Face"+rand) as Material;
            }
            bodyMaterials[bodyMaterials.Length - 1] = Resources.Load<Material>("Outfits/"+GetComponent<NPC>().NPCIdentity.Occupation.ToString()) as Material;
            originalHead = headMaterials;
            originalBody = bodyMaterials;

            hasGottenLooks = true;
        }

        private void UpdateAccessories()
        {
            // Trying to get the hat a couple of times, as it is only given after some time has passed
            if (hasTriedUpdateLooks < 3)
            {
                // Save basic accessories
                List<GameObject> listOfAccessories = new List<GameObject>()
                {
                    accessories[0],
                    accessories[1],
                    accessories[2],
                    accessories[3]
                };

                if (head.transform.childCount > 0)
                    listOfAccessories.Add(head.transform.GetChild(0).gameObject);
                if (body.transform.GetChild(0).childCount > 0)
                    listOfAccessories.Add(body.transform.GetChild(0).GetChild(0).gameObject);

                accessories = listOfAccessories.ToArray();

                hasTriedUpdateLooks++;
            }

            if (accessories == null) return;
            if (playersCanSee < 1)
            {
                foreach (GameObject accessory in accessories)
                {
                    accessory.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject accessory in accessories)
                {
                    accessory.SetActive(true);
                }
            }
        }
    }
}
