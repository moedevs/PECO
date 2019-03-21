using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        HealthSystem.TakeDamage(1);
    }
}
