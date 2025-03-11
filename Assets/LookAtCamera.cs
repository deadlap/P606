using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTf;
    void Awake()
    {
        cameraTf = Camera.main.transform;
        transform.localScale = new Vector3(-1, 1, 1);
    }

    void LateUpdate()
    {
        transform.LookAt(cameraTf.position);
    }
}
