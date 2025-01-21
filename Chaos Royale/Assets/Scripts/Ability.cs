using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public string Name { get; set; }
    public string Cooldown { get; set; }
    public abstract void Use();
}
