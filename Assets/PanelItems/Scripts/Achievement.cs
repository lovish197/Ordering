using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class Achievement
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        int points;

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        int spriteIndex;

        public int SpriteIndex
        {
            get { return spriteIndex; }
            set { spriteIndex = value; }
        }

        bool unlocked;

        public bool Unlocked
        {
            get { return unlocked; }
            set { unlocked = value; }
        }



        GameObject achievementRef;

        List<Achievement> dependencies = new List<Achievement>();

        int currentProgression;
        int maxProgression;

        string child;

        public string Child
        {
            get => child;
            set => child = value;
        }

        public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef, int maxProgression)
        {
            this.name = name;
            this.description = description;
            this.points = points;
            this.spriteIndex = spriteIndex;
            this.unlocked = false;
            this.achievementRef = achievementRef;
            this.maxProgression = maxProgression;
            LoadAchievement();
        }

        public void AddDependency(Achievement dependency)
        {
            dependencies.Add(dependency);
        }

        public bool EarnAchievement()
        {
            if (!unlocked && !dependencies.Exists(x => x.unlocked == false) && CheckProgress())
            {
                achievementRef.GetComponent<Image>().sprite = AchievementManager.Instance.unlockedSprite;
                SaveAchievement(true);

                if (child != null)
                {
                    AchievementManager.Instance.EarnAchievement(child);
                }

                return true;
            }
            return false;
        }

        public void SaveAchievement(bool value)
        {
            unlocked = value;

            int tmpPoints = PlayerPrefs.GetInt("Points", 0);
            tmpPoints += points;
            PlayerPrefs.SetInt("Points", PlayerPrefs.GetInt("Points", 0) + points);
            PlayerPrefs.SetInt("Progression" + Name, currentProgression);
            PlayerPrefs.SetInt(name, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void LoadAchievement()
        {
            unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;

            if (unlocked)
            {
                AchievementManager.Instance.textPoints.text = "Points" + PlayerPrefs.GetInt("Points");
                currentProgression = PlayerPrefs.GetInt("Progression" + Name);
                achievementRef.GetComponent<Image>().sprite = AchievementManager.Instance.unlockedSprite;
            }
        }

        public bool CheckProgress()
        {
            currentProgression =  PlayerPrefs.GetInt("Progression" + Name, 0);
            currentProgression++;

            PlayerPrefs.SetInt("Progression" + Name, currentProgression);

        if (maxProgression != 0)
        {
            achievementRef.transform.GetChild(0+1).GetComponent<Text>().text = Name + " " + currentProgression + "/" + maxProgression;
        }
        else
        {
            achievementRef.transform.GetChild(0+1).GetComponent<Text>().text = Name ;
        }
           
             //SaveAchievement(false);
             unlocked = false;            
            if (maxProgression == 0)
            {
                return true;
            }
            if (currentProgression >= maxProgression)
            {
                return true; // This means current achievement has to be unlocked.
            }
            return false;
        }
    }
