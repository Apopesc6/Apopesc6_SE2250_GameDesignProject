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

    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
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
}
