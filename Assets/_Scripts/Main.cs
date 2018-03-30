using System.Collections;
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

    private int level = 1;
    private BoundsCheck _bndCheck;
    private bool destroy = true;

    private GameObject[] enemy1;
    private GameObject[] enemy0;

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

        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMin = -_bndCheck.camWidth + enemyPadding;
        float xMax = _bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        if (level == 1) //spawns enemies only if it is level 1
        {
            Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
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

    public void DelayedRestart(float delay)
    {
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
