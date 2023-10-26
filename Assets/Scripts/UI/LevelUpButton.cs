using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{
    public Button button;
    public TMP_Text _name;
    public Image _img;
    private Ability _ability;
    [HideInInspector] public AbilityHolder abilityHolder;

    public void OnButtonPress(){
        AbilityUtilities.LevelUpAbility(_ability);
        UIManager.instance.ChoseLevelUp();

    }

    public void FillButton(Ability ability){
        _name.text = ability.name;
        _ability = ability;
        // TODO: fill icon and description

    }
}
