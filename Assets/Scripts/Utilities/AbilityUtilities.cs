using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtilities : MonoBehaviour
{
    // class for getting abilities, modifying abilities, etc.

    public Ability[] abilities;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Ability[] GetRandomAbilities(){
        // randomize

        return abilities;
    }

    public static void LevelUpAbility(Ability ability){
        // check if ability can be upgraded, else levelup
        ability.LevelUp();
    }

    public static Vector3 AddImpact(Vector3 direction, float force){
        // Vector3 impact = Vector3.zero;
        direction.Normalize();
        if(direction.y < 0) direction.y = -direction.y; // reflect down force on the ground
        return direction.normalized * force;

        // return impact;
    }

}
