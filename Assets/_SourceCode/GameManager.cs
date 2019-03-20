using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
   
    public static GameManager gm;

    private void Awake() {
        if(gm == null) {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    private void Update() {
        //Test();
    }

    private void Test() {
        if(Input.GetButtonDown("AttackScissor"))
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

}
