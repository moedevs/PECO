using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public static GameObject player;

    public float rotateSpeed;
    public float horizOffset, verticalOffset;
    [HideInInspector] public Vector3 cameraDir = Vector3.forward;
    private Vector3 posOffset;
    private float angle;

    private void Start() {
        if(Camera.main != GetComponent<Camera>()) {
            try {
                horizOffset = Camera.main.GetComponent<CameraController>().horizOffset;
                verticalOffset = Camera.main.GetComponent<CameraController>().verticalOffset;
            } catch {
                Debug.Log("Previous main camera did not have CameraController component, unable to retrieve offset");
            }
            Destroy(Camera.main.gameObject);
        }
        if(player == null)
            player = PlayerController.pc.controlledPawn;
        tag = "MainCamera";
    }

    private void LateUpdate() {
        if(PlayerController.pc.canAct) {
            // Convert input into rotation
            angle += Input.GetAxis("CameraX") * rotateSpeed;
            if(angle > 360f)
                angle -= 360f;
            else if(angle < 0f)
                angle += 360f;
            cameraDir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));

            // Apply offset position
            posOffset = Vector3.Normalize(cameraDir) * -horizOffset;
            posOffset.y = verticalOffset;
            transform.position = player.transform.position + posOffset;
            transform.rotation = Quaternion.Euler(5, angle, 0);
        }
    }

}
