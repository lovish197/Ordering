using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Ordering;
public class web : MonoBehaviour
{
    int gameScore;

    private void Awake()
    {
        //gameScore = ScoreManager.instance.currentScore;
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetText());
        //StartCoroutine(GetUsers());
        //StartCoroutine(RegisterUser("test100", "123"));
        gameScore = ScoreManager.instance.GetCurrentScore();
    }

    IEnumerator GetText()
    {
        using(UnityWebRequest www = UnityWebRequest.Get("http://localhost/OrderingGame/GetDate.php"))
        {
            yield return www.Send();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // show data as text
                Debug.Log(www.downloadHandler.text);

                // retrieve data as binary data
                byte[] result = www.downloadHandler.data; 
            }
        }
    }

    IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/OrderingGame/getUser.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // show data as text
                Debug.Log(www.downloadHandler.text);

                // retrieve data as binary data
                byte[] result = www.downloadHandler.data;
            }
        }
    }

    public IEnumerator Login(string userName, string passWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", userName);
        form.AddField("loginPassword", passWord);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/OrderingGame/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                StartCoroutine(UpdateUserScore(FindObjectOfType<login>().userNameInput.text, gameScore)); // update score after login
            }
        }
    }

    public IEnumerator RegisterUser(string userName, string passWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", userName);
        form.AddField("loginPassword", passWord);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/OrderingGame/registerUser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator UpdateUserScore(string userName, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", userName);
        form.AddField("score", score);

        using(UnityWebRequest www = UnityWebRequest.Post("http://localhost/OrderingGame/updateScore.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
