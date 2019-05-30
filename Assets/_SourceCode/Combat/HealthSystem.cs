using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static int maxHealth = 8;
    public static int health = maxHealth; // health should always be the max on start
    public static bool dead = false;

    // spaghetti code
    public static void TakeDamage(int amount = 1) {
        health = health - amount;
        UpdateHP();
    }

    public static void HealDamage(int amount = 1) {
        health = health + amount;
        UpdateHP();
    }

    public static void UpdateHP() {
        if(health > maxHealth) {
            health = maxHealth;
        } else if(health <= 0) {
            //dead = true;
            health = maxHealth;
            LevelData.data.RespawnPlayer();
        }
        GameObject text = GameObject.Find("HPText");
        text.GetComponent<UnityEngine.UI.Text>().text = "HP: " + health;
        UpdateHealthUI();
    }

    private static void UpdateHealthUI() {
        List<Image> healthSprites = CanvasManager.cm.healthSprites;
        if(health < 0) {
            for(int i = 0; i < healthSprites.Count; i++) {
                healthSprites[i].sprite = CanvasManager.cm.emptyHeart;
                healthSprites[i].color = new Color32(255, 255, 255, 60);
            }
        } else if(health > healthSprites.Count) {
            for(int i = 0; i < healthSprites.Count; i++) {
                healthSprites[i].sprite = CanvasManager.cm.fullHeart;
                healthSprites[i].color = new Color32(255, 255, 255, 255);
            }
        } else {
            for(int i = 0; i < healthSprites.Count; i++) {
                if(i < health) {
                    healthSprites[i].sprite = CanvasManager.cm.fullHeart;
                    healthSprites[i].color = new Color32(255, 255, 255, 255);
                } else {
                    healthSprites[i].sprite = CanvasManager.cm.emptyHeart;
                    healthSprites[i].color = new Color32(255, 255, 255, 60);
                }
            }
        } 
    }
}
