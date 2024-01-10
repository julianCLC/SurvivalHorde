using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShootAbility : Ability
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

        _cooldownTime *= 0.7f; 

    }

    public override void BaseAbility(GameObject parent){
        // Detect Nearest Enemy
        float minDistSoFar = Mathf.Infinity;
        Collider enemyToTarget = null;
        Collider[] colliders = Physics.OverlapSphere(parent.transform.position, GameManager.SPAWN_RADIUS, hitLayerMask, QueryTriggerInteraction.Ignore);
        if(colliders.Length > 0){
            foreach(Collider collider in colliders){
                float dist = Vector3.Distance(collider.transform.position, parent.transform.position);

                if(dist < minDistSoFar){
                    enemyToTarget = collider;
                    minDistSoFar = dist;    
                }
            }
        }
        
        // Shoot Enemy
        Transform shootPoint = GameManager.instance._firstPersonController.shootPoint;
        Vector3 hitDirection = enemyToTarget != null ? Vector3.Normalize(enemyToTarget.transform.position - shootPoint.position) : shootPoint.forward;
        float bulletDistance = GameManager.SPAWN_RADIUS;

        RaycastHit raycasthit;
        if(Physics.Raycast(shootPoint.position, hitDirection, out raycasthit, bulletDistance, hitLayerMask)){
            var enemyScript = raycasthit.transform.GetComponent<BaseEnemyScript>();

            if(enemyScript){
                enemyScript.TakeDamage(_damage, hitDirection);
            }

            bulletDistance = Vector3.Distance(shootPoint.position, raycasthit.point);    
        }

        // Particle FX
        BulletTracerPooler Pooler = ObjectPoolManager.instance.GetPoolByType<BulletTracerPooler>(PoolType.bulletFX);
        BulletTracer bulletTracer = Pooler.Get();
        bulletTracer.PlayFX(hitDirection, shootPoint.position, bulletDistance);

        // Sound FX
        SoundManager.instance.PlaySound("ShootSFX");
    }

    public override void AbilityUpgrade1(GameObject parent){}
    public override void AbilityUpgrade2(GameObject parent){}
}
