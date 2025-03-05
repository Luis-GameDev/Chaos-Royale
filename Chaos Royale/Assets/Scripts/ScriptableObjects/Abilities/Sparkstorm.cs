using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Managing.Client;
using FishNet;
using FishNet.Connection;

[CreateAssetMenu(fileName = "New Sparkstorm", menuName = "Ability/Sparkstorm")]
public class Sparkstorm : Ability
{
    
    private Character character;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AbilityServerRPC abilityServerRPC;

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
        abilityServerRPC = FindAnyObjectByType<AbilityServerRPC>();
        character.CanMove = true;
        for (int i = 0; i < 6; i++) {
            //GameObject projectile = Instantiate(projectilePrefab, character.transform.position, Quaternion.identity);
            Vector3 direction = Quaternion.Euler(0, i * 60, 0) * Vector3.forward;
            abilityServerRPC.SpawnTransformPrefab(character, projectilePrefab, character.transform.position, Quaternion.identity, InstanceFinder.ClientManager.Connection, direction);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
