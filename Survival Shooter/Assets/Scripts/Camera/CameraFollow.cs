using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        var targetCameraPos = target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPos, smoothing * Time.deltaTime);
    }
}
