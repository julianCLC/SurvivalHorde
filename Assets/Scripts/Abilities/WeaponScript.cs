using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class WeaponScript : MonoBehaviour
{

    public Transform shootPoint;

    [Header("Attributes")]
    public float _shootCooldown = 1f;
    [Tooltip("Time before weapon can shoot again i.e. fire rate")]
    public bool isAutomatic = false;
    public LayerMask hitLayerMask;

    [Header("Weapon Properties")]
    public float _weaponRange = 10f;
    public int _damage = 2;

    private bool _fireHeld = false; // used for checing if weapon is automatic
    private float _timeUntillShoot = 0f; // used to count down from last shot

    [HideInInspector] public BulletTracerPooler Pooler;

    [SerializeField] private StarterAssetsInputs _input;

    // Start is called before the first frame update
    void Start(){
        Pooler = ObjectPoolManager.instance.GetPoolByType<BulletTracerPooler>(PoolType.bulletFX);
    }

    // Update is called once per frame
    void Update(){
        if(_input == null){
            Debug.Log("_input is null: assign StarterAssetsInputs to WeaponScript");
            return;
        }

       // OnShootPressed(); 
    }

    void OnShootPressed(){

        // Handles fire rate and if weapon is automatic
        if(_input.shoot){
            if(CanShoot()){
                _timeUntillShoot = _shootCooldown;
                ConfigureShootProperties();
            }
            _fireHeld = true;
        } else {
            _fireHeld = false;
        }

        // Reduce timer until time to shoot
        if(_timeUntillShoot > 0){
            _timeUntillShoot  -= Time.deltaTime;
        }
    }

    void ConfigureShootProperties(){
        // configure any modifications to the shoot action

        // Vector3 bulletEndPos = shootPoint.position + (Vector3.Normalize(shootPoint.forward) * _weaponRange);
        // Debug.DrawLine(shootPoint.position, bulletEndPos, Color.green, 2f);
        WeaponFire();
    }

    void WeaponFire(){
        // max weapon range
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

        BulletTracer bulletTracer = Pooler.Get();
        bulletTracer.PlayFX(shootPoint.forward, shootPoint.position, bulletDistance);

        SoundManager.instance.PlaySound("ShootFX");

        // Debug.DrawRay(shootPoint.position, shootEndPoint - shootPoint.position, Color.green, 5f);
    }

    /*
    void WeaponFire(Vector3 shootForward, Vector3 shootPos, Vector3 shootEnd){
        // BulletTracer bulletTracer = Pooler.Get();
        // bulletTracer.PlayFX(shootForward, shootPos, shootEnd);

        // Shoot from weapon
        RaycastHit raycastHit;
        if(Physics.Raycast(shootPos, shootEnd - shootPos, out raycastHit, Mathf.Infinity, hitLayerMask, QueryTriggerInteraction.Ignore)){
            // do if hit something check
        }
    }
    */

    bool CanShoot(){
        // if automatic, can shoot if _timeUntillShoot is 0
        // if not, can shoot if ''' ALSO if fire was released before shooting again
        return ((_timeUntillShoot <= 0) && ((!isAutomatic && !_fireHeld) || (isAutomatic && _fireHeld)));
    }
}
