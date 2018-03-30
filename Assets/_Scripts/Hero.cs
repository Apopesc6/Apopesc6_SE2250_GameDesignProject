using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    static public Hero S;

    [Header("Set in Inspector")]
    public float shipSpeed = 100f;
    public float rollMult = -45f;
    public float pitchMult = 30f;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Set Dynamically")]
    [SerializeField]
    private  float _shieldLevel = 1;

    //This variable holds a reference to the last triggering GameObject
    private GameObject _lastTriggerGo = null;

    //Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Create a WeaponFireDelegate field named fireDelegate
    public WeaponFireDelegate fireDelegate;

    void Awake()
    {
        if(S == null)
        {
            S = this;
        }
        //fireDelegate += TempFire;
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }

    // Update is called once per frame
    void Update() {

        float xAxis = Input.GetAxis("horizontal");
        float yAxis = Input.GetAxis("vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * shipSpeed * Time.deltaTime;
        pos.y += yAxis * shipSpeed * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //if(Input.GetKeyDown(KeyCode.Space))
        //{
          //  TempFire();
        //}
        //use the fireDelegate to fire Weapons
        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    void OnTriggerEnter (Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Trigger: " + go.name);

        //make sure if's not the same triggering go as last time
        if(go == _lastTriggerGo)
        {
            return;
        }
        _lastTriggerGo = go;

        if(go.tag == "enemy0" || go.tag == "enemy1")
        {
            _shieldLevel--;
            Destroy(go);
        }
        else
        {
            print("Trigger by non-Enemy: " + go.name);
        }
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            //if the shield is going t be set to less than zero
            if(value < 0)
            {
                Destroy (this.gameObject);
                //Tell Main.S to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
