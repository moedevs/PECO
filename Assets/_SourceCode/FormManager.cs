using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FormManager : MonoBehaviour {
    
    [HideInInspector] public GameObject humanPawn, testPawn, bearPawn;

    public float removeTime;
    public RectTransform removeBar;

    private PlayerController player;
    private AssetBundle formBundle, pecoHumanBundle, pecoTestBundle, pecoBearBundle;
    private float holdTime;

    private void Awake() {
        player = GetComponent<PlayerController>();
        // load AssetBundles
        formBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/formdata"));
        if(formBundle == null) {
            Debug.LogError("Failed to load AssetBundle \"formdata\"!");
            return;
        }
        /*pecoHumanBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/pecohuman"));
        if(pecoHumanBundle == null) {
            Debug.LogError("Failed to load AssetBundle \"pecohuman\"!");
            //return;
        }*/
        pecoTestBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/testcapsule"));
        if(pecoTestBundle == null) {
            Debug.LogError("Failed to load AssetBundle \"testcapsule\"!");
            return;
        }
        pecoBearBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/pecobear"));
        if(pecoBearBundle == null) {
            Debug.LogError("Failed to load AssetBundle \"pecobear\"!");
            return;
        }
        // set player FormData
        if(player.formData == null)
            player.formData = formBundle.LoadAsset<FormDataBase>("TestCapsuleData");
    }

    private void Update() {
        if(removeTime <= 0.1f)
            removeTime = 0.11f;
        if(player.currentForm != PlayerController.Form.Human && player.currentForm != PlayerController.Form.Test) {
            if(Input.GetButton("RemoveCostume")) {
                holdTime += Time.deltaTime;
                if(holdTime >= removeTime) {
                    player.ChangeControlledPawn(PlayerController.Form.Human);
                    removeBar.sizeDelta = new Vector2(0f, 30f);
                } else if(holdTime > 0.1f)
                    removeBar.sizeDelta = new Vector2(((holdTime - 0.1f) / (removeTime - 0.1f)) * 400f, 30f);
            } else if(Input.GetButtonUp("RemoveCostume")) {
                holdTime = 0;
                removeBar.sizeDelta = new Vector2(0f, 30f);
            }
        } else {
            holdTime = 0;
            removeBar.sizeDelta = new Vector2(0f, 30f);
        }
    }

    public GameObject GetNewPawn(PlayerController.Form newForm) {
        switch(newForm) {
            case PlayerController.Form.Human:
                return humanPawn;
            case PlayerController.Form.Bear:
                if(bearPawn == null)
                    bearPawn = Instantiate(pecoBearBundle.LoadAsset<GameObject>("PecoBear"));
                return bearPawn;
            case PlayerController.Form.Test:
                if(testPawn == null)
                    testPawn = Instantiate(pecoTestBundle.LoadAsset<GameObject>("TestCapsule"));
                return testPawn;
            default:
                Debug.Log("Attempting to switch to form that is not set up in FormManager.");
                return null;
        }
    }

    public void GetNewData(PlayerController.Form newForm) {
        switch(newForm) {
            case PlayerController.Form.Human:
                player.formData = formBundle.LoadAsset<FormDataBase>("TestCapsuleData");
                player.formData = formBundle.LoadAsset<FormDataBase>("TestCapsuleData");
                return;
            case PlayerController.Form.Bear:
                player.formData = formBundle.LoadAsset<FormDataBase>("BearData");
                return;
            case PlayerController.Form.Test:
                player.formData = formBundle.LoadAsset<FormDataBase>("TestCapsuleData");
                return;
            default:
                Debug.Log("Attempting to switch to form that is not set up in FormManager.");
                return;
        }
    }

}
