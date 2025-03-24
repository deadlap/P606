using UnityEngine;

namespace RoomFocusing
{
    public class DecorationHider : MonoBehaviour
    {
        [HideInInspector] public float delay;
        private int playersCanSee = 0;
        public void PlayerEntered(int howMany = 1)
        {
            playersCanSee += howMany;
            UpdateLooks();
        }

        public void PlayerExited(int howMany = 1)
        {
            playersCanSee -= howMany;
            Invoke(nameof(UpdateLooks), delay);
        }

        void UpdateLooks()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(playersCanSee > 0);
            }
        }
    }
}
