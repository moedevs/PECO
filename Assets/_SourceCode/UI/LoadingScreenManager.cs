using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {
    
    public static LoadingScreenManager ls;

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
        GameManager.gm.ph.canPause = false;
        GameManager.gm.ph.paused = true;
        //PlayerController.pc.canAct = false;

        // Activate loading screen
        canvas.enabled = true;

        // Load and unload scenes
        Scene oldScene = SceneManager.GetActiveScene();
        yield return StartCoroutine(GameManager.gm.LoadAsync(newScene));
        yield return StartCoroutine(GameManager.gm.UnloadAsync(oldScene));
        GameManager.gm.SetScene(newScene);
        yield return null;

        // Enable pausing and movement, deactivate loading screen
        GameManager.gm.ph.canPause = true;
        GameManager.gm.ph.paused = false;
        //PlayerController.pc.canAct = true;
        canvas.enabled = false;
    }

}
