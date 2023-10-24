using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePuffFX : MonoBehaviour
{

    private ParticleSystem ps;
    private Action<SmokePuffFX> _killAction;
    private float _timeToDestroy = 0;

    void Awake(){
        ps = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeToDestroy = ps.main.startLifetime.constantMax;
    }

    // Update is called once per frame
    void Update()
    {
        _timeToDestroy -= Time.deltaTime;
        if(_timeToDestroy <= 0){ _killAction(this); }
        
    }

    public void OnUseSetup(Action<SmokePuffFX> killAction){
        _killAction = killAction;
        _timeToDestroy = ps.main.startLifetime.constantMax;
    }  

    public void PlayFX(Vector3 position){
        // Reset bullet
        transform.position = position;

        ps.Play();
    }
}
