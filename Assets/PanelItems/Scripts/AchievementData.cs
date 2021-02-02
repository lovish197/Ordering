using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementData : MonoBehaviour
{
    public static AchievementData instance;
    public string Levelnamemanually;
    public Vector2 Level;

    public List<string> TitlesWon;

    bool hasTimeSetToOne;
    #region MonobehavioursMethod
    void Awake()
    {
    //    instance = this;
        MakeSingleton();
        TitlesWon = new List<string>();
    }
    

    private void MakeSingleton()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion


    [ContextMenu("RightAnsManually")]
    public void InsertAchievementForRightAnswer()
    {
        AchievementManager achievementManager = GameObject.FindObjectOfType<AchievementManager>();
        if (achievementManager != null)
        {
            for (int i = 0; i < 23; i++)
            {
                //achievementManager.EarnAchievement("Press f");
                achievementManager.EarnAchievement(achievementManager.allAchievements[i].Title);
            }
        }
    }
    [ContextMenu("LevelWinManually")]
    public void InsertAchievementOnLevelWin()
    {
        AchievementManager achievementManager = GameObject.FindObjectOfType<AchievementManager>();
        if (achievementManager != null)
        {
            //string title = GameObject.FindObjectOfType<SceneInfo>().Levelname;
            string title = "Level " + GetLevelNumber(); 
            achievementManager.EarnAchievement(title);

        }
        //CreateAchievementWhichUserHaveGot();
    }

    [ContextMenu("LevelWinManuallyFromCurrent")]
    public void InsertAchievementOnLevelWinsss()
    {
        AchievementManager achievementManager = GameObject.FindObjectOfType<AchievementManager>();
        if (achievementManager != null)
        {
            //string title = GameObject.FindObjectOfType<SceneInfo>().Levelname;
            string title = Levelnamemanually; 
            achievementManager.EarnAchievement(title);

        }
    }

    [ContextMenu("LevelWinManuallyFromCurrentInRange")]
    public void InsertAchievementOnLevelWinsssss()
    {
        AchievementManager achievementManager = GameObject.FindObjectOfType<AchievementManager>();
        int n = (int)Level.y - (int)Level.x;
        int number = (int)Level.x;
        for (int i = 0; i < n; i++)
        {
            
            if (achievementManager != null)
            {
                //string title = GameObject.FindObjectOfType<SceneInfo>().Levelname;
                string title = "Level "+number;
                achievementManager.EarnAchievement(title);

            }
            number++;
        }
    }

    int GetLevelNumber()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    [ContextMenu("CreateAchievementsWhichUserHaveGot")]
    public void CreateAchievementWhichUserHaveGot()
    {
        if (TitlesWon.Count > 0f)
        {

            GameObject.FindObjectOfType<AchievementManager>().EarnCanvas.SetActive(true);
            foreach (var item in TitlesWon)
            {
                GameObject.FindObjectOfType<AchievementManager>().CreateAchievementAfterLevelWon(item);
            }

            TitlesWon.Clear();

            if (Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
                hasTimeSetToOne = true;
            }

            StartCoroutine(E_DisabledAchievement());
        }

    }

    IEnumerator E_DisabledAchievement()
    {
        yield return new WaitForSeconds(2f);
        
        AchievementManager.Instance.DeleteAllPreviousAchievement();
        ResetTiming();
        GameObject.FindObjectOfType<AchievementManager>().EarnCanvas.SetActive(false);

    }

    public void ResetTiming()
    {
        if (hasTimeSetToOne) { Time.timeScale = 0f; hasTimeSetToOne = false; }
    }

    public void InsertTitlesWon(string _title)
    {
        if(!TitlesWon.Contains(_title))
        TitlesWon.Add(_title);
    }
        
}


