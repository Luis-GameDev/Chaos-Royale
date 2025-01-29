using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionComponent : MonoBehaviour
{
    public int damage = 150;
    public float lifetime = 1.0f;

    private void Update() {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Debug.Log("Hit player: " + other.gameObject);
            other.gameObject.GetComponent<Character>().TakeDamage(damage);
        }
    }
}
