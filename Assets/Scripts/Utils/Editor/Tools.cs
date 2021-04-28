using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class Tools
{
    [MenuItem("Tools/Screenshot")]
    static void Screenshot() => ScreenCapture.CaptureScreenshot(EditorUtility.SaveFilePanel("Choose Screenshot Save Path","/","screenshot.png","png"));
}
