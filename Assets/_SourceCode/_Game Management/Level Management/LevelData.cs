using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {
    
    public static LevelData data;

    public Vector3 currentRespawn;
    public Transform spawnPosition;
    public float minYPos = -10f;

    public bool debugRespawn;

    private void Awake() {
        if(data == null) {
            data = this;
        } else if(data != this) {
            Destroy(data.gameObject);
            data = this;
        }
        try {
            currentRespawn = spawnPosition.position;
        } catch {
            currentRespawn = PlayerController.pc.controlledPawn.transform.position;
        }
    }

    private void Update() {
        if(debugRespawn) {
            RespawnPlayer();
            debugRespawn = false;
        } else if(!LoadingScreenManager.ls.loading && PlayerController.pc.controlledPawn.transform.position.y < minYPos) 
            RespawnPlayer();
    }

    public void RespawnPlayer() {
        PlayerController.pc.TeleportPlayer(currentRespawn);
    }

}
