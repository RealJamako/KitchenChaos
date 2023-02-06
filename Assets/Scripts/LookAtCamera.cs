using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private CameraLookAtModes lookAtMode;

    private void LateUpdate()
    {
        switch (lookAtMode)
        {
            case CameraLookAtModes.DefaultLookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case CameraLookAtModes.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case CameraLookAtModes.LookAtForward:
                transform.forward = Camera.main.transform.forward;
                break;
        }
    }
}