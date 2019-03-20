using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCapsule : ScriptableObject, FormDataBase {

    public float WalkSpeed {
        get { return 6; }
    }

    public float GravityBase {
        get { return 5; }
    }

    public float GravityShortHop {
        get { return 3; }
    }

    public float GravityFalling {
        get { return 8; }
    }

    public Vector3 GroundedSkin {
        get { return new Vector3(0.25f, 0.005f, 0.25f); }
    }

}
