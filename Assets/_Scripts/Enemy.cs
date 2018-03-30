using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Set in Inspector: Enemy")]

    public float speed = 10f;
    public float xSpeed = 10f;
    public float health = 5;
    public int score = 100;

    Vector3 tempPos;
    Vector3 tempPos1;
    private float _randomNumber;
    private bool _runOnce = true;
    
    // new variables to make the enemy blink red when shot
    public float showDamageDur = 0.1f;
    public float dmgDone;
    public bool showDmg = false;
    public Material[] materials;
    public Color[] colors;

    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
        //code using utils to get the materials
        materials = Utils.getAllMaterials(gameObject);
        colors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            colors[i] = materials[i].color;
        }

    }

	// Use this for initialization
	void Start () {

        _randomNumber = Random.Range(1,2);
        for (int i = 0; i < _randomNumber; i++)
        {
            xSpeed = xSpeed * -1;
        }
        

	}
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (showDmg && Time.time > dmgDone)
        {
            unShowDamage();
        }

        if(_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }
        if (_bndCheck != null && (_bndCheck.offRight || _bndCheck.offLeft))
        {
            if(_runOnce)
            {
                ChangeXDirection();
                _runOnce = false;
            }
        }
    }

    void ChangeXDirection()
    {
        xSpeed = xSpeed * -1;
    }

    public virtual void Move()
    {
        if(gameObject.CompareTag("enemy0"))
        {
            tempPos = pos;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        if (gameObject.CompareTag("enemy1"))
        {
            tempPos1 = pos;
            tempPos1.y -= speed * Time.deltaTime;
            tempPos1.x -= xSpeed * Time.deltaTime;
            pos = tempPos1;
        }

    }
    void OnCollisionEnter( Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                if(!_bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }

                showDamage();
                //hurt this enemy
                //Get the damage amount from the Main WEAP_DICT.
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0)
                {
                    //Desotry this Enemy
                    Destroy(this.gameObject);
                    ScoreScript.scoreValue += score;
                }
                Destroy(otherGO);
                break;

            default:
                print("Eney hit by non-projectileHero: " + otherGO.name);
                break;
        }   
    }
    void showDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showDmg = true;
        dmgDone = Time.time + showDamageDur;
    }
    void unShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = colors[i];
        }
        showDmg = false;
    }

}
