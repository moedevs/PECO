using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    static int MaxHealth = 6;
    public static int Health = 0;
    public static bool fuckingDead = false;
    void Start() {
        Health = MaxHealth;
    }
    // spaghetti
    public static void TakeDamage(int amount = 1) {
        Health = Health - amount;
        updateHP();
    }
    // code
    public static void HealDamage(int amount = 1) {
        Health = Health + amount;
        updateHP();
    }
    // af
    public static void updateHP() {
        GameObject text = GameObject.Find("HPText");
        text.GetComponent<UnityEngine.UI.Text>().text = "HP: " + Health;
        if (Health > MaxHealth) {
            Health = MaxHealth;
            return;
        }
        if (Health <= 0) {
            fuckingDead = true;
            return;
        }
    }
}
