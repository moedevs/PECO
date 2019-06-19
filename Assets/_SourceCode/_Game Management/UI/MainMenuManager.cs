using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public Canvas mainMenu, optionsMenu, creditsMenu;

    private void Awake() {
        PauseHandler.canPause = false;
        PauseHandler.paused = true;

        mainMenu.enabled = true;
        optionsMenu.enabled = false;
        creditsMenu.enabled = false;
    }

    private void Start() {
        CanvasManager.cm.GetComponent<Canvas>().enabled = false;
    }

    public void StartGame(string scene = "TestScene") {
        LoadingScreenManager.ls.LoadNewScene(scene);
        //StartCoroutine(StartGameCo(scene));
    }

    // 0 = mainMenu, 1 = optionsMenu, 2 = creditsMenu
    public void OpenMenu(int menuId) {
        switch(menuId) {
            case 0:
                mainMenu.enabled = true;
                optionsMenu.enabled = false;
                creditsMenu.enabled = false;
                break;
            case 1:
                mainMenu.enabled = false;
                optionsMenu.enabled = true;
                creditsMenu.enabled = false;
                break;
            case 2:
                mainMenu.enabled = false;
                optionsMenu.enabled = false;
                creditsMenu.enabled = true;
                break;
            default:
                break;
        }
    }

    /*private IEnumerator StartGameCo(string scene) {
        yield return StartCoroutine(GameManager.gm.LoadAsync(scene));
        GameManager.gm.ph.canPause = true;
        GameManager.gm.ph.paused = false;
        GameManager.gm.SetScene(scene);
        GameManager.gm.UnloadScene("MainMenu");
    }*/

}
