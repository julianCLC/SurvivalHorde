using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracerPooler : PoolerBase<BulletTracer>
{

    [SerializeField] private BulletTracer _bulletTracerPrefab;

    private void Start() {
        InitPool(_bulletTracerPrefab, 50, 100);
    }

    protected override void GetSetup(BulletTracer obj){
        base.GetSetup(obj);
        obj.OnUseSetup(killAction);
    }

    private void killAction(BulletTracer _script){
        Release(_script);
    }

}

