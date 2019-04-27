using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BehaviorBase : MonoBehaviour {
    
    public enum DetectedMode {Unaware,Suspicious,Detected};
    public static float overallMaxDetection = 100;

    public float detectedLevel, suspiciousLevel, giveUpTime;
    [HideInInspector] public float currentDetection, timer;
    [HideInInspector] public DetectedMode detectedState = DetectedMode.Unaware;
    [HideInInspector] public Vector3 lastKnownPlayerPos = Vector3.zero;

    protected bool susFlag = false, detFlag = false;
    private GameObject text;

    protected virtual void Start() {
        text = transform.GetComponentInChildren<RectTransform>().gameObject;
        text.SetActive(false);
    }

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

    /// <summary>
    /// Continuously called when the enemy is unaware of the player
    /// </summary>
    public virtual void OnIdle() {
        susFlag = false;
        detFlag = false;
    }


    /// <summary>
    /// Called once when the enemy first becomes suspicous of the player
    /// </summary>
    public virtual void OnFirstSuspicious() {
        susFlag = true;
        StartCoroutine(DisplayText("?", 1.5f));
    }

    /// <summary>
    /// Continuously called while the enemy is suspicous of the player
    /// </summary>
    public virtual void OnSuspicious() {
        detectedState = DetectedMode.Suspicious;
        detFlag = false;
        if(!susFlag)
            OnFirstSuspicious();
    }

    /// <summary>
    /// Called once when the enemy first detects the player
    /// </summary>
    public virtual void OnFirstDetectPlayer() {
        detFlag = true;
        StartCoroutine(DisplayText("!", 2f));
    }

    /// <summary>
    /// Called continuously while the enemy has detected the player
    /// </summary>
    public virtual void OnDetectPlayer() {
        detectedState = DetectedMode.Detected;
        susFlag = false;
        if(!detFlag)
            OnFirstDetectPlayer();
    }

    /// <summary>
    /// Displays "?" and "!" markers above the enemy for suspicion and detection, respectively
    /// </summary>
    public virtual IEnumerator DisplayText(string newText, float time = 1f) {
        float timer = 0;
        Text tText = text.GetComponent<Text>();
        tText.text = newText;
        text.SetActive(true);
        while(timer <= time && tText.text == newText) {
            SetTextRot();
            yield return null;
            timer += Time.deltaTime;
        }
        if(text.GetComponent<Text>().text == newText)
            text.SetActive(false);
    }

    /// <summary>
    /// Sets rotation of "?" and "!" markers to face the camera
    /// </summary>
    protected void SetTextRot() {
        Vector3 camera = Camera.main.transform.position;
        camera.y = 0;
        text.GetComponent<RectTransform>().LookAt(camera);
    }

    public virtual bool WithinAttackRange() {
        return false;
    }

}
