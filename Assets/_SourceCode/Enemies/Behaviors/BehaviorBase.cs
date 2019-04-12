using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorBase : MonoBehaviour {
    
    public enum DetectedMode {Unaware,Suspicious,Detected};
    public static float overallMaxDetection = 100;

    public float detectedLevel, suspiciousLevel, giveUpTime;
    [HideInInspector] public float currentDetection, timer;
    [HideInInspector] public DetectedMode detectedState = DetectedMode.Unaware;

    protected virtual void Update() {
        Mathf.Clamp(currentDetection, 0f, overallMaxDetection);

        if(currentDetection >= detectedLevel)
            OnDetectPlayer();
        else if(currentDetection >= suspiciousLevel)
            OnSuspicious();
        else {
            detectedState = DetectedMode.Unaware;
            timer = 0;
        }
    }

    public virtual void OnIdle() {

    }

    public virtual void OnSuspicious() {
        detectedState = DetectedMode.Suspicious;
    }

    public virtual void OnDetectPlayer() {
        detectedState = DetectedMode.Detected;
    }

}
