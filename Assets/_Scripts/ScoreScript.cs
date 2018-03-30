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

    private bool nextLevel = true;

	// Use this for initialization
    void Start()
    {
        _nextLevel = GameObject.Find("NextLevelText").GetComponent<Text>();
        _score = GameObject.Find("ScoreText").GetComponent<Text>();
        _highScore = GameObject.Find("HighScoreText").GetComponent<Text>();
        _nextLevel.enabled = false;
        scoreValue = 0;
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
            Main.S.nextLevel();
            _nextLevel.enabled = true;
            Invoke("DisableText", 5f);
            nextLevel = false;
        }
    }
     void DisableText()
    {
        _nextLevel.enabled = false;
    }
}
