using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ZoomMode
{
    Default,
    KnifeZoom
}


[RequireComponent(typeof(Camera))]
public class CameraZoomer : MonoBehaviour
{
    private Camera camera;

    public float knifeZoom;
    private float Default;
    


    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        Default = camera.fieldOfView;
    }

    public void SetZoom(ZoomMode mode)
    {
        switch(mode)
        {
            case ZoomMode.Default:
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, Default, 0.05f);
                break;
            case ZoomMode.KnifeZoom:
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, knifeZoom, 0.05f);
                break;
        }
    }
}
