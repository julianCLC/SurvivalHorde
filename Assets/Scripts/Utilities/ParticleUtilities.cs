using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleUtilities : MonoBehaviour
{
    public static void PlayFXAtPosition(Vector3 pos, PoolType poolType){
        GenericFXPooler Pooler = ObjectPoolManager.instance.GetPoolByType<GenericFXPooler>(poolType);
        GenericStaticFX particleFX = Pooler.Get();
        particleFX.PlayFX(pos);

    }
}
