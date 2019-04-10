using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    
    public float maxHealth, health, baseDamage, viewRangeAngle, viewRangeDistance, audioListenRange;

    private GameObject viewRangeLight, player;

    private void Awake() {
        try {
            viewRangeLight = transform.GetChild(0).gameObject;
        } catch {
            Debug.LogError("Failed to get FOV Light child object of " + gameObject);
        }
    }

    private void Start() {
        if(viewRangeLight != null) {
            if(!GameManager.gm.displayEnemyFOV)
                Destroy(viewRangeLight);
            else {
                Light light = viewRangeLight.GetComponent<Light>();
                light.range = viewRangeDistance;
                light.spotAngle = viewRangeAngle;
            }
        }
        player = PlayerController.pc.controlledPawn;
    }

    private void Update() {
        // Detect if player's controlled pawn changed, and reset reference if so
        if(player != PlayerController.pc.controlledPawn)
            player = PlayerController.pc.controlledPawn;

        // Test if player is within field of view
        if(Vector3.Distance(transform.position, player.transform.position) <= viewRangeDistance && 
            Vector3.Angle(transform.forward, player.transform.position - transform.position) <= viewRangeAngle / 2) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, viewRangeDistance, LayerMask.GetMask("Terrain", "Player"))) {
                if(hit.collider.CompareTag("PlayerControllable"))
                    Debug.Log("player in field of view");
                else
                    Debug.Log("player obscured by terrain");
            }
        }
        //Debug.DrawRay(transform.position, transform.forward, Color.red, 5f);
        //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.blue, 5f);
    }

}
