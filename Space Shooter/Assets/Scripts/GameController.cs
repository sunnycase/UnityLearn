using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private int _score;
    private bool _restart;
    private bool _gameOver;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
        _score = 0;
        _restart = false;
        _gameOver = false;
        restartText.text = string.Empty;
        gameOverText.text = string.Empty;
        UpdateScore();
    }

    private void Update()
    {
        if(_restart)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while(true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                var spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if(_gameOver)
            {
                restartText.text = "Press 'R' to restart";
                _restart = true;
                break;
            }
        }
    }

    public void AddScore(int score)
    {
        _score += score;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + _score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        _gameOver = true;
    }
}
