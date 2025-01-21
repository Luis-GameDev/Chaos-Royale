using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Character
{   

    [Header("Stats")]
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private List<Ability> _abilities;

    public Melee() {
        Name = _name;
        MovementSpeed = _movementSpeed;
        Health = _health;
        Abilities = _abilities;
    }

    public override void UseAbility(int index)
    {
        throw new NotImplementedException();
    }
}
