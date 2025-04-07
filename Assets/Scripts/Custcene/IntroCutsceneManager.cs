using UnityEngine;

namespace Cutscene
{
    public class IntroCutsceneManager : MonoBehaviour
    {
        [HideInInspector] public static IntroCutsceneManager instance;
        [SerializeField] private CutscenePerson[] personel;
        private int peopleFilled = 0;

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
            if (peopleFilled >= personel.Length)
            {
                Debug.LogWarning($"More than {personel.Length} NPCs inform the CutsceneManager of their generation");
                return;
            }

            Debug.Log("Manager tells a cutscene person");

            personel[peopleFilled].AssignLooks(thisNPCInfo);

            peopleFilled++;
        }
    }
}
