using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    [HideInInspector] public int health;
    public int maxHealth, baseDamage;
    public float viewRangeAngle, viewRangeDistance, audioListenRange, detectGain, detectLoss, gravity;

    protected float detectLossMultiplier;
    protected bool outOfRange;
    protected GameObject viewRangeLight, player;
    protected BehaviorBase behavior;
    protected Animator anim;

    protected virtual void Awake() {
        try {
            viewRangeLight = transform.GetChild(0).gameObject;
        } catch {
            Debug.LogError("Failed to get FOV Light child object of " + gameObject);
        }
        behavior = GetComponent<BehaviorBase>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    protected virtual void Start() {
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

    protected virtual void Update() {
        // Check health
        if(health <= 0) {
            Destroy(gameObject);
        }

        // Detect if player's controlled pawn changed, and reset reference if so
        if(player != PlayerController.pc.controlledPawn)
            player = PlayerController.pc.controlledPawn;

        // Test if player is within field of view
        if(Vector3.Distance(transform.position, player.transform.position) <= viewRangeDistance && 
            Vector3.Angle(transform.forward, player.transform.position - transform.position) <= viewRangeAngle / 2) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, viewRangeDistance, LayerMask.GetMask("Terrain", "Player"))) {
                if(hit.collider.CompareTag("PlayerControllable")) {
                    //Debug.Log("player in field of view");
                    behavior.currentDetection += detectGain * Time.deltaTime;
                    behavior.lastKnownPlayerPos = player.transform.position;
                    outOfRange = false;
                } else {
                    //Debug.Log("player obscured by terrain");
                    outOfRange = true;
                }
            }
        } else
            outOfRange = true;

        // Lose interest if player is out of range
        if(outOfRange) {
            if(behavior.timer >= behavior.giveUpTime) {
                if(behavior.timer >= 1.5 * behavior.giveUpTime)
                    detectLossMultiplier = 1.5f;
                else
                    detectLossMultiplier = 1;
                if(PlayerController.pc.isSneaking)
                    detectLossMultiplier *= 1.5f;
                detectLossMultiplier *= Time.deltaTime;
            } else
                detectLossMultiplier = 0;
            switch(behavior.detectedState) {
                /*case BehaviorBase.DetectedMode.Unaware:
                    break;*/
                case BehaviorBase.DetectedMode.Suspicious:
                    behavior.timer += Time.deltaTime;
                    behavior.currentDetection -= detectLoss * detectLossMultiplier;
                    break;
                case BehaviorBase.DetectedMode.Detected:
                    behavior.timer += Time.deltaTime * 0.75f;
                    behavior.currentDetection -= detectLoss * detectLossMultiplier * 0.5f;
                    break;
                default:
                    break;
            }
        } else
            detectLossMultiplier = 0;
        //Debug.Log("Timer: " + behavior.timer + ", Detection: " + behavior.currentDetection + ", Multiplier: " + (detectLossMultiplier));
    }

    protected void OnDestroy() {
        foreach(Transform child in transform)
            Destroy(child.gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if(other.CompareTag("ScissorAttack")) {
            Destroy(gameObject);
        }
    }

    public virtual void Attack() {

    }

}
