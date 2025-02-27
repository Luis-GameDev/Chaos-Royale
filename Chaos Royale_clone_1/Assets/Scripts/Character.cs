using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Character : NetworkBehaviour {

    public string Name { get; set; }
    public GameObject winScreen;
    public GameObject loseScreen;
    public readonly SyncVar<int> Health = new SyncVar<int>();
    public int MaxHealth { get; set; }
    public float MovementSpeed { get; set; }
    public List<Ability> Abilities { get; set; }
    public List<Item> Items { get; set; }
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
        Debug.Log("Taking damage " + Health.Value);
        combatTimeLeft = combatTime;
        
        if (Health.Value - damage > 0) {
            Health.Value -= damage;
        } else {
            Health.Value = 0;

            loseScreen = GameObject.FindWithTag("PlayerUI").GetComponent<BackToLobby>().loseScreen;
            loseScreen.SetActive(true);
            Despawn(gameObject);
            ServerManager server = FindAnyObjectByType<ServerManager>();
            server.PlayerDeath(InstanceFinder.ClientManager.Connection.ClientId);
        }

        HPbar.fillAmount = (float)Health.Value / MaxHealth;
    }

    public void WinMatch()
    {
        winScreen = GameObject.FindWithTag("PlayerUI").GetComponent<BackToLobby>().winScreen;
        winScreen.SetActive(true);
    }

    public virtual void Heal(int health) {
        if (Health.Value + health < MaxHealth) {
            Health.Value += health;
        } else {
            Health.Value = MaxHealth;
        }

        if(!HPbar) return;
        HPbar.fillAmount = (float)Health.Value / MaxHealth;
    }
}

