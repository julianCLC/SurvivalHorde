using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.Rendering;
using UnityEngine;

// FOR TESTING
public class PlayerAnimations : MonoBehaviour
{
    public StarterAssetsInputs _inputs;

    // public float Speed;
    public float Amplitude;
    public float PeriodTime;
    // public float Periods;
    public float StartRange;
    public float EndRange;
    // [Range(0,360)]
    // public float pos;

    // private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // SinAnimation(Speed, Amplitude);
        if(_inputs.jump) { StartCoroutine(SinAnimate());}
    }

    /*
    void SinAnimation(float speed, float amplitude){

        timer += Time.deltaTime;
        float posDeg = pos * Mathf.Deg2Rad;
        // float y = Mathf.Sin(posDeg * speed) * amplitude;
        // float y = Mathf.Cos(posDeg * speed) * amplitude;
        float y = Mathf.Sin(timer * speed) * amplitude;
        Vector3 newPos = new Vector3(0,  y, 0);

        transform.rotation = Quaternion.Euler(newPos);

        Debug.Log(timer);

        timer %= 360;
    }
    */

    IEnumerator SinAnimate(){

        float _timer = 0;
        float animTime = PeriodTime; // seconds


        while(_timer < animTime){

            float sinResult = Mathf.Sin(_timer * (2 * Mathf.PI) / PeriodTime) * Amplitude;

            Vector3 newPos = new Vector3(0, sinResult, 0);
            transform.rotation = Quaternion.Euler(newPos);


            _timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    public void SwingAnimation(Transform playerTransform, float swingSpeed, bool swingOpposite){



    }

    IEnumerator _SwingAnimation(Transform playerTransform, float animTime, bool inverseRotation){

        float timer = 0.25f * animTime;

        while(timer < animTime) {


            timer += Time.deltaTime;
        }

        yield return null;
    }
}
