using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBasic : BehaviorBase {

    public Transform[] patrolPoints;
    public float waitTime, pauseTime, attackRange;

    private int currentPoint;
    private bool waiting;
    private NavMeshAgent agent;

    protected override void Start() {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        currentPoint = 0;
        agent.SetDestination(patrolPoints[0].position);
        waiting = false;
        if(patrolPoints.Length == 0)
            Debug.Log("No patrol points set for " + gameObject.name);
    }

    public override void OnIdle() {
        base.OnIdle();
        if(detectedState != DetectedMode.Unaware)
            agent.SetDestination(patrolPoints[currentPoint].position);
        if(!waiting && AtDestination())
            StartCoroutine(StartWait());
    }

    public override void OnFirstSuspicious() {
        base.OnFirstSuspicious();
        StartCoroutine(Pause());
    }

    public override void OnSuspicious() {
        base.OnSuspicious();
        waiting = false;
    }

    public override void OnDetectPlayer() {
        base.OnDetectPlayer();
        waiting = false;
        if(Vector3.Distance(transform.position, PlayerController.pc.controlledPawn.transform.position) <= attackRange) {
            //agent.isStopped = false;
            agent.SetDestination(transform.position);
        } else {
            //agent.isStopped = true;
            //agent.velocity = Vector3.zero;
            agent.SetDestination(PlayerController.pc.controlledPawn.transform.position);
        }
        //agent.SetDestination(PlayerController.pc.controlledPawn.transform.position);
    }

    protected void NextDestination() {
        currentPoint++;
        if(currentPoint >= patrolPoints.Length)
            currentPoint = 0;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    protected bool AtDestination() {
        bool atDest = Vector3.Distance(transform.position, patrolPoints[currentPoint].position) <= 0.2f;
        if(!atDest && Mathf.Abs(transform.position.y - patrolPoints[currentPoint].position.y) <= 1.25f) {
            Vector3 xzPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 xzDest = new Vector3(patrolPoints[currentPoint].position.x, 0, patrolPoints[currentPoint].position.z);
            if(Vector3.Distance(xzPos, xzDest) <= 0.1f)
                atDest = true;
        }
        return atDest;
    }

    protected IEnumerator StartWait() {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        if(waiting) {
            waiting = false;
            NextDestination();
        }
    }

    protected IEnumerator Pause() {
        agent.isStopped = true;
        yield return new WaitForSeconds(pauseTime);
        if(detectedState == DetectedMode.Suspicious)
            agent.SetDestination(lastKnownPlayerPos);
        agent.isStopped = false;
    }

    public override bool WithinAttackRange() {
        return Vector3.Distance(transform.position, PlayerController.pc.controlledPawn.transform.position) <= attackRange;
    }

}
