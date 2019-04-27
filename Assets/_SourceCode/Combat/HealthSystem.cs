using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    static int MaxHealth = 6;
    public static int Health = MaxHealth; // health should always be the max on start
    public static bool dead = false;
    // spaghetti code
    public static void TakeDamage(int amount = 1) {
        Health = Health - amount;
        updateHP();
    }
    public static void HealDamage(int amount = 1) {
        Health = Health + amount;
        updateHP();
    }
    public static void updateHP() {
        if (Health > MaxHealth) {
            Health = MaxHealth;
        }
        if (Health <= 0) {
            dead = true;
        }
        GameObject text = GameObject.Find("HPText");
        text.GetComponent<UnityEngine.UI.Text>().text = "HP: " + Health;
    }
}
