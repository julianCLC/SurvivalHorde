using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    [Tooltip("How fast to move the bullet")]
    public float moveSpeed = 0.2f;
    [Tooltip("How far bullet moves per moveSpeed")]
    public float moveSegment = 10f;
    [Tooltip("How long the bullet length is (as percent)")]
    public float tailLength = 10f;
    // [Tooltip("Width of bullet")]
    // public float width = 

    // private Transform _transform;
    private bool _destroyFlag = false;
    private float _timeToDestroy = 0;
    private Action<BulletTracer> _killAction;
    private TrailRenderer trailRenderer;

    void Awake(){
        // _transform = transform;
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Start(){
        trailRenderer.time = moveSpeed*(tailLength/100f);
    }

    void Update(){
        if(_destroyFlag){
            _timeToDestroy -= Time.deltaTime;
            // if(_timeToDestroy <= 0){ BulletTracerPooler.instance.Release(this); }
            if(_timeToDestroy <= 0){ _killAction(this); }
            
        }
    }

    public void OnUseSetup(Action<BulletTracer> killAction){
        _killAction = killAction;
        _timeToDestroy = moveSpeed + 1;
    }    

    public void PlayFX(Vector3 orientation, Vector3 startPos, float magnitude){
        // Reset bullet
        transform.position = startPos;
        transform.forward = orientation;
        trailRenderer.Clear();
        
        Vector3 endPos = startPos + (Vector3.Normalize(orientation) * magnitude);

        // Moves bullet
        StartCoroutine(_Animate(startPos, endPos));
    }

    IEnumerator _Animate(Vector3 startPos, Vector3 endPos){
        float percent = 0;
        float currentTime = 0;
        float animTime = Vector3.Magnitude(endPos - startPos) * moveSpeed/moveSegment;

        while(percent < 1){
             transform.position = Vector3.Lerp(startPos, endPos, percent);
             currentTime += Time.deltaTime;
             percent = currentTime / animTime;
             
             yield return new WaitForEndOfFrame();
        }
        
        _destroyFlag = true;
    }
}
