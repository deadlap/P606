using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTf;
    float offset = 0.3f;
    void Awake()
    {
        cameraTf = Camera.main.transform;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z + offset); //quick fix for avoiding hats clipping the name.
    }

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, cameraTf.position.y, transform.position.z);
        transform.rotation = Quaternion.Euler(transform.rotation.x, Vector3.RotateTowards(transform.position, targetPosition, 20, 999).y, transform.rotation.z);
    }
}
