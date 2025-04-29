using UnityEngine;

namespace RoomFocusing
{
    public class WallDowner : MonoBehaviour
    {
        private int playersInside = 0;
        [HideInInspector] public float goDownLength = 0.4f;

        protected bool isCopy = false;

        private float currentGoDown = 0f;

        [Tooltip("Y-value when wall is in its lowered position")] private float lowY = -3.5f;

        private float startY;

        [SerializeField] private Material shadowMaterial;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (isCopy) return; isCopy = true;

            // Create a copy of this that has a shadow
            GameObject shadowCopy = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
            shadowCopy.name = "shadowWall";
            Renderer[] shadowRenderers = shadowCopy.GetComponentsInChildren<Renderer>();
            foreach (Renderer shadowRenderer in shadowRenderers)
            {
                shadowRenderer.materials = new Material[] { shadowMaterial };
                shadowRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
            shadowCopy.GetComponent<WallDowner>().enabled = false;

            // Disable this objects shadow and colliders
            Renderer[] myRenderers = GetComponentsInChildren<Renderer>();
            foreach(Renderer myRenderer in myRenderers)
                GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            startY = transform.localPosition.y;
        }

        // Update is called once per frame
        void Update()
        {
            currentGoDown += Time.deltaTime * (playersInside > 0 ? 1 / goDownLength : 1 / -goDownLength);
            currentGoDown = Mathf.Clamp01(currentGoDown);

            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(startY, lowY, currentGoDown), transform.localPosition.z);
        }

        public virtual void PlayerEntered()
        {
            playersInside++;
            //Debug.Log($"There are {playersInside} players inside {name}");
        }
        public virtual void PlayerExited()
        {
            playersInside--;
            //Debug.Log($"There are {playersInside} players inside {name}");
        }
    }
}
