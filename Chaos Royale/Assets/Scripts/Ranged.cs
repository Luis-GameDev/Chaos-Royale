using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ranged : Character
{   

    [Header("Stats")]
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private List<Ability> _abilities;

    public void Start() {
        Agent = GetComponent<NavMeshAgent>();
        Name = _name;
        MovementSpeed = _movementSpeed;
        Health = _health;
        Abilities = _abilities;
    }

    void Update() {
        Agent.speed = MovementSpeed;
    }

    public Ranged() {
        
    }
}
