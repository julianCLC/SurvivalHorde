using System;
using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class BaseEnemyScript : MonoBehaviour
{
    // properties to spawn with
    public int initHealth = 5;
    public int initSpeed = 1;
    public int attackPower = 1;
    public int xp = 1;

    private int _health;
    private int _speed;
    private float _attackRange = 2; // for melee hit
    private float initHitRecoverTime = 0.2f;
    private float _hitRecoverTime = 0f;
    private bool dead;

    // Pooler Properties
    private Action<BaseEnemyScript> _killAction;
    
    [HideInInspector] public SmokePuffPooler Pooler;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start(){
        InitializeEnemey();

        Pooler = ObjectPoolManager.instance.GetPoolByType<SmokePuffPooler>(PoolType.smokePuffFX);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        // Check if I'm dead
        if(dead){
            // DeathSequence();
            return;
        }
        
        Move();
        Attack();

        // Timers
        _hitRecoverTime -= Time.deltaTime;

    }

    public virtual void Move(){
        if(_hitRecoverTime <= 0){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            float step = _speed * Time.deltaTime;
            Vector3 toMove = Vector3.MoveTowards(transform.position, GameManager.instance._playerPos, step);
            // transform.position = new Vector3(toMove.x, 1, toMove.z);
            rb.MovePosition(toMove);
        }
    }

    public virtual void Attack(){
        float playerDistance = Vector3.Distance(transform.position, GameManager.instance._playerPos);
        if(playerDistance < _attackRange){
            GameManager.instance._playerGameFunctions.TakeDamage(attackPower);
        }

    }

    public void TakeDamage(int damage, Vector3 hitDirection){
        _health -= damage;
        DamageSequence(damage, hitDirection);

    }

    public virtual void DamageSequence(int damage, Vector3 hitDirection){
        // do damage animation etc
        if(_hitRecoverTime  < 0){ _hitRecoverTime = 0;}
        _hitRecoverTime += initHitRecoverTime;
        rb.AddForce(hitDirection * damage, ForceMode.Impulse);

        if(_health <= 0){
            rb.AddForce(hitDirection * damage, ForceMode.Impulse);
            DeathSequence();
        }
    }

    public virtual void DeathSequence(){
        if(!dead){
            GameManager.instance.GainXP(xp);
            dead = true;
        }

        // add death animations etc
        rb.drag = 5f;
        StartCoroutine(_DeathSequence());
        
    }

    IEnumerator _DeathSequence(){
        yield return new WaitForSeconds(initHitRecoverTime * 3);
        SmokePuffFX smokeFx = Pooler.Get();
        smokeFx.PlayFX(transform.position);

        _killAction(this);
    }

    public void InitializeEnemey(){
        _health = initHealth;
        _speed = initSpeed;
        dead = false;
    }

    // Pooler Scripts
    public void OnUseSetup(Action<BaseEnemyScript> killAction){
        _killAction = killAction;
        InitializeEnemey();
    }

}
