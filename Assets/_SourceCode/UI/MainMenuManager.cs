using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    private void Awake() {
        GameManager.gm.ph.canPause = false;
        GameManager.gm.ph.paused = true;
    }

    public void StartGame(string scene = "TestScene") {
        LoadingScreenManager.ls.LoadNewScene(scene);
        //StartCoroutine(StartGameCo(scene));
    }

    /*private IEnumerator StartGameCo(string scene) {
        yield return StartCoroutine(GameManager.gm.LoadAsync(scene));
        GameManager.gm.ph.canPause = true;
        GameManager.gm.ph.paused = false;
        GameManager.gm.SetScene(scene);
        GameManager.gm.UnloadScene("MainMenu");
    }*/

}
