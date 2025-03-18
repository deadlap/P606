using UnityEngine;

namespace RoomFocusing
{
    public class WallDowner : MonoBehaviour
    {
        private int playersInside = 0;
        [HideInInspector] public float goDownLength = 0.4f;

        private float currentGoDown = 0f;

        private float startY;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startY = transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            currentGoDown += Time.deltaTime * (playersInside > 0 ? 1 / goDownLength : 1 / -goDownLength);
            currentGoDown = Mathf.Clamp01(currentGoDown);

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startY, -startY + 0.5f, currentGoDown), transform.position.z);
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
