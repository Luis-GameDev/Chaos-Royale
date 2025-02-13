using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Sparkstorm", menuName = "Ability/Sparkstorm")]
public class Sparkstorm : Ability
{
    
    private Character character;
    [SerializeField] private GameObject projectilePrefab;

    public override void Use(GameObject caster) {
        
        character = caster.GetComponent<Character>();

        if(character.CanMove && character.globalCooldownLeft <= 0 && character.ability1Cooldown <= 0) {
            character.globalCooldownLeft = character.globalCooldown;
            character.CanMove = false;
            character.ability1Cooldown = Cooldown;
            character.StartCoroutine(waitCast());
        }
    }

    private IEnumerator waitCast() {
        yield return new WaitForSeconds(CastTime);
        character.StartCoroutine(Execute());
    }

    private IEnumerator Execute() {
        character.CanMove = true;
        for (int i = 0; i < 6; i++) {
            GameObject projectile = Instantiate(projectilePrefab, character.transform.position, Quaternion.identity);
            projectile.GetComponent<ProjectileComponent>().direction = Quaternion.Euler(0, i * 60, 0) * Vector3.forward;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
