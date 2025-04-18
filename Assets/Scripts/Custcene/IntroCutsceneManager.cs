using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Cutscene
{
    public class IntroCutsceneManager : MonoBehaviour
    {
        [HideInInspector] public static IntroCutsceneManager instance;
        [SerializeField] private CutscenePerson[] personel;

        private List<CutsceneNPCInfo> people = new List<CutsceneNPCInfo>();

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("There already exists an instance of the IntroCutsceneManager?");
            }
            instance = this;
        }
        
        public void InformOfNPC(CutsceneNPCInfo thisNPCInfo)
        {
            if (people.Count >= personel.Length)
            {
                Debug.LogWarning($"More than {personel.Length} NPCs inform the CutsceneManager of their generation");
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
    }
}
