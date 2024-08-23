using System.Runtime.InteropServices;
using UnityEngine;

public class CookieManagement : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SetCookie(string name, string value, int days);

    [DllImport("__Internal")]
    private static extern string GetCookie(string name);

    public static void SetBrowserCookie(string name, string value, int days)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            SetCookie(name, value, days);
        #else
            Debug.LogWarning("SetCookie is only available in WebGL builds.");
        #endif
    }

    public static string GetBrowserCookie(string name)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            return GetCookie(name);
        #else
            Debug.LogWarning("GetCookie is only available in WebGL builds.");
            return null;
        #endif
    }
}
