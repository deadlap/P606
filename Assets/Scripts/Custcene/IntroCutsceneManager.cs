using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Cutscene
{
    public enum CutsceneVariations
    {
        crewAlive,
        chefDead,
        waiterDead,
        janitorDead,
        bartenderDead
    }

    public class IntroCutsceneManager : MonoBehaviour
    {
        [HideInInspector] public static IntroCutsceneManager instance;
        [SerializeField, Tooltip("Assign the cutscene variations here. In order of Crew, noChef, noWaiter, noJanitor, noBartender")] private TimelineAsset[] cutsceneTimelines; 
        [SerializeField, Tooltip("Who are the actors in the cutscene")] private CutscenePerson[] personel;
        [SerializeField, Tooltip("What are the outfits, in order of Identity.Occupations")] private Material[] bodyMaterials;
        [SerializeField, Tooltip("What are the hat prefabs, in order of Identity.Occupations")] private GameObject[] hats;
        [SerializeField, Tooltip("What do they carry, in order of Identity.Occupations")] private GameObject[] carriables;

        private CutsceneVariations cutsceneVariation = CutsceneVariations.crewAlive;

        private List<CutsceneNPCInfo> people = new List<CutsceneNPCInfo>();

        private bool hasBegunCutscene = false;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("There already exists an instance of the IntroCutsceneManager?");
            }
            instance = this;
        }

        /// <summary>
        /// Extracts info about NPC needed for cutscene from its identity
        /// </summary>
        /// <param name="npcIdentity"></param>
        /// <param name="headMaterial"></param>
        public void InformOfNPC(Identity npcIdentity, Material headMaterial)
        {
            CutsceneNPCInfo personInfo = new CutsceneNPCInfo();

            int occupationInt = (int)npcIdentity.Occupation;

            // Visual identity
            personInfo.headMaterial = headMaterial;
            personInfo.bodyMaterial = bodyMaterials[occupationInt];
            personInfo.hat = hats[occupationInt];
            personInfo.carriable = carriables[occupationInt];

            // Name, occupation, victim status
            personInfo.occupation = npcIdentity.Occupation;
            personInfo.myName = npcIdentity.Name.ToString();
            personInfo.isDead = npcIdentity.PrimaryRole == Identity.PrimaryRoles.Victim;

            InformOfNPC(personInfo);
        }
        
        public void InformOfNPC(CutsceneNPCInfo thisNPCInfo)
        {
            if (people.Count >= personel.Length)
            {
                Debug.LogWarning($"More than {personel.Length} NPCs inform the CutsceneManager of their generation. Maybe IsTesting is true for some CutscenePerson scripts?");
                return;
            }

            people.Add(thisNPCInfo);

            if (people.Count == personel.Length)
            {
                Debug.Log("Manager tells all cutscene persons");

                int regularLads = 0;

                bool[] missingRole = new bool[4] { true, true, true, true };

                for (int i = 0; i < personel.Length; i++)
                {
                    // Assign dead person to end
                    if (people[i].isDead)
                    {
                        personel[7].AssignLooks(people[i]);

                        // Set which variation of cutscene should play
                        switch (people[i].occupation)
                        {
                            case Identity.Occupations.Chef:
                                cutsceneVariation = CutsceneVariations.chefDead;
                                break;
                            case Identity.Occupations.Janitor:
                                cutsceneVariation = CutsceneVariations.janitorDead;
                                break;
                            case Identity.Occupations.Waiter:
                                cutsceneVariation = CutsceneVariations.waiterDead;
                                break;
                            case Identity.Occupations.Bartender:
                                cutsceneVariation = CutsceneVariations.bartenderDead;
                                break;
                            default:
                                // As a default cutsceneVariation is already set to crewAlive, so no changes needed
                                break;
                        }

                        continue;
                    }

                    // Assign specific roles
                    switch (people[i].occupation)
                    {
                        case Identity.Occupations.Chef:
                            missingRole[0] = false;
                            personel[0].AssignLooks(people[i]);
                            continue;
                        case Identity.Occupations.Bartender:
                            missingRole[1] = false;
                            personel[1].AssignLooks(people[i]);
                            continue;
                        case Identity.Occupations.Waiter:
                            missingRole[2] = false;
                            personel[2].AssignLooks(people[i]);
                            continue;
                        case Identity.Occupations.Janitor:
                            missingRole[3] = false;
                            personel[3].AssignLooks(people[i]);
                            continue;
                        default:
                            break;
                    }

                    // Assign regular lad to special personal number if three regular spots taken
                    if (regularLads >= 3)
                    {
                        for (int j = 0; j < missingRole.Length; j++)
                        {
                            if (missingRole[j]) personel[j].AssignLooks(people[i]);
                        }
                    }

                    // Assign regular lads
                    personel[6 - regularLads].AssignLooks(people[i]);
                    regularLads++;
                }
            }
        }

        public void BeginCutscene(bool mustGetNPCs = false)
        {
            if (hasBegunCutscene) return;
            hasBegunCutscene = true;

            Cursor.visible = false;

            if (mustGetNPCs)
            {
                foreach (CutsceneNPCInfoTransferer npcWithInfo in CutsceneNPCInfoTransferer.infoTransferers)
                {
                    npcWithInfo.TransferInfo();
                }
            }

            Debug.Log($"TODO: begin playing cutscene, specifically variation {cutsceneVariation}. Currently just playing variation {CutsceneVariations.crewAlive}");
            GetComponent<PlayableDirector>().playableAsset = cutsceneTimelines[0];
            GetComponent<PlayableDirector>().Play();
        }
    }
}
