using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : PoolerBase<BaseEnemyScript>
{
    
    [SerializeField] private BaseEnemyScript _enemyPrefab;

    private void Start() {
        InitPool(_enemyPrefab, 50, 100);
    }

    protected override void GetSetup(BaseEnemyScript obj){
        base.GetSetup(obj);
        obj.OnUseSetup(killAction);
    }

    private void killAction(BaseEnemyScript _script){
        Release(_script);
    }
}
