﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] ProceduralGenerator procedGenerator = null;
    [SerializeField] Text playerScoreText = null;
    [SerializeField] Text highScoreText = null;
    [SerializeField] Text finalScoreText = null;
    [SerializeField] Text finalHighScoreText = null;
    [SerializeField] GameObject gameOverPanel = null;
    [SerializeField] GameObject pauseMenu = null;

    public int score = 0;
    private int dividerCounter = -1;

    void Start()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("Highscore").ToString();
        UpdateScore();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))//press 0 in the top number line to reset highscore
        {
            PlayerPrefs.SetInt("Highscore", 0);
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("Highscore").ToString();
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            if(transform.position.z > score)
            {
                UpdateScore();
            }
        }

        PauseGame();
    }

    void PauseGame()//press esc to pause. Press again to resume
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }
        }
    }

    public void OnClickResumeButton()//unpause game with button
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Divider")
        {
            if (dividerCounter > 0)
            {
                procedGenerator.TrackTerrain();
            }
            else
            {
                dividerCounter++;
            }
            other.enabled = false;
        }

        if(other.tag == "Death")//destroy player and end game, play a sound
        {
            GameOver();
        }

        if (other.tag == "Water")//play a sound and trigger particle effect
        {
            Invoke("GameOver", 0.5f);
        }

        if (other.tag == "Car")//kill player, play sound effect, trigger particle affect
        {
            GameOver();
        }
    }

    public void UpdateScore()//adds a point each time the player moves forwards
    {
        score = (int)transform.position.z;
        playerScoreText.text = "Score: " + score.ToString();
        if(PlayerPrefs.GetInt("Highscore") < score)//set new high score if current highscore is lower than current game score
        {
            PlayerPrefs.SetInt("Highscore", score);
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("Highscore").ToString();
        }
    }

    public void GameOver()//destroys the player, activates game over panel, display the current and high scores
    {
        Destroy(gameObject);
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Score: " + score;
        finalHighScoreText.text = "High Score: " + PlayerPrefs.GetInt("Highscore").ToString();
    }
}
