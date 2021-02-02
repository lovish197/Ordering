using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    public static NewBehaviourScript instance;
    public List<userVariables> userVariablesList = new List<userVariables>();

    private void Awake()
    {
        instance = this;
    }
    public IEnumerator LeaderBoard()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/OrderingGame/LeaderBoard.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // show data as text
                //Debug.Log(www.downloadHandler.text);
                string[] UserDataRankWise = www.downloadHandler.text.Split(new string[] { "<br><br>" }, System.StringSplitOptions.None);
                List<string[]> userWholeData = new List<string[]>();  // jacked array

                for (int i = 0; i < UserDataRankWise.Length; i++)
                {
                    string[] ColoumnWiseData = UserDataRankWise[i].Split(new string[] { "<br>" }, System.StringSplitOptions.None);
                    List<string> colomnList = ColoumnWiseData.ToList();

                    for (int j = 0; j < colomnList.Count; j++)
                    {
                        if (colomnList[j] == null || colomnList[j] == "" || colomnList[j] == " ")
                        {
                            colomnList.RemoveAt(j);
                        }
                    }
                    userWholeData.Add(ColoumnWiseData);
                }

                for(int i =0; i < userWholeData.Count; i++)
                {

                }
            }
        }
    }

    [System.Serializable] public class userVariables{
        public Text rank,
            id,
            username,
            score;

        public userVariables(string rank, string id, string username, string score)
        {
            this.rank.text = rank;
            this.id.text = id;
            this.username.text = username;
            this.score.text = score;
        }
    }
}
