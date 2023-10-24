using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float activeTime;
    [SerializeField] private int damage;
    [SerializeField] private int level;

    [HideInInspector] public float _cooldownTime;
    [HideInInspector] public float _activeTime;
    [HideInInspector] public int _damage;
    [HideInInspector] public int _level;

    void OnEnable(){
        _cooldownTime = cooldownTime;
        _activeTime = activeTime;
        _damage = damage;
        _level = level;

    }
    
    public virtual void Activate(GameObject parent) {}
    public virtual void LevelUp(){ level++; }
}
