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
    public float globalCooldownLeft { get; set; }
    public Image HPbar { get; set; }
    public float combatTime { get; set; }
    public int teamNumber = 0;
    public float ability0Cooldown = 0f;
    public float ability1Cooldown = 0f;
    public float ability2Cooldown = 0f;
    public float ability3Cooldown = 0f;
    public float globalCooldown = 5.0f; 
    public float combatTimeLeft = 0f;
    
    public Player player;

    public void UseAbility(int index) {
        Abilities[index].Use(this.gameObject);
    }  
    public virtual void Move(Vector3 destination) { 
        if (Agent != null && CanMove) {
            Agent.SetDestination(destination);
        } 
    }

    public virtual void TakeDamage(int damage) {
        combatTimeLeft = combatTime;
        
        if (Health - damage > 0) {
            Health -= damage;
        } else {
            Health = 0;
            Destroy(gameObject);
        }

        HPbar.fillAmount = (float)Health / MaxHealth;
    }

    public virtual void Heal(int health) {
        if (Health + health < MaxHealth) {
            Health += health;
        } else {
            Health = MaxHealth;
        }

        HPbar.fillAmount = (float)Health / MaxHealth;
    }
}

