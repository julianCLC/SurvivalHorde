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
        Transform shootPoint = GameManager.instance._firstPersonController.shootPoint;
        float bulletDistance = _weaponRange;

        // get direction to cursor
        Vector3 cursorWorldPos = GameManager.instance.ScreenToWorldPos(GameManager.instance._firstPersonController._cursorScreenPos);
        Vector3 shootEndPoint = new Vector3(cursorWorldPos.x, shootPoint.position.y, cursorWorldPos.z);
        Vector3 hitDirection = Vector3.Normalize(shootEndPoint - shootPoint.position);

        RaycastHit raycasthit;
        if(Physics.Raycast(shootPoint.position, hitDirection, out raycasthit, _weaponRange, hitLayerMask)){
            // Transform enemyTransform = raycasthit.transform;
            var enemyScript = raycasthit.transform.GetComponent<BaseEnemyScript>();
            if(enemyScript){
                enemyScript.TakeDamage(_damage, hitDirection);
            }
            bulletDistance = Vector3.Distance(shootPoint.position, raycasthit.point);    
        }

        BulletTracerPooler Pooler = ObjectPoolManager.instance.GetPoolByType<BulletTracerPooler>(PoolType.bulletFX);
        BulletTracer bulletTracer = Pooler.Get();
        bulletTracer.PlayFX(shootPoint.forward, shootPoint.position, bulletDistance);

        SoundManager.instance.PlaySound("ShootSFX");
    }
    public override void AbilityUpgrade1(GameObject parent){}
    public override void AbilityUpgrade2(GameObject parent){}
}
