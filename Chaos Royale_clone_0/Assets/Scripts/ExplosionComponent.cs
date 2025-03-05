using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionComponent : MonoBehaviour
{
    public int damage = 150;
    public Character owner;
    public float lifetime = 1.0f;

    private void Update() {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<Character>() != owner) {
            Character character = other.gameObject.GetComponent<Character>();
            ServerManager serverManager = FindObjectOfType<ServerManager>();
            serverManager.DamagePlayer(damage, character);
        }
    }
}
