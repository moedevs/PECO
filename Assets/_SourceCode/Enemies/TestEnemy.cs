using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase {

    public float attackTimer;
    private float internalTimer = 0f;
    private DamagePlayer attackData;

    protected override void Start() {
        base.Start();
        try {
            attackData = GetComponentInChildren<DamagePlayer>();
        } catch {
            Debug.LogError("Failed to get DamagePlayer component in Attack Hitbox for enemy " + gameObject);
        }
    }

    protected override void Update() {
        base.Update();
        if(behavior.detectedState == BehaviorBase.DetectedMode.Detected && behavior.WithinAttackRange()) {
            internalTimer += Time.deltaTime;
            if(internalTimer >= attackTimer) {
                Attack();
                internalTimer = 0f;
            }
        } else
            internalTimer = 0f;
    }

    public override void Attack() {
        base.Attack();
        attackData.amount = baseDamage;
        anim.SetTrigger("Attack");
    }

}
