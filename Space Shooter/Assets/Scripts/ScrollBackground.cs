using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    public float scrollSpeed;
    public float tileSizeZ;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = _startPosition - transform.forward * Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
    }
}
