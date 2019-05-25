using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public static GameObject player;

    public float rotateSpeed;
    public float horizOffset, verticalOffset;
    private Vector3 cameraDir = Vector3.forward;
    private Vector3 posOffset, newPos;
    private float angle, distance;
    private bool inWall;

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
        distance = Mathf.Sqrt(Mathf.Pow(horizOffset, 2f) + Mathf.Pow(verticalOffset, 2f));
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

            // Prevent camera from going inside walls
            posOffset = Vector3.Normalize(cameraDir) * -horizOffset;
            posOffset.y = verticalOffset;
            newPos = player.transform.position + posOffset;
            //Debug.DrawRay(newPos, player.transform.position - newPos, Color.blue, 2f, true);
            if(inWall) {
                if(Physics.Raycast(player.transform.position, newPos - player.transform.position, out RaycastHit hit, distance, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore))
                    newPos = hit.point;
            }

            // Apply offset position
            transform.position = newPos;
            transform.rotation = Quaternion.Euler(5, angle, 0);

        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Terrain"))
            inWall = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Terrain"))
            inWall = false;
    }

}
