using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestReadLinks : MonoBehaviour
{
    [SerializeField] TMP_Text textResult;
    [SerializeField] string rouletteLink;
    [SerializeField] string gameId;
    string linkRead;

    public void ReadLink() {
        textResult.text = OpenLinks.GetWindowLocation();
        linkRead = OpenLinks.GetWindowLocation();
        Debug.Log(getParam(linkRead));
        Debug.Log(getParam("irgl.petra.ac.id/irgl2024/lol"));
        if (getParam("irgl.petra.ac.id/irgl2024/lol") == "") {
            // Blackscreen dan Button kembali ke Roulette muncul
        }

        // Call API untuk pastikan tim ini boleh memainkan game ini.
    }

    public string getParam(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return string.Empty;
        }

        int questionMarkIndex = url.IndexOf('?');
        if (questionMarkIndex == -1 || questionMarkIndex == url.Length - 1)
        {
            return string.Empty;
        }

        return url.Substring(questionMarkIndex + 1);
    }
}
