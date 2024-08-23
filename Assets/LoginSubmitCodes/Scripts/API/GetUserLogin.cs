using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Networking
using UnityEngine.Networking;

// UI
using UnityEngine.UI;
using TMPro;
using System;
using System.Net;

public class GetUserLogin : MonoBehaviour
{
    [SerializeField] LoginSceneUIManager loginSceneUIManager;
    [SerializeField] RetryButtonChangeColor retryButtonChangeColorScript;
    [SerializeField] public string game_id;

    [System.Serializable]
    public class Response
    {
        public int code;
        public bool success;
        public UserData data;
    }

    [System.Serializable]
    public class UserData
    {
        public string id;
        public string username;
    }


    private void Start()
    {
        // StartCoroutine(GetInfoOfUsername("user1", 0, 3));
        FetchInfoOfUsername();
    }



    private void Update()
    {
        // // When user click Return Keyboard Button, Submit form
        // if (loginSceneUIManager.LoginFormButton.interactable && Input.GetKeyUp(KeyCode.Return))
        // {
        //     FetchInfoOfUsername();
        // }
    }

    IEnumerator GetInfoOfUsername(string username, int retryAmount, int maxRetryAmount)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("game_id", game_id);

        // UnityWebRequest www = UnityWebRequest.Post("http://localhost/irgl-api-test/getUserDataByGamePass.php", form);
        // UnityWebRequest www = UnityWebRequest.Post("https://irgl.petra.ac.id/irgl2024/elim/api/getUserDataByGamePass.php", form);
        UnityWebRequest www = UnityWebRequest.Post("https://irgl.petra.ac.id/api/getUserDataByGamePass", form);
        www.timeout = 10; // Timeout in seconds

        // Token
        GlobalTeamData.token = CookieManagement.GetBrowserCookie("token");
        www.SetRequestHeader("Authorization", "Bearer " + GlobalTeamData.token);

        yield return www.SendWebRequest();

        // Error handling
        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);

            if (www.responseCode == 400 || www.responseCode == 422 || www.responseCode == 403)
            {
                // Show response
                loginSceneUIManager.PlayLoadingAnimation(false);
                loginSceneUIManager.ChangeLoadingText(www.responseCode, "");

                // Cleanup
                www.Dispose();
                yield break;
            }
            else
            {
                // Retry logic
                Debug.LogWarning("Retrying username info request...");
                if (retryAmount < maxRetryAmount)
                {
                    yield return new WaitForSeconds(Mathf.Min(8, retryAmount * 2)); // Wait for a short duration before retrying, max wait for 8 seconds
                    yield return StartCoroutine(GetInfoOfUsername(username, retryAmount + 1, maxRetryAmount)); // Start the retry coroutine
                }
                else
                {
                    Debug.LogError("Max retries reached, unable to get user info.");
                    loginSceneUIManager.LoginFormButton.interactable = true;
                    loginSceneUIManager.PlayLoadingAnimation(false);
                    loginSceneUIManager.ChangeLoadingText(www.responseCode, "");
                    retryButtonChangeColorScript.ChangeSubmitButtonImageSpriteToBorder();

                    // Cleanup
                    www.Dispose();
                    yield break;
                }
            }


        }
        else
        {
            // If MySQL Server down
            string responseText = www.downloadHandler.text;
            if (responseText.ToLower().Contains("database connection failed"))
            {
                loginSceneUIManager.LoginFormButton.interactable = true;
                loginSceneUIManager.PlayLoadingAnimation(false);
                loginSceneUIManager.ChangeLoadingText(599, ""); // Code 599 = SQL Server Down
                retryButtonChangeColorScript.ChangeSubmitButtonImageSpriteToBorder();
                www.Dispose();
                yield break;
            }

            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);

            // Deserialize JSON into UserData object
            Response userData = JsonUtility.FromJson<Response>(www.downloadHandler.text);

            // Access the fields
            string id = userData.data.id;
            string usernameFromDB = userData.data.username;

            Debug.Log(id);
            Debug.Log(usernameFromDB);
            Debug.Log(www.downloadHandler.text);

            // Update UI

            // mainMenuUIManager.LoginFormGameObjectsSetActive(false);
            // mainMenuUIManager.ResultFormGameObjectsSetActive(true);
            // mainMenuUIManager.ChangeUsernameText(usernameFromDB);
            // mainMenuUIManager.ChangeCommentText(comment);
            // mainMenuUIManager.ChangeLinkText(link);
            loginSceneUIManager.PlayLoadingAnimation(false);
            loginSceneUIManager.ChangeLoadingText(www.responseCode, usernameFromDB);
            loginSceneUIManager.PlayAnimationThenChangeScene();

            // Update Global data
            GlobalTeamData.teamId = id;
            GlobalTeamData.teamName = usernameFromDB;
            GlobalTeamData.teamGamePass = username;

            // Cleanup
            www.Dispose();
            yield break;
        }
    }

    public void FetchInfoOfUsername()
    {
        loginSceneUIManager.LoginFormButton.interactable = false;
        loginSceneUIManager.PlayLoadingAnimation(true);
        string username = getParam(OpenLinks.GetWindowLocation()); // TODO: Nyalakan ini untuk WebGL buildnya
        // string username = getParam("www.petra.ac.id/irgl24?abc"); // TODO: Ini hanya untuk testing di unity editor
        StartCoroutine(GetInfoOfUsername(username, 0, 3));
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
