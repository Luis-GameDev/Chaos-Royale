using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    public bool isStatic = false;
    public bool isPiercing = true;
    public int damage = 100;
    public float speed = 3.0f;
    public float range = 10.0f;
    public float lifetime = 15.0f;
    public Vector2 direction = Vector2.zero;
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
        if (!isStatic && direction != Vector2.zero) {
            transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
            if (Vector3.Distance(startPosition, transform.position) < range) {
                transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Character>().TakeDamage(damage);
            if (!isPiercing) {
                Destroy(gameObject);
            }
        }
    }
}
