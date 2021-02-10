using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    public List<UserVariables> userVariablesList = new List<UserVariables>();
    private void Start()
    {
        StartCoroutine(LeaderBoardGenerator());
    }
    public IEnumerator LeaderBoardGenerator()
    {
        //Debug.Log("inside ienumerator");
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/OrderingGame/LeaderBoard.php"))
        {
            //Debug.Log("inside leader board generator");
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // show data as text
                //Debug.Log(www.downloadHandler.text);
                //Debug.Log("inside else condition");
                string[] UserDataRankWise = www.downloadHandler.text.Split(new string[] { "<br><br>" }, System.StringSplitOptions.None);
                List<string[]> userWholeData = new List<string[]>();  // jagged array
                for (int i = 0; i < UserDataRankWise.Length; i++)
                { 
                    if (UserDataRankWise[i].Trim().Length > 0)  // trim() is used to neglect spacs from a string
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
                        userWholeData.Add(colomnList.ToArray());
                    }
                }

                Debug.Log("Userdatalength" + userWholeData.Count);

                for(int i =0; i < userWholeData.Count; i++)
                {
                    // the data will be stored in the form of table so we made a jagged array
                    // and userWholeData[i][0] means ith row and 0th colomn similarly other colomns in the row
                    //Debug.Log("Printing user data" + userWholeData[i][0] + " " + userWholeData[i][1] + " " + userWholeData[i][2] + " " + userWholeData[i][3]);
                    userVariablesList[i].rank = userWholeData[i][0];
                    userVariablesList[i].id = userWholeData[i][1];
                    userVariablesList[i].username = userWholeData[i][2];
                    userVariablesList[i].score = userWholeData[i][3];
                }
            }
        }
    }

    [System.Serializable]public class UserVariables
    {
        public string rank,
            id,
            username,
            score;
        public int temp;
        public UserVariables(string rank, string id, string username, string score)
        {
            this.rank = rank;
            this.id = id;
            this.username = username;
            this.score = score;
        }
    }
}
