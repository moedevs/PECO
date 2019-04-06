using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    public int amount;

    private void Awake() {
        if(amount <= 0)
            amount = 1;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PlayerControllable"))
            HealthSystem.TakeDamage(amount);
    }
}
