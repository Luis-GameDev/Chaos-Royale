using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Lightningstrike", menuName = "Ability/Lightningstrike")]
public class Lightningstrike : Ability
{
    
    private Character character;

    private void Update() {
        if(CooldownLeft > 0) {
            CooldownLeft -= Time.deltaTime;
        } else {
            CooldownLeft = 0;
        }
    }
    public override void Use(GameObject caster) {
        character = caster.GetComponent<Character>();
        if(character.CanMove && character.globalCooldownLeft <= 0 && CooldownLeft <= 0) {
            character.globalCooldownLeft = character.globalCooldown;
            character.CanMove = false;
            CooldownLeft = Cooldown;
            caster.GetComponent<MonoBehaviour>().StartCoroutine(waitCast());
        }
    }

    private IEnumerator waitCast() {
        yield return new WaitForSeconds(CastTime);
    }

    private void Execute() {
        character.CanMove = true;
    }
}
