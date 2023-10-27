using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTracerPooler : PoolerBase<SlashTracer>
{
    [SerializeField] private SlashTracer _slashTracerPrefab;

    private void Start() {
        InitPool(_slashTracerPrefab, 50, 100);
    }

    protected override void GetSetup(SlashTracer obj){
        base.GetSetup(obj);
        obj.OnUseSetup(killAction);
    }

    private void killAction(SlashTracer _script){
        Release(_script);
    }
}
