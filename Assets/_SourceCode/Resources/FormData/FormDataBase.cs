using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FormDataBase {
    
    float WalkSpeed { get; }
    float GravityBase { get; }
    float GravityShortHop { get;}
    float GravityFalling { get; }
    Vector3 GroundedSkin { get; }

}
