using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour {

    public int amount;

    private void Awake() {
        if(amount <= 0)
            amount = 1;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")) {
            other.GetComponent<EnemyBase>().health -= amount;
            //other.GetComponent<BehaviorBase>().currentDetection += 5f;
        }
    }
}
