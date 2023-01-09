﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject howToPanel = null;
    [SerializeField] Text highScoreText = null;

    private void Start()
    {
        highScoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }

    public void OnClickQuitButton()//close game
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OnClickHowToButton()//on main menu, activates instruction panel
    {
        howToPanel.SetActive(true);
    }

    public void OnClickCloseButton()//on main menu how to panel, hides the panel
    {
        howToPanel.SetActive(false);
    }

    public void OnClickResetScoreButton()//reset the high score back to 0
    {
        PlayerPrefs.SetInt("Highscore", 0);
        highScoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }
}
