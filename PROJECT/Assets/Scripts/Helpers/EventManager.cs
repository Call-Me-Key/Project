using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CameraMoveEvent : UnityEvent<CameraController>
{
}

public static class EventManager
{
    public static CameraMoveEvent cameraMove = new CameraMoveEvent();
}
