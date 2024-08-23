using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class OpenLinks : MonoBehaviour
{
    public static void OpenURL(string url)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenTab(url);
#endif
    }

    public static string GetWindowLocation()
    {
    #if !UNITY_EDITOR && UNITY_WEBGL
        return GetURLFromPage();
    #endif

        return "NOT WEBGL";
    }

    [DllImport("__Internal")]
    private static extern void OpenTab(string url);

    [DllImport("__Internal")]
    private static extern string GetURLFromPage();
}
