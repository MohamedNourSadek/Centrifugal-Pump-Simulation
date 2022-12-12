using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesProvider : MonoBehaviour
{
    public static ServicesProvider instance;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
    }
}
