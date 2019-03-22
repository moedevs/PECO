using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public GameObject PauseMenu;

    private float timeMarker;
    private bool paused, canPause;

    void Awake() {
        timeMarker = 1f;
        paused = false;
        canPause = true;
    }

    void Update() {
        if (Input.GetButtonDown("Pause") && canPause) {
            PauseGame();
        }
        MouseLock();
    }

    private void PauseGame() {
        paused = !paused;
        PauseMenu.SetActive(paused);
        try {
            if(paused) {
                timeMarker = Time.timeScale;
                Time.timeScale = 0;
            } else {
                Time.timeScale = timeMarker;
                MouseLock();
            }
        } catch {
            Debug.LogError("Pause Menu reference does not exist. Was the menu deleted, did the scene change, or was the reference not set?");
        }
    }

    private void MouseLock() {
        if (!paused) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
