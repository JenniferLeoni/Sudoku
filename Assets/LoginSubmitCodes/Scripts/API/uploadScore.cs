using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Networking
using UnityEngine.Networking;

public class uploadScore : MonoBehaviour
{
    [SerializeField] ResultUIManager resultUIManager;

    [HideInInspector] public string score;
    [SerializeField] public string game_id;
    private string team_id;

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
        public string team_id;
        public string game_id;        
        public string score;
    }

    public void UploadScore(string num, bool fun)
    {
        team_id = GlobalTeamData.teamId;
        score = num;
        StartCoroutine(Upload(0, 3, fun));
    }

    IEnumerator Upload(int retryAmount, int maxRetryAmount, bool fun)
    {
        WWWForm form = new WWWForm();

        //kolom
        form.AddField("game_id", game_id);
        form.AddField("team_id", team_id);
        form.AddField("score", score);

        UnityWebRequest www;
        // if (fun) {
        //     www = UnityWebRequest.Post("http://localhost/irgl-api-test/insertScoreWin.php", form);
        // } else {
        //     www = UnityWebRequest.Post("http://localhost/irgl-api-test/insertScoreLose.php", form);
        // }
        if (fun) {
            // www = UnityWebRequest.Post("https://irgl.petra.ac.id/irgl2024/elim/api/insertScoreWin.php", form);
            www = UnityWebRequest.Post("https://irgl.petra.ac.id/api/uploadScoreWin", form);
        } else {
            // www = UnityWebRequest.Post("https://irgl.petra.ac.id/irgl2024/elim/api/insertScoreLose.php", form);
            www = UnityWebRequest.Post("https://irgl.petra.ac.id/api/uploadScoreLose", form);
        }
        www.timeout = 10; // Timeout in seconds

        // Token
        www.SetRequestHeader("Authorization", "Bearer " + GlobalTeamData.token);

        yield return www.SendWebRequest();
        

        // Error handling
        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);

            if (www.responseCode == 422 || www.responseCode == 405 || www.responseCode == 500)
            {
                // TODO: Show response tapi yang ada response error
                // Show response
                resultUIManager.PlayLoadingAnimation(false);
                resultUIManager.ChangeLoadingText(www.responseCode);
                Debug.Log("Response Error");

                // Cleanup
                www.Dispose();
                yield break;
            }
            else
            {
                // Retry logic
                Debug.LogWarning("Retrying score insert request...");
                if (retryAmount < maxRetryAmount)
                {
                    yield return new WaitForSeconds(Mathf.Min(8, retryAmount * 2)); // Wait for a short duration before retrying, max wait for 8 seconds
                    yield return StartCoroutine(Upload(retryAmount + 1, maxRetryAmount, fun)); // Start the retry coroutine
                }
                else
                {
                    Debug.LogError("Max retries reached, unable to upload score.");
                    // Munculkan retry button
                    resultUIManager.RetrySubmitScoreButton.interactable = true;
                    resultUIManager.PlayLoadingAnimation(false);
                    resultUIManager.ChangeLoadingText(www.responseCode);
                    resultUIManager.RetryButtonChangeColorScript.ChangeSubmitButtonImageSpriteToBorder();

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
            if (responseText.ToLower().Contains("database connection failed")) {
                // Ketika SQL Server down
                Debug.Log("SQL Down");
                resultUIManager.RetrySubmitScoreButton.interactable = true;
                resultUIManager.PlayLoadingAnimation(false);
                resultUIManager.ChangeLoadingText(599); // Code 599 = SQL Server Down
                resultUIManager.RetryButtonChangeColorScript.ChangeSubmitButtonImageSpriteToBorder();
                www.Dispose();
                yield break;
            }

            Debug.Log("Form upload complete!");

            // Deserialize JSON into UserData object
            Response userData = JsonUtility.FromJson<Response>(www.downloadHandler.text);

            // Access the fields
            string game_id = userData.data.game_id;
            string team_id = userData.data.team_id;
            string score = userData.data.score;

            Debug.Log(game_id);
            Debug.Log(team_id);
            Debug.Log(score);
            Debug.Log(www.downloadHandler.text);

            // Kalau success, back to roulette
            // Update UI
            resultUIManager.PlayLoadingAnimation(false);
            resultUIManager.ChangeLoadingText(www.responseCode);
            resultUIManager.ShowResultUI();

            // Cleanup
            www.Dispose();
            yield break;
        }
    }
}

