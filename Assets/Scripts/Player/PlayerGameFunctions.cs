using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all game functions in regards to the player

public class PlayerGameFunctions : MonoBehaviour
{
    // initial properties
    public int initHealth = 5;
    // public int initSpeed = 1;
    public float initHitRecoverTime = 1;

    public int _health {get; private set;}
    public int _maxHealth { get; private set;}
    // private int _speed;
    private float _hitRecoverTime = 0f;

    public bool _dead {get; private set;}

    // Start is called before the first frame update
    void Start(){
        _health = initHealth;
        _maxHealth = initHealth;
        _dead = false;
    }

    // Update is called once per frame
    void Update(){
        // Check if I'm dead
        if(_health <= 0){
            DeathSequence();
        }

        // Timers
        _hitRecoverTime -= Time.deltaTime;
    }

    public void TakeDamage(int damage){
        if(_dead) return;
        
        if(_hitRecoverTime <= 0){
            _health -= damage;
            _hitRecoverTime = initHitRecoverTime;
            Debug.Log("Player took damage");

            DamageSequence();
        }
    }

    public void DamageSequence(){
        // do damage animation etc
        SoundManager.instance.PlaySound("PlayerTakeDamageFX");
    }

    public void DeathSequence(){
        _dead = true;
        // end game / revive etc.
        // Debug.Log("Player DIED");
        GameManager.instance.PauseGame();
        // Time.timeScale = 0;

    }
}
