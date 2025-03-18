using UnityEngine;


namespace RoomFocusing
{
    public class LightDamper : MonoBehaviour
    {
        [SerializeField] private bool startOn = false;

        private int playersInside = 0;
        [HideInInspector] public float dampenLength = 0.4f;

        private float currentBrightness = 1f;

        private float startBrightness;

        private Light lamp;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (!TryGetComponent(out lamp)) return;
            startBrightness = lamp.intensity;
            if (startOn) currentBrightness = 0f;
            lamp.intensity = Mathf.Lerp(startBrightness, startBrightness / 5, currentBrightness);
        }

        // Update is called once per frame
        void Update()
        {
            currentBrightness += Time.deltaTime * 1 / (playersInside > 0 ?  -dampenLength : dampenLength);
            currentBrightness = Mathf.Clamp01(currentBrightness);

            lamp.intensity = Mathf.Lerp(startBrightness, 0, currentBrightness);
        }

        public void PlayerEntered()
        {
            playersInside++;
            //Debug.Log($"There are {playersInside} players inside {name}");
        }
        public void PlayerExited()
        {
            playersInside--;
            //Debug.Log($"There are {playersInside} players inside {name}");
        }
    }
}
