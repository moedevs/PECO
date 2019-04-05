using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FormManager : MonoBehaviour {
    
    private PlayerController player;
    private AssetBundle formBundle;

    private void Awake() {
        player = GetComponent<PlayerController>();
        formBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/formdata"));
        if(formBundle == null) {
            Debug.LogError("Failed to load AssetBundle \"formdata\"!");
            return;
        }
        if(player.formData == null)
            player.formData = formBundle.LoadAsset<FormDataBase>("TestCapsuleData");
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
