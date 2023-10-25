using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject LevelUpScreen;

    public static UIManager instance {get; private set;}

    void Start()
    {
        if(instance != null && instance != this){
            Destroy(this);
        }
        else {
            instance = this;
        }
    }

    void OnEnable(){
        GameManager.levelUpEvent += OnLevelUp;
    }

    void OnDisable(){
        GameManager.levelUpEvent -= OnLevelUp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLevelUp(){
        LevelUpScreen.SetActive(true);
    }

    public void ChoseLevelUp(){
        LevelUpScreen.SetActive(false);
        GameManager.instance.TogglePause();
    }
}
