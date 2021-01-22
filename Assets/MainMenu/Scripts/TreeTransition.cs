using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TreeTransition : MonoBehaviour
{
    public static TreeTransition instance;
    public Animator TreesAnimtaor;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //    Open();
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (TreesAnimtaor == null) { TreesAnimtaor = transform.GetChild(0).gameObject.GetComponent<Animator>(); }
    }

    /// <summary>
    /// When Trees goes out from center
    /// </summary>
    public void Open()
    {
        TreesAnimtaor.GetComponent<Animator>().SetTrigger("open");
    }
    /// <summary>
    /// When Trees goes to center
    /// </summary>
    public void Close()
    {
        TreesAnimtaor.GetComponent<Animator>().SetTrigger("close");

        Invoke("PleaseShowAchievement", 1.5f);  
    }
    void PleaseShowAchievement()
    {

        if (AchievementData.instance != null)
        {
            if (AchievementData.instance.TitlesWon.Count > 0f)
            {
                AchievementData.instance.CreateAchievementWhichUserHaveGot();
            }
            else
            {
              //  ShowLevelSelectInfo();
            }

        }
        else
        {
            //ShowLevelSelectInfo();
        }
    }

    private void ShowLevelSelectInfo()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.GetComponentInChildren<Text>().text = SceneManager.GetActiveScene().name;
    }

    public void OpenClose()
    {
        TreesAnimtaor.GetComponent<Animator>().SetTrigger("openclose");
    }

    
}
