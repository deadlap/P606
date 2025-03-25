using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera camera;
    [SerializeField] [Range(0.001f, 1)] float followDelay;
    [SerializeField] Vector3 camOffset;
    [SerializeField] Vector3 initialRotation;
    GameObject player;
    void Awake()
    {
        camera = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        RegularCam();
    }
    
    void RegularCam()
    {
        var tf = transform;
        tf.position = Vector3.Lerp(tf.position, player.transform.position + camOffset, followDelay);
        tf.eulerAngles = initialRotation;
    }
}
