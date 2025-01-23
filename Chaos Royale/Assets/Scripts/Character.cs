using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour {

    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public float MovementSpeed { get; set; }
    public List<Ability> Abilities { get; set; }
    public NavMeshAgent Agent { get; set; }
    public bool CanMove { get; set; }
    public float globalCooldown = 5.0f; 
    public float globalCooldownLeft { get; set; }
    public Image HPbar { get; set; }

    public void UseAbility(int index) {
        Debug.Log("Using ability " + index);
        Abilities[index].Use(this.gameObject);
    }  
    public virtual void Move(Vector3 destination) { 
        if (Agent != null && CanMove) {
            Agent.SetDestination(destination);
        } 
    }

    public void TakeDamage(int damage) {
        
        if (Health - damage > 0) {
            Health -= damage;
        } else {
            Health = 0;
            // trigger death here
        }

        HPbar.fillAmount = (float)Health / MaxHealth;
    }
}

