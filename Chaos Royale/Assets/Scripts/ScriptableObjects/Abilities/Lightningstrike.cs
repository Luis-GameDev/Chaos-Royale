using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Lightningstrike", menuName = "Ability/Lightningstrike")]
public class Lightningstrike : Ability
{
    
    private Character character;
    [SerializeField] private GameObject projectilePrefab;

    public override void Use(GameObject caster) {
        
        character = caster.GetComponent<Character>();

        if(character.CanMove && character.globalCooldownLeft <= 0 && character.ability0Cooldown <= 0) {
            character.globalCooldownLeft = character.globalCooldown;
            character.CanMove = false;
            character.ability0Cooldown = Cooldown;
            character.StartCoroutine(waitCast());
        }
    }

    private IEnumerator waitCast() {
        yield return new WaitForSeconds(CastTime);
        Execute();
    }

    private void Execute() {
        character.CanMove = true;
        GameObject projectile = Instantiate(projectilePrefab, character.transform.position, Quaternion.identity);

        ProjectileComponent projectileScript = projectile.GetComponent<ProjectileComponent>();
        Camera mainCam = GameObject.FindGameObjectWithTag("MainCam").GetComponent<Camera>();
        
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, character.transform.position);
        if (plane.Raycast(ray, out float distance)) {
            Vector3 mousePosition = ray.GetPoint(distance);
            Vector3 direction = (mousePosition - character.transform.position).normalized;
            direction.y = 0; // Ensure no vertical movement
            projectileScript.direction = direction;
        }
    }
}
