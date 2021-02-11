using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System;

public class LeaderBoard : MonoBehaviour
{
    int rowCount;
    public List<UserVariables> uservariablesCollection;
    public Transform Container;

    private void Awake()
    {
        StartCoroutine(GetRowCount(LeaderBoardGenerator)); // system call back for calling multiple co routines 
    }

    //delegate IEnumerator CoroutineDelegate();
    //CoroutineDelegate coroutineDelegate;

    private void Start()
    {
        
    }
    public IEnumerator GetRowCount(Func<IEnumerator> _MyFuncDelegate)
    {
        int RowCount = 1;
        //WWWForm form = new WWWForm();
        //form.AddField("rowCount", RowCount);
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/OrderingGame/GetRowCount.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                RowCount = int.Parse(www.downloadHandler.text);
                Debug.Log("Coroutine wala row count" + RowCount);
            }
        }
        RowCountMethod(RowCount);
        uservariablesCollection = new List<UserVariables>(new UserVariables[rowCount]); // initialize the list
        Debug.Log("rowCount :" + rowCount + " " + "listCount" + uservariablesCollection.Count); // for test
        StartCoroutine(_MyFuncDelegate()); // start this coroutine once the first one get completed
    }
    public IEnumerator LeaderBoardGenerator()
    {
        //Debug.Log("inside ienumerator");
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/OrderingGame/LeaderBoard.php"))
        {
            Debug.Log("inside leader board generator");
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

                //Debug.Log("Userdatalength" + userWholeData.Count);
                //uservariablesCollection = new List<UserVariables>(new UserVariables[userWholeData.Count]);
                AssignDataToLeaderBoard(userWholeData);
            }
        }
    }

    private void AssignDataToLeaderBoard(List<string[]> userWholeData)
    {
        for (int i = 0; i < userWholeData.Count; i++)
        {
            uservariablesCollection[i] = new UserVariables();
            // the data will be stored in the form of table so we made a jagged array
            // and userWholeData[i][0] means ith row and 0th colomn similarly other colomns in the row
            // containers child index is i + 1 because the 0th index is inactive check highscore.cs
            Container.GetChild(i + 1).GetChild(0).GetComponent<Text>().text = uservariablesCollection[i].rank = userWholeData[i][0];
            Container.GetChild(i + 1).GetChild(1).GetComponent<Text>().text = uservariablesCollection[i].username = userWholeData[i][2];
            Container.GetChild(i + 1).GetChild(2).GetComponent<Text>().text = uservariablesCollection[i].score = userWholeData[i][3];
        }
    }

    public void RowCountMethod(int k)
    {
        rowCount = k;
    }

    [System.Serializable] public class UserVariables
    {
        public string rank,
            username,
            score;
    }
}
