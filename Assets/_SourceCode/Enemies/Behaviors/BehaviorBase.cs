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

    public virtual void OnIdle() {
        susFlag = false;
        detFlag = false;
    }

    public virtual void OnFirstSuspicious() {
        susFlag = true;
        StartCoroutine(DisplayText("?", 1.5f));
    }

    public virtual void OnSuspicious() {
        detectedState = DetectedMode.Suspicious;
        detFlag = false;
        if(!susFlag)
            OnFirstSuspicious();
    }

    public virtual void OnFirstDetectPlayer() {
        detFlag = true;
        StartCoroutine(DisplayText("!", 2f));
    }

    public virtual void OnDetectPlayer() {
        detectedState = DetectedMode.Detected;
        susFlag = false;
        if(!detFlag)
            OnFirstDetectPlayer();
    }

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

    protected void SetTextRot() {
        Vector3 camera = Camera.main.transform.position;
        camera.y = 0;
        text.GetComponent<RectTransform>().LookAt(camera);
    }

}
