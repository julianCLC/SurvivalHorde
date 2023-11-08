using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class SlashAbility : Ability
{
    // consider making new variable for "slash speed" instead of using _activeTime
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

        Transform mesh = GameManager.instance._firstPersonController.playerMesh;

        // TODO: tweak hitbox to match visual better
        float halfWidth = _weaponRange/2f;
        Vector3 centre = mesh.TransformPoint(new Vector3(mesh.localPosition.x - halfWidth, mesh.localPosition.y, mesh.localPosition.z + (halfWidth/2f + 1)));
        Vector3 width = new Vector3(halfWidth, halfWidth, halfWidth);
        
        RaycastHit[] hits = Physics.BoxCastAll(centre, width, mesh.right, Quaternion.identity, _weaponRange, hitLayerMask, QueryTriggerInteraction.Ignore);
        foreach(RaycastHit hit in hits){
            BaseEnemyScript enemy = hit.transform.GetComponent<BaseEnemyScript>();
            if( enemy != null){
                enemy.TakeDamage(_damage, hit.transform.position - mesh.position);
                ParticleUtilities.PlayFXAtPosition(hit.transform.position, PoolType.hitFX);

            }
        }

        // Visual FX
        SlashTracerPooler Pooler = ObjectPoolManager.instance.GetPoolByType<SlashTracerPooler>(PoolType.slashEffectFx);
        SlashTracer slashTracer = Pooler.Get();
        slashTracer.PlayFX(mesh, 80f, _activeTime, _weaponRange);

        SoundManager.instance.PlaySound("SwordSlashSFX");
    } 
    public override void AbilityUpgrade1(GameObject parent){}
    public override void AbilityUpgrade2(GameObject parent){}

    
}
