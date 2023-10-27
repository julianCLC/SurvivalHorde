using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SlashAbility : Ability
{

    public float _weaponRange;

    public override void Activate(GameObject parent) {

        switch (upgradeType) {
            case Upgrade.none:
                BaseAbility(parent);
                break;
            
            case Upgrade.upgrade1:
                AbilityUpgrade1(parent);
                break;

            
            case Upgrade.upgrade2:
                AbilityUpgrade2(parent);
                break;
        }

    }

    public override void LevelUp()
    {
        base.LevelUp();


    }

    public override void BaseAbility(GameObject parent){

        Transform mesh = GameManager.instance._firstPersonController.playerMesh;

        // TODO: some sort of physics cast calculate damage

        SlashTracerPooler Pooler = ObjectPoolManager.instance.GetPoolByType<SlashTracerPooler>(PoolType.slashEffectFx);
        SlashTracer slashTracer = Pooler.Get();
        slashTracer.PlayFX(mesh, 80f, _activeTime, _weaponRange);
    }
    public override void AbilityUpgrade1(GameObject parent){}
    public override void AbilityUpgrade2(GameObject parent){}
}
