using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBasic : BehaviorBase {

    public Transform[] patrolPoints;
    public float waitTime, pauseTime;

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
        if(!waiting && AtDestination()) {
            StartCoroutine(StartWait());
        }
    }

    public override void OnFirstSuspicious() {
        base.OnFirstSuspicious();
        StartCoroutine(Pause());
    }

    public override void OnSuspicious() {
        base.OnSuspicious();
        Debug.Log("suspicious");
        waiting = false;
    }

    public override void OnDetectPlayer() {
        base.OnDetectPlayer();
        Debug.Log("detected");
        waiting = false;
        agent.SetDestination(PlayerController.pc.controlledPawn.transform.position);
    }

    protected void NextDestination() {
        currentPoint++;
        if(currentPoint >= patrolPoints.Length)
            currentPoint = 0;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    protected bool AtDestination() {
        return Vector3.Distance(transform.position, patrolPoints[currentPoint].position) <= 0.1f;
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

}
