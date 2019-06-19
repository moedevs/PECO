using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FormManager : MonoBehaviour {
    
    [HideInInspector] public GameObject humanPawn, testPawn, bearPawn;

    public float removeTime;
    
    private PlayerController player;
    private RectTransform removeBar;
    private AssetBundle formBundle, pecoHumanBundle, pecoTestBundle, pecoBearBundle;
    private float holdTime;
    private static bool loaded = false;

    private void Awake() {
        player = GetComponent<PlayerController>();
        // load AssetBundles
        if(!loaded) {
            LoadBundle("formdata");
            //LoadBundle("pecohuman");
            LoadBundle("pecobear");
            LoadBundle("testcapsule");
            loaded = true;
        }
        // set player FormData
        if(player.formData == null)
            player.formData = formBundle.LoadAsset<FormDataBase>("TestCapsuleData");
    }

    private void Start() {
        removeBar = CanvasManager.cm.costumeRemoveBar.GetComponent<RectTransform>();
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

    /// <summary>
    /// Returns pawn of type newForm, if it exists. If it doesn't, a new pawn is loaded from the respective AssetBundle and created.
    /// </summary>
    /// <param name="newForm">The type of pawn to return.</param>
    /// <returns>Pawn of type newForm.</returns>
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

    /// <summary>
    /// Sets the player's formData to the data for form newForm.
    /// </summary>
    public void GetNewData(PlayerController.Form newForm) {
        switch(newForm) {
            case PlayerController.Form.Human:
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

    /// <summary>
    /// Loads asset bundle with name bundleName. Should only be called in between scenes, such as during a loading screen.
    /// </summary>
    /// <param name="bundleName">The name of the asset bundle to be loaded.</param>
    public void LoadBundle(string bundleName) {
        switch(bundleName) {
            case "formdata":
                formBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/formdata"));
                return;
            case "pecohuman":
                pecoHumanBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/pecohuman"));
                return;
            case "testcapsule":
                pecoTestBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/testcapsule"));
                return;
            case "pecobear":
                pecoBearBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/pecobear"));
                return;
            default:
                Debug.Log("Attempting to load invalid AssetBundle \"" + bundleName + "\"!");
                return;
        }
    }

    /// <summary>
    /// Unloads asset bundle with name bundleName, with load type unloadType. Should only be called in between scenes, such as during a loading screen.
    /// </summary>
    /// <param name="bundleName">The name of the asset bundle to be unloaded.</param>
    /// <param name="unloadType">If true, all assets in the bundle will be unloaded and destroyed. If false, only compressed assets in the bundle are unloaded.</param>
    public void UnloadBundle(string bundleName, bool unloadType = true) {
        switch(bundleName) {
            case "formdata":
                formBundle.Unload(unloadType);
                return;
            case "pecohuman":
                pecoHumanBundle.Unload(unloadType);
                return;
            case "testcapsule":
                pecoTestBundle.Unload(unloadType);
                return;
            case "pecobear":
                pecoBearBundle.Unload(unloadType);
                return;
            default:
                Debug.Log("Attempting to unload invalid AssetBundle \"" + bundleName + "\"!");
                return;
        }
    }

}
