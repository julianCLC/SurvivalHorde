using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SlashTracer : MonoBehaviour
{

    [Tooltip("How fast to move the bullet")]
    public float moveSpeed = 0.2f;
    [Tooltip("How wide the tracer is")]
    public float slashWidth = 10f;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform trailSource;

    private bool _destroyFlag = false;
    private float _timeToDestroy = 0;
    private Action<SlashTracer> _killAction;

    void Update(){
        if(_destroyFlag){
            _timeToDestroy -= Time.deltaTime;
            if(_timeToDestroy <= 0){ _killAction(this); }
            
        }
    }

    public void OnUseSetup(Action<SlashTracer> killAction){
        _killAction = killAction;
        _timeToDestroy = moveSpeed + 1;
    }    

    public void PlayFX(Transform playerTransform, float swingDegree, float moveSpeed, float tracerWidth){

        transform.position = playerTransform.position;
        transform.forward = playerTransform.forward;

        trailSource.localPosition = new Vector3(0f, 0f, tracerWidth/2f + 1f);
        trailRenderer.widthMultiplier = tracerWidth;

        if(UnityEngine.Random.Range(0,2) == 1) { swingDegree *= -1;} // randomly swing from left to right or vice versa

        StartCoroutine(_Animate(playerTransform, swingDegree, moveSpeed));
        
    }

    
    IEnumerator _Animate(Transform parentTransform, float degrees, float animTime){

        float percent = 0;
        float currentTime = 0;

        Quaternion startRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - degrees, transform.eulerAngles.z);
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + degrees, transform.eulerAngles.z);

        while(percent < 1){
            transform.position = parentTransform.position;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, percent);

            currentTime += Time.deltaTime;
            percent = currentTime / animTime;
             
             yield return new WaitForEndOfFrame();
        }
        
        _destroyFlag = true;
        
    }
    
    
}
