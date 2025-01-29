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
    [SerializeField] private Player _player;


    public void Awake() {
        Agent = GetComponent<NavMeshAgent>();
        Name = _name;
        MovementSpeed = _movementSpeed;
        Health = _health;
        MaxHealth = _health;
        Abilities = _abilities;
        CanMove = true;
        HPbar = _hpBar;
        player = _player;
    }

    void Update() {
        Agent.speed = MovementSpeed;
        if(globalCooldownLeft > 0) {
            globalCooldownLeft -= Time.deltaTime;
        } else {
            globalCooldownLeft = 0;
        }

        ability0Cooldown -= Time.deltaTime;
        ability1Cooldown -= Time.deltaTime;
        ability2Cooldown -= Time.deltaTime;
        ability3Cooldown -= Time.deltaTime;

        //Debug.Log(globalCooldownLeft);
    }

    public Melee() {
        
    }
}
