using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FormManager : MonoBehaviour {
    
    [HideInInspector] public GameObject humanPawn, testPawn, bearPawn;

    private PlayerController player;
    private AssetBundle formBundle, pecoHumanBundle, pecoTestBundle, pecoBearBundle;

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
                Debug.Log("Human form data not yet set up.");
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
