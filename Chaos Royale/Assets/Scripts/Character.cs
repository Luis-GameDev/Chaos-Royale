using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour {

    public string Name { get; set; }
    public int Health { get; set; }
    public float MovementSpeed { get; set; }
    public List<Ability> Abilities { get; set; }
    public NavMeshAgent Agent { get; set; }

    public void UseAbility(int index) {
        Abilities[index].Use();
    }  
    public virtual void Move(Vector3 destination) { 
        if (Agent != null) {
            Agent.SetDestination(destination);
        } else {
            Debug.LogWarning("NavMeshAgent is not assigned.");
        }
    }
}

