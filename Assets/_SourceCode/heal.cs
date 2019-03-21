using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        HealthSystem.HealDamage(1);
    }
}
