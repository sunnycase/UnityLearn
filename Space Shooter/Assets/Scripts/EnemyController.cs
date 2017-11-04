using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;

    private AudioSource _shotAudio;

    private void Start()
    {
        _shotAudio = GetComponent<AudioSource>();
        InvokeRepeating("Fire", delay, fireRate);
    }

    private void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation).tag = tag + " Own";
        _shotAudio.Play();
    }
}
