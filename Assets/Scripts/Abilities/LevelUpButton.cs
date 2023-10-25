using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{

    public Button button;
    [HideInInspector] public AbilityHolder abilityHolder;

    public void OnButtonPress(){
        // GameManager.instance.TogglePause();
        UIManager.instance.ChoseLevelUp();

    }
}
