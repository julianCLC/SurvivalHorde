using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu]
public class DashAbility : Ability
{

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

        _weaponRange *= 1.2f;
        _cooldownTime *= 0.8f;
    }

    public override void BaseAbility(GameObject parent){

        Debug.Log("DASH");

        CharacterController _controller = parent.GetComponent<CharacterController>();
        Vector3 impact = AbilityUtilities.AddImpact(_controller.velocity, _weaponRange);
        GameManager.instance._firstPersonController.ApplyImpact(impact);

        // Visual FX
        // SlashTracerPooler Pooler = ObjectPoolManager.instance.GetPoolByType<SlashTracerPooler>(PoolType.slashEffectFx);
        // SlashTracer slashTracer = Pooler.Get();
        // slashTracer.PlayFX(mesh, 80f, _activeTime, _weaponRange);

        // SoundManager.instance.PlaySound("SwordSlashSFX");
    } 
    public override void AbilityUpgrade1(GameObject parent){}
    public override void AbilityUpgrade2(GameObject parent){}
}
