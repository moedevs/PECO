using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager cm;

    public GameObject pauseMenu, stealthGradient, costumeRemoveBar;

    private void Awake() {
        if(cm == null) {
            cm = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
        pauseMenu.SetActive(false);
        stealthGradient.SetActive(false);
    }

    private void OnDestroy() {
        foreach(Transform child in transform) {
            foreach(Transform subchild in child) {
                foreach(Transform subsubchild in subchild)
                    Destroy(subsubchild.gameObject);
                Destroy(subchild.gameObject);
            }
            Destroy(child.gameObject);
        }
    }

}
