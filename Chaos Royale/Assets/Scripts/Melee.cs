using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Melee : Character
{   

    [Header("Stats")]
    [SerializeField] private string _name = "Melee";
    [SerializeField] private int _health = 700;
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private List<Ability> _abilities;
    [SerializeField] private Image _hpBar;


    public void Awake() {
        Agent = GetComponent<NavMeshAgent>();
        Name = _name;
        MovementSpeed = _movementSpeed;
        Health = _health;
        MaxHealth = _health;
        Abilities = _abilities;
        CanMove = true;
        HPbar = _hpBar;
    }

    void Update() {
        Agent.speed = MovementSpeed;
        if(globalCooldownLeft > 0) {
            globalCooldownLeft -= Time.deltaTime;
        } else {
            globalCooldownLeft = 0;
        }

        //Debug.Log(globalCooldownLeft);


        if(Input.GetKeyDown(KeyCode.Q) && globalCooldownLeft <= 0) {
            UseAbility(0);
        }
        if(Input.GetKeyDown(KeyCode.W) && globalCooldownLeft <= 0) {
            UseAbility(1);
        }
        if(Input.GetKeyDown(KeyCode.E) && globalCooldownLeft <= 0) {
            UseAbility(2);
        }
        if(Input.GetKeyDown(KeyCode.R) && globalCooldownLeft <= 0) {
            UseAbility(3);
        }
    }

    public Melee() {
        
    }
}
