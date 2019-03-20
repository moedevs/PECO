using System;
using UnityEngine;

public static class Utils
{
    public static void WithKeyHold(KeyCode code, Action onHold, Action onRelease)
    {
        if (Input.GetKeyDown(code))
        {
            onHold();
        }

        if (Input.GetKeyUp(code))
        {
            onRelease();
        }
    }
}