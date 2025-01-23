using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Lightningstrike", menuName = "Ability/Lightningstrike")]
public class Lightningstrike : Ability
{
    
    private Character character;
    [SerializeField] private GameObject projectilePrefab;

    private void Update() {
        if(CooldownLeft > 0) {
            CooldownLeft -= Time.deltaTime;
        } else {
            CooldownLeft = 0;
        }
    }
    public override void Use(GameObject caster) {
        Debug.Log("Using Lightningstrike");
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
        Execute();
    }

    private void Execute() {
        Debug.Log("Lightningstrike executed");
        character.CanMove = true;
        GameObject projectile = Instantiate(projectilePrefab, character.transform.position, Quaternion.identity);

        ProjectileComponent projectileScript = projectile.GetComponent<ProjectileComponent>();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z-coordinate is zero since directions are processed in 2D
        Vector3 direction = (mousePosition - character.transform.position).normalized;
        projectileScript.direction = direction;
    }
}
