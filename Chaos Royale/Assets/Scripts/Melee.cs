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
    [SerializeField] private float _combatTime = 10.0f;

    public void Awake() {
        Agent = GetComponent<NavMeshAgent>();
        Name = _name;
        MovementSpeed = _movementSpeed;
        Health = _health;
        MaxHealth = _health;
        Abilities = _abilities;
        CanMove = true;
        HPbar = _hpBar;
        player = GetComponent<Player>();
        combatTime = _combatTime;
    }

    void FixedUpdate() {
        if (combatTimeLeft > 0) {
            combatTimeLeft -= Time.deltaTime;
        } else {
            combatTimeLeft = 0;
            Heal(1);
        }
    }
    void Update() {

        Agent.speed = MovementSpeed;
        if(globalCooldownLeft > 0) {
            globalCooldownLeft -= Time.deltaTime;
        } else {
            globalCooldownLeft = 0;
        }

        if (ability0Cooldown > 0) {
            ability0Cooldown -= Time.deltaTime;
        } else {
            ability0Cooldown = 0;
        }

        if (ability1Cooldown > 0) {
            ability1Cooldown -= Time.deltaTime;
        } else {
            ability1Cooldown = 0;
        }

        if (ability2Cooldown > 0) {
            ability2Cooldown -= Time.deltaTime;
        } else {
            ability2Cooldown = 0;
        }

        if (ability3Cooldown > 0) {
            ability3Cooldown -= Time.deltaTime;
        } else {
            ability3Cooldown = 0;
        }

        //Debug.Log(globalCooldownLeft);
    }

    public Melee() {
        
    }
}
