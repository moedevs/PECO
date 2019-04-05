using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FormData", menuName = "Form Data/Data", order = 1)]
public class FormDataBase : ScriptableObject {
    
    public float walkSpeed, jumpStrength, gravityBase, gravityShortHop, gravityFalling, maxFallSpeed, formHeight;
    public Vector3 groundedSkin;

}
