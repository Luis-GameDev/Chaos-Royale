using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    public bool isStatic = false;
    public bool isPiercing = false;
    public int damage = 100;
    public float speed = 3.0f;
    public float range = 10.0f;
    public float lifetime = 15.0f;
    public Vector3 direction = Vector3.zero;
    public Vector3 startPosition;

    private void Awake() {
        startPosition = transform.position;
    }

    private void Update() {
        // move the projectile here
        lifetime -= Time.deltaTime;
        if(lifetime <= 0) {
            Destroy(gameObject);
        }
        if (!isStatic && direction != Vector3.zero) {

            if (Vector3.Distance(startPosition, transform.position) < range) {
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Debug.Log("Hit player: " + other.gameObject);
            other.gameObject.GetComponent<Character>().TakeDamage(damage);
            if (!isPiercing) {
                Destroy(gameObject);
            }
        }
    }
}
