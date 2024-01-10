using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    // Handles player input and activation of ability

    public Ability ability;
    public Ability augmentAbility;
    public float holdTime = 1;
    float cooldownTime;
    float activeTime;
    

    float currentHoldTime;

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

    void Update()
    {
        Debug.Log("holdtime: " + currentHoldTime);
        switch (state) {
            case AbilityState.ready:
            // if input held, increase hold time
            // if not,
            //      if hold time is enough, augment ability
            // else 
                if(GetAbilityInput()){
                    currentHoldTime += Time.deltaTime;
                }
                else{
                    Ability abilityToActivate = currentHoldTime > holdTime ? augmentAbility : ability;

                    StartCoroutine(ActivateMulitple(abilityToActivate));

                    currentHoldTime = 0;
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

    IEnumerator ActivateMulitple(Ability _ability){

        for(int i = 0; i < _ability._targetAmt; i++){
            _ability.Activate(GameManager.instance.player);
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void LevelUpAbility(){
        ability.LevelUp();
    }
}
