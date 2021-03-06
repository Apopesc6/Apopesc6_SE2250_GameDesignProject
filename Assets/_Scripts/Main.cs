﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 1f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;

	public GameObject prefabPowerUp;

    private int level = 1;
    private BoundsCheck _bndCheck;
    private bool lastLevel = false;
    private bool gameOver = false;

    private GameObject[] enemy1;
    private GameObject[] enemy0;
    private GameObject[] enemy2;

	public void shipDestroyed( Enemy e )
	{
		// Potentially generate a PowerUp
		if (Random.value < 0.2)
		{ 
			WeaponType[] powerUpFrequency = new WeaponType[] {WeaponType.boost,WeaponType.clear,WeaponType.fastShoot};
			WeaponType puType = powerUpFrequency[Random.Range(0,powerUpFrequency.Length)];
			GameObject go = Instantiate(prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp>();
			pu.SetType(puType); // f
			switch (pu.type) {
			case WeaponType.boost:
				pu.letter.text = "B";
				break;
			case WeaponType.fastShoot:
				pu.letter.text = "F";
				break;
			case WeaponType.clear:
				pu.letter.text = "C";
				break;
			}
			pu.transform.position = e.transform.position;
		}
	}

	void Awake()
    {
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);


        //A generic Dictionary with WeaponType as the key 
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }
	
    public void SpawnEnemy()
    {
        if (level == 1)
        {
            // ndx = pick a random enemy prefab to instantiate
            int ndx = Random.Range(0, 2); //only the first two enemies are allowed to spawn
            GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

            float enemyPadding = enemyDefaultPadding;
            if (go.GetComponent<BoundsCheck>() != null)
            {
                enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }
            Vector3 pos = Vector3.zero;
            float xMin = -_bndCheck.camWidth + enemyPadding;
            float xMax = _bndCheck.camWidth - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = _bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;
        }
        if (level == 1) //spawns enemies only if it is level 1
        {
            Invoke("SpawnEnemy", 0.7f);
        }
        else if (lastLevel == false)
        {
            Invoke("SpawnWave2", 1f / enemySpawnPerSecond); //calls the spawn wave 2 function if it is level 2
        }
    }


    public void SpawnWave2() //spawns the second wave of enemies with new enemies
    {
        if (lastLevel == false)
        {
            // ndx = pick a random enemy prefab to instantiate
            int ndx = Random.Range(0, 3);
            GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

            float enemyPadding = enemyDefaultPadding;
            if (go.GetComponent<BoundsCheck>() != null)
            {
                enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }
            Vector3 pos = Vector3.zero;
            float xMin = -_bndCheck.camWidth + enemyPadding;
            float xMax = _bndCheck.camWidth - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = _bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;
        }
        if (lastLevel == false)
        {
            Invoke("SpawnWave2", 0.3f); //spawns wave 2 enemies only if its level 2
        }
        else if (lastLevel == true)
        {
            Invoke("Boss", 0.3f);
        }
    }

    public void Boss()
    {
        GameObject go = Instantiate<GameObject>(prefabEnemies[4]);
        Vector3 pos = Vector3.zero;
        pos.x = 0.3f;
        pos.y = 34f;
        go.transform.position = pos;

        Invoke("Rockets", 0.6f); //spawns the rockets
    }

    public void Rockets() //spawns the rockets coming out of the boss
    {
        // ndx = pick a random enemy prefab to instantiate
        int ndx = Random.Range(3, 4);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMin = -_bndCheck.camWidth + enemyPadding;
        float xMax = _bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight - 15f;
        go.transform.position = pos;

        if (gameOver == false)
        {
            Invoke("Rockets", 0.4f); //spawns the rockets
        }
            
       
    }

    public void nextLevel() //called from scoremanager script (advances to the next level after a certain amount of score)
    {
        level++; //increases the level variable
        enemy1 = GameObject.FindGameObjectsWithTag("enemy1");
        for (var i = 0; i < enemy1.Length; i++)  //clears all of the enemies off the screen
        {
            Destroy(enemy1[i]);
        }
        enemy0 = GameObject.FindGameObjectsWithTag("enemy0");
        for (var i = 0; i < enemy0.Length; i++)
        {
            Destroy(enemy0[i]);
        }
    }


    public void finalBoss()
    {
        lastLevel=true; //makes last level = true
        enemy1 = GameObject.FindGameObjectsWithTag("enemy1");
        for (var i = 0; i < enemy1.Length; i++)  //clears all of the enemies off the screen
        {
            Destroy(enemy1[i]);
        }
        enemy0 = GameObject.FindGameObjectsWithTag("enemy0");
        for (var i = 0; i < enemy0.Length; i++)
        {
            Destroy(enemy0[i]);
        }
        enemy2 = GameObject.FindGameObjectsWithTag("enemy3");
        for (var i = 0; i < enemy2.Length; i++)
        {
            Destroy(enemy2[i]);
        }
    }


    public void DelayedRestart(float delay)
    {
        gameOver = true;
        //invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        //Reload_Scene_0 to restart the game
        SceneManager.LoadScene("_Scene_0");
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        return (new WeaponDefinition());
    }
}
