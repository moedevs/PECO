using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    private void Awake() {
        if (gm == null) {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    public void LoadScene(string scene) {
        StartCoroutine(LoadAsync(scene));
    }

    private IEnumerator LoadAsync(string scene) {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while(!async.isDone)
            yield return null;
    }

    public void SetScene(string scene) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public void UnloadScene(string scene) {
        StartCoroutine(UnloadAsync(scene));
    }

    private IEnumerator UnloadAsync(string scene) {
        AsyncOperation async = SceneManager.UnloadSceneAsync(scene);
        while(!async.isDone)
            yield return null;
    }

    public void ExitGame() {
        Application.Quit();
    }

}
