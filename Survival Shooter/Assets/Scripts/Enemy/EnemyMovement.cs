using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private Transform _player;
    private PlayerHealth _playerHealth;
    private EnemyHealth _enemyHealth;
    private NavMeshAgent _nav;


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_enemyHealth.currentHealth > 0 && _playerHealth.currentHealth > 0)
        {
            _nav.SetDestination(_player.position);
        }
        else
        {
            _nav.enabled = false;
        }
    }
}
