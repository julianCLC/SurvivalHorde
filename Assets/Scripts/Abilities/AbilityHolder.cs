using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
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

    enum AbilityInput {
        shoot,
        ability1,
        ability2
    }
    
    AbilityState state = AbilityState.ready;
    [SerializeField] AbilityInput abilityInput;

    [Tooltip("Grab this from player prefab")]
    public StarterAssetsInputs _input;

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case AbilityState.ready:
                if(GetAbilityInput()){
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

    bool GetAbilityInput(){
        bool inputToReturn = false;

        switch (abilityInput) {
            case AbilityInput.shoot:
                inputToReturn = _input.shoot;
                break;
            case AbilityInput.ability1:
                inputToReturn = _input.ability1;
                break;
            case AbilityInput.ability2:
                inputToReturn = _input.jump;
                break;
        }

        return inputToReturn;
    }

    public void LevelUpAbility(){
        ability.LevelUp();
    }
}
