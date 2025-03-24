using UnityEngine;

namespace RoomFocusing
{
    public class ShadowPerson : MonoBehaviour
    {
        [SerializeField] private GameObject mainLook;
        [SerializeField] private GameObject shadowLook;
        [SerializeField] private GameObject nameTag;

        private int playersCanSee = 0;

        public void AddPlayers(int howMany = 1)
        {
            playersCanSee += howMany;
            UpdateLooks();
        }

        public void RemovePlayers(int howMany = 1)
        {
            playersCanSee -= howMany;
            UpdateLooks();
        }

        private void UpdateLooks()
        {
            mainLook.SetActive(playersCanSee > 0);
            shadowLook.SetActive(playersCanSee < 1);
            nameTag.SetActive(playersCanSee > 0);
        }
    }
}
