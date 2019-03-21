using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    GameObject PauseMenu;
    private bool paused, canPause;

    void Awake() {
        GameObject PauseMenu = GameObject.Find("PauseMenu");
        paused = false;
        canPause = true;
    }

    void Update() {
        if (Input.GetButtonDown("Pause") && canPause) {
            paused = !paused;
        }
        PauseMenu.SetActive(paused);
        MouseLock();
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
