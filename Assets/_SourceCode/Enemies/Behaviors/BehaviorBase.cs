using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorBase : MonoBehaviour {
    
    public enum DetectedMode {Unaware,Suspicious,Detected};
    public static float overallMaxDetection = 100;

    public float detectedLevel, suspiciousLevel, giveUpTime;
    [HideInInspector] public float currentDetection, timer;
    [HideInInspector] public DetectedMode detectedState = DetectedMode.Unaware;
    [HideInInspector] public Vector3 lastKnownPlayerPos = Vector3.zero;

    protected bool susFlag = false;

    protected virtual void Update() {
        Mathf.Clamp(currentDetection, 0f, overallMaxDetection);

        if(currentDetection >= detectedLevel)
            OnDetectPlayer();
        else if(currentDetection >= suspiciousLevel)
            OnSuspicious();
        else {
            OnIdle();
            detectedState = DetectedMode.Unaware;
            timer = 0;
        }
    }

    public virtual void OnIdle() {
        susFlag = false;
    }

    public virtual void OnFirstSuspicious() {
        susFlag = true;
    }

    public virtual void OnSuspicious() {
        detectedState = DetectedMode.Suspicious;
        if(!susFlag)
            OnFirstSuspicious();
    }

    public virtual void OnDetectPlayer() {
        detectedState = DetectedMode.Detected;
        susFlag = false;
    }

}
