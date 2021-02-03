using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class AchievementManager : MonoBehaviour
{
    //achievement variables
  //  [HideInInspector]
//    public int a1 = 0, a2 = 0;                                      // a1 represents Achievement 1         

    public GameObject general;
    public GameObject earnCanvas;

    public GameObject achievementPrefab;
    public Sprite[] badge;
    public GameObject visualAchievement;
    public GameObject EarnCanvas;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    public Sprite unlockedSprite;

    public Text textPoints;

    int fadeTime = 1;

    private static AchievementManager instance;
    public List<AchievementScriptable> allAchievements;

    public static AchievementManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievementManager>();
            }

            return AchievementManager.instance;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // REMEMBER TO DELETE THIS, IF THE DATA NEEDS TO BE SAVED
       // PlayerPrefs.DeleteAll();

//        CreateAchievement(general, "Press f", "press f 3 times to unlock", 15, 0, 3);
        CreateAchievementInitial();
        
  //      CreateAchievement(general, "Press W", "press W to unlock", 5, 0, 0);
    //    CreateAchievement(general, "Press S", "press S to unlock", 5, 1, 0);
       // CreateAchievement(general, "Press W and S", "press W and S to unlock", 10, 2, 0, new string[] { "Press W", "Press S" });
        

//        CreateAchievement(general, "Achievement 1", "Answer 1 question", 5, 0, 0);
  //      CreateAchievement(general, "Achievement 2", "Answer 5 question", 10, 2, 5);
    }

    private void CreateAchievementInitial()
    {
        int badgenumber = -1;
        foreach (var item in allAchievements)
        {
            badgenumber++;
            if (badgenumber >= badge.Length) { badgenumber = 0; }

            //CreateAchievement(general, item.Title, item.Description +" to unlock", item.Points, item.SpriteIndex, item.Progress,item.DependenciesTitles);
            CreateAchievement(general, item.Title, item.Description +" to unlock", item.Points, badgenumber, item.Progress,item.DependenciesTitles);

        }
    }

    public void CreateAchievement(GameObject parent, string title, string description, int points, int spriteIndex, int progress, string[] dependencies = null)        //int achievementIndex
    {
        GameObject achievement = Instantiate(achievementPrefab) as GameObject;
        // achievement.transform.SetParent(GameObject.Find(parent).transform);
        achievement.transform.SetParent(general.transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);

        Achievement newAchievement = new Achievement(title, description, points, spriteIndex, achievement, progress);
        achievements.Add(title, newAchievement);

        SetAchievementInfo(parent, achievement, title, progress);

        if (dependencies != null && dependencies.Length>0)
        {
            foreach (string achievementTitle in dependencies)
            {
                Achievement dependency = achievements[achievementTitle];
                dependency.Child = title;
                newAchievement.AddDependency(dependency);
            }
        }
    }

    public void SetAchievementInfo(GameObject parent, GameObject achievement, string title, int progression = 0)
    {
        string progress = progression > 0 ? "  " + PlayerPrefs.GetInt("Progression" + title) + "/" + progression.ToString() : string.Empty;
        //achievement.transform.SetParent(GameObject.Find(parent).transform);
        //achievement.transform.SetParent(general.transform);
        //achievement.transform.SetParent(parent.transform);
        //achievement.GetComponent<RectTransform>().SetParent(parent.GetComponent<RectTransform>());
        achievement.transform.GetChild(0+1).GetComponent<Text>().text = title + progress;
        achievement.transform.GetChild(1+1).GetComponent<Text>().text = achievements[title].Description;
        achievement.transform.GetChild(2+1).GetChild(0).GetComponent<Text>().text = achievements[title].Points.ToString();
        achievement.transform.GetChild(3+1).GetComponent<Image>().sprite = badge[achievements[title].SpriteIndex];
    }

    public void SetAchievementInfoForEarning(GameObject parent, GameObject achievement, string title, int progression = 0)
    {
        string progress = progression > 0 ? "  " + PlayerPrefs.GetInt("Progression" + title) + "/" + progression.ToString() : string.Empty;
        //achievement.transform.SetParent(GameObject.Find(parent).transform);
        //achievement.transform.SetParent(general.transform);
        //achievement.transform.SetParent(parent.transform);
        //achievement.GetComponent<RectTransform>().SetParent(parent.GetComponent<RectTransform>());
        achievement.transform.GetChild(0).GetChild(0+1).GetComponent<Text>().text = title + progress;
        achievement.transform.GetChild(0).GetChild(1 + 1).GetComponent<Text>().text = "Congratulations\n";
        achievement.transform.GetChild(0).GetChild(2+1).GetChild(0).GetComponent<Text>().text = achievements[title].Points.ToString();
        achievement.transform.GetChild(0).GetChild(3+1).GetComponent<Image>().sprite = badge[achievements[title].SpriteIndex];
    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            AchievementData.instance.InsertTitlesWon(title);
            //CreateAchievementAfterLevelWon(title);
        }
    }

    public void CreateAchievementAfterLevelWon(string title)
    {
        GameObject achievement = Instantiate(visualAchievement) as GameObject;

        //SetAchievementInfo("EarnCanvas", achievement, title);
        achievement.GetComponent<RectTransform>().SetParent(earnCanvas.GetComponent<RectTransform>());
        //achievement.GetComponent<RectTransform>().localScale = new Vector3(1.35f, 1.35f, 0);
        achievement.GetComponent<RectTransform>().localScale = new Vector3(0.8513814f, 0.8513814f, 0);
        SetAchievementInfoForEarning(earnCanvas, achievement, title);
        textPoints.text = "Points:" + PlayerPrefs.GetInt("Points");
        //StartCoroutine(FadeAchievement(achievement)); //This is for fading out the achievement items.
    }

    /// <summary>
    /// Achievements should be delete which has shown to users
    /// </summary>
    public void DeleteAllPreviousAchievement()
    {
        GameObject[] k = new GameObject[earnCanvas.transform.childCount];


        for (int i = 0; i < k.Length; i++)
        {
            k[i] = earnCanvas.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < k.Length; i++)
        {
            Destroy(k[i]);
        }
        
        
    }

    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    // Update is called once per frame
    void Update()
    {
      /*  if (Input.GetKeyDown(KeyCode.W))
        {
            EarnAchievement("Press W");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            EarnAchievement("Press S");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            EarnAchievement("Press f");
        }

        if (a1 == 1)
        {
            print(a1);
            EarnAchievement("Achievement 1");
        }
        if (a2 == 2)
        {
            EarnAchievement("Achievement 2");
        }
*/
    }

    IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();
        float rate = 1.0f / fadeTime;
        int startAlpha = 0;
        int endAlpha = 1;


        for (int i = 0; i < 2; i++)
        {
            float progress = 0.0f;
            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(2);

            startAlpha = 1;
            endAlpha = 0;
        }

        Destroy(achievement);

    }

    //[ContextMenu("CreateAchievementObject")]
    //public void Create()
    //{
    //    AchievementScriptable obj;
        
    //    for (int i = 0; i < allAchievements.Count; i++)
    //    {
    //        obj = ScriptableObject.CreateInstance<AchievementScriptable>();
    //        obj.Title = allAchievements[i].Title;
    //        obj.Description = allAchievements[i].Description;
    //        obj.Points = allAchievements[i].Points;
    //        obj.SpriteIndex = allAchievements[i].SpriteIndex;
    //        obj.Progress = allAchievements[i].Progress;
    //        obj.DependenciesTitles = new string[ allAchievements[i].DependenciesTitles.Length ];
    //        for (int j = 0; j < allAchievements[i].DependenciesTitles.Length; j++)
    //        {
    //            obj.DependenciesTitles[j] = allAchievements[i].DependenciesTitles[j];
    //        }
    //        UnityEditor.AssetDatabase.CreateAsset(obj, "Assets/PanelItems/AllAchievement" + "/" + obj.Title + ".asset");
    //    }
        

        
    //}
}



