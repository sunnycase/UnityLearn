using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public int scoreValue;

    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag(tag + " Own") || other.CompareTag(tag)) return;
        Destroy(other.gameObject);
        Destroy(gameObject);

        Instantiate(explosion, transform.position, transform.rotation);

        if (!CompareTag("Player"))
            _gameController.AddScore(scoreValue);
        else
            _gameController.GameOver();
    }
}
