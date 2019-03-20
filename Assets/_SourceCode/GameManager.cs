using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
   
    public static GameManager gm;

    private bool paused, canPause;

    private void Awake() {
        if(gm == null) {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        paused = false;
        canPause = true;
    }

    private void Update() {
        //Test();
        MouseLock();
    }

    private void Test() {
        if(Input.GetButtonDown("Pause"))
            Debug.Log("button down");
        else if(Input.GetAxisRaw("RemoveCostume") > 0.05)
            Debug.Log("axis down");
    }

    public void LoadScene(string scene) {
        try {
            LoadScene(scene);
        } catch {
            Debug.LogError("Attempting to load invalid scene " + scene);
        }
    }

    public void ExitGame() {
        Application.Quit();
    }

    private void MouseLock() {
        if (Input.GetButtonDown("Pause") && canPause) {
            paused = !paused;
        }
        if (!paused) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
