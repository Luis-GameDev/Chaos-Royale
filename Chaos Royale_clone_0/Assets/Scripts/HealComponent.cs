using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealComponent : MonoBehaviour
{
    public int healPerSecond = 100;
    public float lifetime = 5.0f;
    public List<Character> characters = new List<Character>();

    private void Start() {
        StartCoroutine(ExecuteHealEverySecond());
    }
    private void Update() {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            characters.Add(other.gameObject.GetComponent<Character>());
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") {
            characters.Remove(other.gameObject.GetComponent<Character>());
        }
    }

    private IEnumerator ExecuteHealEverySecond() {
        while(lifetime > 0) {
            foreach(Character character in characters) {
                character.Heal(healPerSecond);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
