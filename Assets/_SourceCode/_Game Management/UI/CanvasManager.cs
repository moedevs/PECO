using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager cm;

    public GameObject pauseMenu, stealthGradient, costumeRemoveBar, healthBar;
    public Sprite fullHeart, emptyHeart;

    [HideInInspector] public List<Image> healthSprites;
    
    private void Awake() {
        if(cm == null) {
            cm = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
        pauseMenu.SetActive(false);
        stealthGradient.SetActive(false);
        foreach(Transform child in healthBar.transform)
            healthSprites.Add(child.GetComponent<Image>());
    }

    public void PauseGame() {
        GameManager.ph.PauseGame();
    }

    public void ExitGame() {
        GameManager.gm.ExitGame();
    }

    public void LoadScene(string scene) {
        LoadingScreenManager.ls.LoadNewScene(scene);
    }

    private void OnDestroy() {
        foreach(Transform child in transform) {
            foreach(Transform subchild in child) {
                foreach(Transform subsubchild in subchild) {
                    Destroy(subsubchild.gameObject);
                }
                Destroy(subchild.gameObject);
            }
            Destroy(child.gameObject);
        }
    }

}
