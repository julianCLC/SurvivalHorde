using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public LayerMask hitLayerMask;

    // ~~ Init ability properties~~
    // ~ timers
    [SerializeField] private float cooldownTime;
    [SerializeField] private float activeTime;
    [SerializeField] private float holdTime; // how long to hold before ability is augmented

    [SerializeField] private float weaponRange;
    [SerializeField] private int damage;
    [SerializeField] private int targetAmt = 1;
    [SerializeField] private int level;

    // ~~ Properties using during game
    [HideInInspector] public float _cooldownTime;
    [HideInInspector] public float _activeTime;
    [HideInInspector] public float _weaponRange;
    [HideInInspector] public float _holdTime;
    [HideInInspector] public int _damage;
    [HideInInspector] public int _targetAmt;
    [HideInInspector] public int _level;
    
    
    [HideInInspector] public Upgrade upgradeType;

    public enum Upgrade{
        none,
        upgrade1,
        upgrade2
    }

    void OnEnable(){
        _cooldownTime = cooldownTime;
        _activeTime = activeTime;
        _holdTime = holdTime;
        
        _weaponRange = weaponRange;
        _damage = damage;
        _targetAmt = targetAmt;
        _level = level;
        

        upgradeType = Upgrade.none;

    }
    
    public virtual void Activate(GameObject parent) {}
    public virtual void LevelUp(){ level++; }
    public virtual void BaseAbility(GameObject parent){}
    public virtual void AbilityUpgrade1(GameObject parent){}
    public virtual void AbilityUpgrade2(GameObject parent){}
}
