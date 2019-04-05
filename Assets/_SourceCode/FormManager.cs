using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FormManager : MonoBehaviour {
    
    private PlayerController player;

    private void Awake() {
        player = GetComponent<PlayerController>();
        var testCapsule = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/formdata"));
        if(testCapsule == null) {
            Debug.LogError("Failed to load AssetBundle \"formdata\"!");
            return;
        }
        player.formData = testCapsule.LoadAsset<FormDataBase>("TestCapsuleData");
    }

}
