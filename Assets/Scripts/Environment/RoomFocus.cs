using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RoomFocusing
{
    [RequireComponent(typeof(Rigidbody))]
    public class RoomFocus : MonoBehaviour
    {
        [SerializeField, Tooltip("Assign the walls to go down in this room")] private WallDowner[] wallsToDown;
        [SerializeField, Tooltip("Assign the lamps to light up in this room")] private LightDamper[] lampsToBrighten;
        [SerializeField, Tooltip("Assign the decorations to be shown in this room") ] private DecorationHider decorationsToShow;
        [SerializeField, Tooltip("Assign Water")] private WaterFocus waterFocus;
        [SerializeField] private string nameOfRoom;
        [SerializeField] bool isCabin = false;
        [SerializeField] GameObject cabin;

        private List<ShadowPerson> npcsInside = new List<ShadowPerson>();

        private int playersInside = 0;

        [Header("Temp settings, to be hidden")]
        [Range(0f, 2f)] private float lowerLength = 0.4f;

        void Start()
        {
            GetComponent<Rigidbody>().useGravity = false;
            if (wallsToDown.Length == 0)
            {
                Debug.LogWarning($"There are no WallDowners assigned to {name}");
            }

            foreach (WallDowner wall in wallsToDown)
            {
                wall.goDownLength = lowerLength;
            }
            foreach (LightDamper light in lampsToBrighten)
            {
                light.dampenLength = lowerLength;
            }
            if(decorationsToShow != null)
            {
                decorationsToShow.delay = lowerLength;
                decorationsToShow.PlayerEntered(0);
            }
            if(waterFocus != null)
            {
                waterFocus.delay = lowerLength;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                if (!other.TryGetComponent(out ShadowPerson thatNpc))
                {
                    Debug.LogWarning($"NPC {other.name} does not have a ShadowPerson script attached");
                }
                else
                {
                    thatNpc.AddPlayers(playersInside);
                    npcsInside.Add(thatNpc);
                }
            }

            //Debug.Log($"{other.name} entered {name}");
            if (!other.CompareTag("Player")) return;

            if(isCabin)
            {
                RoomName.Instance.AddRoomName(cabin.name);

            }
            else
            {
                RoomName.Instance.AddRoomName(nameOfRoom);
            }
            playersInside++;

            decorationsToShow?.PlayerEntered();
            waterFocus?.PlayerEntered();
            foreach (ShadowPerson npcShadow in npcsInside)
            {
                npcShadow.AddPlayers();
            }
            foreach (WallDowner wall in wallsToDown)
            {
                wall.PlayerEntered();
            }
            foreach(LightDamper light in lampsToBrighten)
            {
                light.PlayerEntered();
            }
        }

        //private void OnTriggerStay(Collider other)
        //{
        //    RoomName.Instance.DisplayRoomName(nameOfRoom);
        //}

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                if (!other.TryGetComponent(out ShadowPerson thatNpc))
                {
                    Debug.LogWarning($"NPC {other.name} does not have a ShadowPerson script attached");
                }
                else
                {
                    thatNpc.RemovePlayers(playersInside);
                    npcsInside.Remove(thatNpc);
                }
            }

            //Debug.Log($"{other.name} exited {name}");
            if (!other.CompareTag("Player")) return;

            if (isCabin)
            {
                RoomName.Instance.RemoveRoomName(cabin.name);

            }
            else
            {
                RoomName.Instance.RemoveRoomName(nameOfRoom);
            }
            playersInside--;

            foreach (ShadowPerson npcShadow in npcsInside)
            {
                npcShadow.RemovePlayers();
            }
            foreach (WallDowner wall in wallsToDown)
            {
                wall.PlayerExited();
            }
            foreach (LightDamper light in lampsToBrighten)
            {
                light.PlayerExited();
            }

            decorationsToShow?.PlayerExited();
            waterFocus?.PlayerExited();
        }
    }
}
