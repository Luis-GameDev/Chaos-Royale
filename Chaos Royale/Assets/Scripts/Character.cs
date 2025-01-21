using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public string Name { get; set; }
    public int Health { get; set; }
    public float MovementSpeed { get; set; }
    public List<Ability> Abilities { get; set; }

    public abstract void UseAbility(int index);  // Abstrakte Methode, die in den abgeleiteten Klassen überschrieben wird.
    public virtual void Move(Vector3 destination) { /* Implementiere allgemeine Bewegung */ }
}

