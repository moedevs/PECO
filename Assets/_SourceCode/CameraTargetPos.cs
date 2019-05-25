using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetPos : MonoBehaviour {
    
    public static CameraTargetPos target;

    [HideInInspector] public float leftDist, rightDist;
    [HideInInspector] public bool leftInWall, rightInWall;
    private bool insideWall;

    private void Awake() {
        if(target == null) {
            target = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    private void LateUpdate() {
        if(!insideWall) {
            if(Physics.Raycast(transform.position + transform.TransformVector(Vector3.right * 0.1f), transform.TransformVector(Vector3.left), out RaycastHit hitL, 0.3225f + 0.15f, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore)) {
                leftInWall = true;
                leftDist = 0.3225f - hitL.distance + 0.1f;
            } else {
                leftInWall = false;
                leftDist = 0;
            }
            if(Physics.Raycast(transform.position + transform.TransformVector(Vector3.left * 0.1f), transform.TransformVector(Vector3.right), out RaycastHit hitR, 0.3225f + 0.15f, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore)) {
                rightInWall = true;
                rightDist = 0.3225f - hitR.distance + 0.1f;
            } else {
                rightInWall = false;
                rightDist = 0;
            }
        } else {
            leftInWall = true;
            rightInWall = true;
        }
        
        Debug.Log(insideWall);

    }

    private void OnTriggerEnter(Collider other) {
        insideWall = true;
    }

    private void OnTriggerExit(Collider other) {
        insideWall = false;
    }

}
