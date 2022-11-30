using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed;

    void Update()
    {
        Vector3 positionLerp = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        positionLerp.z = transform.position.z;
        transform.position = positionLerp;
    }
}
