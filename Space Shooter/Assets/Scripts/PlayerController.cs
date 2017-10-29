using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private Rigidbody _rigidbody;
    private float nextFire;
    private AudioSource _shotAudio;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _shotAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            _shotAudio.Play();
        }
    }

    private void FixedUpdate()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        _rigidbody.velocity = new Vector3(moveHorizontal, 0, moveVertical) * speed;
        _rigidbody.position = new Vector3(
            Mathf.Clamp(_rigidbody.position.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(_rigidbody.position.z, boundary.zMin, boundary.zMax));
        _rigidbody.rotation = Quaternion.Euler(0, 0, _rigidbody.velocity.x * tilt);
    }
}
