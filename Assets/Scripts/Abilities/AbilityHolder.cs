using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{

    public Ability ability;
    float cooldownTime;
    float activeTime;

    enum AbilityState {
        ready,
        active,
        cooldown
    }
    
    AbilityState state = AbilityState.ready;

    [Tooltip("Grab this from player prefab")]
    public StarterAssetsInputs _input;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            
            case AbilityState.ready:
                if(_input.shoot){
                    ability.Activate(GameManager.instance.player);
                    state = AbilityState.active;
                    activeTime = ability._activeTime;
                }
                break;
            case AbilityState.active:
                if(activeTime > 0){ activeTime -= Time.deltaTime; }
                else{ 
                    state = AbilityState.cooldown;
                    cooldownTime = ability._cooldownTime;
                }
                break;
            case AbilityState.cooldown:
                if(cooldownTime > 0){ cooldownTime -= Time.deltaTime; }
                else{ state = AbilityState.ready; }
            break;
        }
    }

    public void LevelUpAbility(){
        ability.LevelUp();
    }
}
