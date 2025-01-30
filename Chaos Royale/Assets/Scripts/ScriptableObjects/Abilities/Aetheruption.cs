using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Aetheruption", menuName = "Ability/Aetheruption")]
public class Aetheruption : Ability
{
    
    private Character character;
    [SerializeField] private GameObject explosionPrefab;

    public override void Use(GameObject caster) {
        
        character = caster.GetComponent<Character>();

        if(character.CanMove && character.globalCooldownLeft <= 0 && character.ability2Cooldown <= 0) {
            character.globalCooldownLeft = character.globalCooldown;
            character.CanMove = false;
            character.ability2Cooldown = Cooldown;
            character.StartCoroutine(waitCast());
        }
    }

    private IEnumerator waitCast() {
        yield return new WaitForSeconds(CastTime);
        Execute();
    }

    private void Execute() {
        character.CanMove = true;
        Vector3 position = character.player.GetCursorWorldPosition();
        position.y += 1.0f; 
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }
}
