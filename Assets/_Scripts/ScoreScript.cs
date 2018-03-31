using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public static int scoreValue;
    public static int highScore_value;

    Text _score;
    Text _highScore;
    Text _nextLevel;
    Text _levelOne;
    Text _finalBoss;
    Text _YouWin;

    private bool nextLevel = true;
    private bool finalLevel = true;
    private bool newGame = true;

	// Use this for initialization
    void Start()
    {
        _YouWin = GameObject.Find("YouWinText").GetComponent<Text>();
        _finalBoss = GameObject.Find("FinalBossText").GetComponent<Text>();
        _levelOne = GameObject.Find("LevelOneText").GetComponent<Text>();
        _nextLevel = GameObject.Find("NextLevelText").GetComponent<Text>();
        _score = GameObject.Find("ScoreText").GetComponent<Text>();
        _highScore = GameObject.Find("HighScoreText").GetComponent<Text>();
        _nextLevel.enabled = false;
        _finalBoss.enabled = false;
        _YouWin.enabled = false;
        scoreValue = 0;

        newGame = true;
        _levelOne.enabled = true;
        Invoke("DisableText", 3f); //disables the text after 3 seconds

        highScore_value = PlayerPrefs.GetInt("highScore_value", highScore_value);
        _score.text = "Score: " + scoreValue;
        _highScore.text = "High Score: " + highScore_value;
        

    }
	
	// Update is called once per frame
	void Update () {
        _score.text = "Score: " + scoreValue;
        if(scoreValue > highScore_value)
        {
            highScore_value = scoreValue;
            PlayerPrefs.SetInt("HighScore", highScore_value);
            _highScore.text = "High Score: " + highScore_value;
        }
        if (scoreValue > 200 && nextLevel == true)
        {
            Main.S.nextLevel(); // calls the next level
            _nextLevel.enabled = true; //displays the level 2 text
            Invoke("DisableText", 3f); //disables the text after 3 seconds
            nextLevel = false;
        }
        if (scoreValue > 500 && finalLevel == true)
        {
            Main.S.finalBoss(); // calls the next level
            _finalBoss.enabled = true; //displays the level 2 text
            Invoke("DisableText", 3f); //disables the text after 3 seconds
            finalLevel = false;
        }
        if(scoreValue > 10000 && newGame == true)
        {
            _YouWin.enabled = true;
            Main.S.DelayedRestart(2f);
            newGame = false;
        }
    }
     void DisableText()
    {
        _nextLevel.enabled = false;
        _levelOne.enabled = false;
        _finalBoss.enabled = false;
    }

}
