using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePuffPooler : PoolerBase<SmokePuffFX>
{

    [SerializeField] private SmokePuffFX _smokePuffPrefab;

    private void Start() {
        InitPool(_smokePuffPrefab, 50, 100);
    }

    protected override void GetSetup(SmokePuffFX obj){
        base.GetSetup(obj);
        obj.OnUseSetup(killAction);
    }

    private void killAction(SmokePuffFX _script){
        Release(_script);
    }

}

