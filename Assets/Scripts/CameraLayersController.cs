using Photon.Pun;
using UnityEngine;

internal class CameraLayersController : MonoBehaviourPun
{

    private static int lr_layer = 3;
    private static int dr_layer = 6;
    private static int layerMask = 0;
    private static Camera currentCam;

    [System.Obsolete]
    public static void setupCamera()
    {
        // Get current camera and add the base layers to the mask.
        currentCam = (Camera)FindObjectOfType(typeof(Camera));
        layerMask = (1 << 0);
        layerMask |= (1 << 5);
    }

    [System.Obsolete]
    public static void switchToLR()
    {
        if (currentCam == null) 
        { 
            setupCamera();
        }
        // Add LR and remove DR layer
        layerMask |= (1 << lr_layer);
        layerMask &= ~(1 << dr_layer);
        currentCam.cullingMask = layerMask;
    }

    [System.Obsolete]
    public static void switchToDR()
    {
        if (currentCam == null)
        {
            setupCamera();
        }

        // Add DR and remove LR layer
        layerMask |= (1 << dr_layer);
        layerMask &= ~(1 << lr_layer);
        currentCam.cullingMask = layerMask;
    }

}
