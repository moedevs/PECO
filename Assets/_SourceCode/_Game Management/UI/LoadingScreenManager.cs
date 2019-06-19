using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {
    
    public static LoadingScreenManager ls;

    public bool loading = false;

    private Canvas canvas;

    private void Awake() {
        if(ls == null) {
            ls = this;
            DontDestroyOnLoad(gameObject);
        } else {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
            Destroy(gameObject);
        }
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void LoadNewScene(string newScene) {
        StartCoroutine(LoadCoroutine(newScene));
    }

    private IEnumerator LoadCoroutine(string newScene) {
        // Disable pausing and movement
        PauseHandler.canPause = false;
        PauseHandler.paused = true;
        Time.timeScale = 0;
        try {
            PlayerController.pc.currentForm = PlayerController.Form.Test;
            PlayerController.pc.canAct = false;
        } catch { }

        // Activate loading screen
        canvas.enabled = true;
        loading = true;

        // Load and unload scenes
        Scene oldScene = SceneManager.GetActiveScene();
        yield return StartCoroutine(GameManager.gm.LoadAsync(newScene));
        yield return StartCoroutine(GameManager.gm.UnloadAsync(oldScene));
        GameManager.gm.SetScene(newScene);
        yield return null;

        // Enable pausing and movement, deactivate loading screen
        try {
            PlayerController.pc.canAct = true;
        } catch { }
        CanvasManager.cm.GetComponent<Canvas>().enabled = true;
        if(newScene != "MainMenu") {
            PlayerController.pc.FindNewPawn();
            yield return new WaitForSecondsRealtime(0.05f);
            PauseHandler.canPause = true;
            PauseHandler.paused = false;
            Time.timeScale = 1;
        } else {
            PauseHandler.SetMenu(false);
        }
        canvas.enabled = false;
        loading = false;
    }

}
