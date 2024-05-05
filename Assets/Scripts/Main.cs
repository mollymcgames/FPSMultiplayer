using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using WebSocketSharp.Server;


public class Main : MonoBehaviour
{
    public static Main instance;

    public Web Web;    

    private void Start()
    {
        instance = this;
        Web =  GetComponent<Web>();
    }
}

   