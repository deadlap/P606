using System.Collections;
using UnityEngine;

namespace RoomFocusing
{
    public class WaterFocus : MonoBehaviour
    {
        private int playersCanSee = 0;
        [HideInInspector] public float delay;
        Material material;
        [SerializeField] Material blueWater;
        [SerializeField] Material greyWater;
        float time;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
        }

        public void PlayerEntered(int howMany = 1)
        {
            playersCanSee += howMany;
            StopCoroutine(nameof(UpdateLooks));
            StartCoroutine(nameof(UpdateLooks));
        }

        public void PlayerExited(int howMany = 1)
        {
            playersCanSee -= howMany; 
            StopCoroutine(nameof(UpdateLooks));
            StartCoroutine(nameof(UpdateLooks));
        }
        
        IEnumerator UpdateLooks()
        {
            float targetTime = 0;
            if (playersCanSee > 0)
            {
                targetTime = 1;
            }
            while (time != targetTime)
            {
                time += Time.deltaTime / delay * (playersCanSee > 0 ? 1 : -1);
                time = Mathf.Clamp01(time);
                material.Lerp(greyWater, blueWater, time);
                yield return null;
            }
            material.Lerp(greyWater, blueWater, targetTime);
        }
    }
}
