using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public Vector3 offset = new Vector3(0, 10, -7);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        Vector3 desiredPosition = _target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(_target);
    }
}
