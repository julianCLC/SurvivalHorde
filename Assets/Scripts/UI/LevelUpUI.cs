using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{

    [SerializeField] LevelUpButton[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable(){
        // GameManager.levelUpEvent += OnLevelUp;
        // Debug.Log("ENABLED");
        OnLevelUp();
    }

    void OnDisable(){
        // GameManager.levelUpEvent -= OnLevelUp;
    }

    void OnLevelUp(){
        Ability[] abilities = GameManager.instance.abilityUtilities.GetRandomAbilities();

        int i = 0;
        foreach(LevelUpButton button in buttons){
            if(i < abilities.Length){
                button.FillButton(abilities[i]);
                i++;
                
            }
        }
    }
}
