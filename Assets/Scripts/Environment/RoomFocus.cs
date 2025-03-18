using UnityEngine;

namespace RoomFocusing
{
    public class RoomFocus : MonoBehaviour
    {
        [SerializeField, Tooltip("Assign the walls to go down in this room")] private WallDowner[] wallsToDown;
        [SerializeField, Tooltip("Assign the lamps to light up in this room")] private LightDamper[] lampsToBrighten;

        [Header("Temp settings, to be hidden")]
        [Range(0f, 2f)] private float lowerLength = 0.6f;

        void Awake()
        {
            if (wallsToDown.Length == 0)
            {
                Debug.LogWarning($"There are no WallDowners assigned to {name}");
            }

            foreach (WallDowner wall in wallsToDown)
            {
                wall.goDownLength = lowerLength;
            }
        }

        private void Update()
        {
            foreach (WallDowner wall in wallsToDown)
            {
                wall.goDownLength = lowerLength;
            }
            foreach (LightDamper light in lampsToBrighten)
            {
                light.dampenLength = lowerLength;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"{other.name} entered {name}");
            if (!other.CompareTag("Player")) return;
            foreach (WallDowner wall in wallsToDown)
            {
                wall.PlayerEntered();
            }
            foreach(LightDamper light in lampsToBrighten)
            {
                light.PlayerEntered();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log($"{other.name} exited {name}");
            if (!other.CompareTag("Player")) return;
            foreach (WallDowner wall in wallsToDown)
            {
                wall.PlayerExited();
            }
            foreach (LightDamper light in lampsToBrighten)
            {
                light.PlayerExited();
            }
        }
    }
}
