using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;

namespace Ordering
{

    public class LevelUI : MonoBehaviour
    {
        public static LevelUI instance;

        public Text Score_Text, GameOverScoreText, WonPanelSCoreText, Timer_Text, Question_Text;
        public GameObject Pause_Panel, Game_Over_Panel, WonPanel;
        public GameObject Disable_Touch_Object;
        public GameObject Correct_Answer_Time, WrongAnswerPanel,RightAnswer_TickMark,RightAnswerBoard,WrongAnswerBoard;
        public GameObject Answer_Number_Status; // It is used for showing the correct answer to user when user attempt wrong answer.
        public GameObject InsectEnemy;
        public string currentUserNumber;//Used to set the number of showing when player give correct answer.

        bool haswon;
        void OnEnable()
        {
            UserAnswer.OnGiveCorrectAnswer += Correct_Answer;
        }
        void OnDisable()
        {
            UserAnswer.OnGiveCorrectAnswer -= Correct_Answer;
        }


        private void Update()
        {
            if (!haswon)
            {
                if (Timer_Text != null && Ordering.Timer.instance != null)
                { Timer_Text.text = Ordering.Timer.instance.GetCurrentTime(); }

                if (Score_Text != null && Ordering.ScoreManager.instance != null)
                { Score_Text.text = Ordering.ScoreManager.instance.GetCurrentScore().ToString(); }

                if (GameOverScoreText != null && Ordering.ScoreManager.instance != null)
                { GameOverScoreText.text = "Score : " + Ordering.ScoreManager.instance.GetCurrentScore().ToString(); }

                if (WonPanelSCoreText != null && Ordering.ScoreManager.instance != null)
                { WonPanelSCoreText.text = "Score : " + Ordering.ScoreManager.instance.GetCurrentScore().ToString(); }



                if (Question_Text != null && Ordering.LevelQuestion.instance != null)
                {
                    QuestionTextInfo();
                }

                if (ScoreManager.instance.GetCurrentScore() > 200)
                {
                    haswon = true;
                    PlayerWon();
                }
            }

        }


        public void QuestionTextInfo()
        {
            string ques = "";
            switch (Ordering.LevelQuestion.instance.gameType)
            {
                case Ordering.LevelQuestion.GameType.Pick_Smallest: ques = "Choose The Smallest Number";
                    break;
                case Ordering.LevelQuestion.GameType.Pick_Largest: ques = "Choose The Largest Number";
                    break;
                case Ordering.LevelQuestion.GameType.Make_Smallest: ques = "Make a Smallest Number";
                    break;
                case Ordering.LevelQuestion.GameType.Make_Largest: ques = "Make a Largest Number";
                    break;
                case Ordering.LevelQuestion.GameType.Ascending_Order: ques = "Choose in Ascending Order";
                    break;
                case Ordering.LevelQuestion.GameType.Descending_Order: ques = "Choose in Descending Order";
                    break;
                case Ordering.LevelQuestion.GameType.GreaterThan: ques = "Chosse number greater than " + LevelQuestion.instance.first_num;
                    break;
                case Ordering.LevelQuestion.GameType.LessThan: ques = "Chosse number less than " + LevelQuestion.instance.first_num;
                    break;
                case Ordering.LevelQuestion.GameType.RangeBetween: ques = "Chosse number between " + LevelQuestion.instance.first_num + " & " + LevelQuestion.instance.second_num;
                    break;

                default: ques = "Please Do Correct Question Text";
                    break;
            }

            Question_Text.text = ques;
        }
        private void Awake()
        {
            instance = this;
        }
        public void Resume()
        {
            Pause_Panel.SetActive(false);
            Disable_Touch_Object.gameObject.SetActive(false);
            Ordering.Timer.instance.GamePause = false;
        }
        public void Pause()
        {
            print("user wanna to pause");
            Pause_Panel.SetActive(true);
            Disable_Touch_Object.gameObject.SetActive(true);
            Ordering.Timer.instance.GamePause = true;

        }
        public void GameOver()
        { if (Game_Over_Panel != null)
                Game_Over_Panel.SetActive(true);
            StartCoroutine(main.Instance.Web.UpdateUserScore(login.UserName, ScoreManager.instance.GetCurrentScore())); // update the score to database
        }

        public void PlayerWon()
        { if (WonPanel != null)
                WonPanel.SetActive(true);
            StartCoroutine(main.Instance.Web.UpdateUserScore(login.UserName, ScoreManager.instance.GetCurrentScore())); // update the score to database

            if (AchievementData.instance != null)
                AchievementData.instance.InsertAchievementOnLevelWin();
            Timer.instance.GamePause = true;
        }


        public void Restart()
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            StartCoroutine(Enum_LoadScene(SceneManager.GetActiveScene().buildIndex));
        }

        public void Home()
        {
            //SceneManager.LoadScene("MainMenu");
            StartCoroutine(Enum_LoadScene(0));
        }
        public void PlayNextLevel()
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(Enum_LoadScene(SceneManager.GetActiveScene().buildIndex+1));

        }
        public void Correct_Answer()
        {
         
            if (Correct_Answer_Time != null)
            {
                Correct_Answer_Time.SetActive(false);
                Correct_Answer_Time.SetActive(true);
            }
            //if (RightAnswer_TickMark != null)
            //{
            //    RightAnswer_TickMark.SetActive(false);
            //    RightAnswer_TickMark.SetActive(true);
            //}
            if (RightAnswerBoard != null)
            {
                RightAnswerBoard.SetActive(false);
                RightAnswerBoard.SetActive(true);
            }

            CreaterSpawner.instance.ResetAllThingTillCreated();
            SetMaximumCharacterForDificultyLevel();
            LevelQuestion.instance.GetOptions();

            if (AchievementData.instance != null)
                AchievementData.instance.InsertAchievementForRightAnswer();

        }

        void SetMaximumCharacterForDificultyLevel()
        {
            int max_difficulty=2;
            //if (ScoreManager.instance.currentScore > 100) { max_difficulty = 3; }
            if(ScoreManager.instance.currentScore>=100)
            max_difficulty = ScoreManager.instance.currentScore / 50 + 1;
            LevelQuestion.instance.Max_Options = max_difficulty;
        }
        public void WrongAnswer()
        {
            StartCoroutine(Wrong());
            
        }

        IEnumerator Wrong()
        {
         
            //if (WrongAnswerPanel != null) { WrongAnswerPanel.SetActive(false); WrongAnswerPanel.SetActive(true); }
            if (WrongAnswerBoard != null)
            {
                WrongAnswerBoard.SetActive(false);
                WrongAnswerBoard.SetActive(true);
            }

            ItemMoveController[] k = GameObject.FindObjectsOfType<ItemMoveController>();
            foreach (var item in k)
            {
                item.canCheckNeighour = false;
                item.GetComponent<BoxCollider2D>().enabled = false;
                item.movingright = true;
                //item.speed = Math.Abs(item.speed);
                item.speed = 0f;

            }

            yield return new WaitForSeconds(0.2f);//2f

            //if (WrongAnswerPanel != null) WrongAnswerPanel.SetActive(false);

            InsectEnemy.SetActive(false);
            GameObject.Find("DestroyParticleOnWrongAnswer").GetComponent<ParticleSystem>().Play();

            SetAllCharacterToWalk();

            yield return new WaitForSeconds(2f);
            InsectEnemy.SetActive(true);

            CreaterSpawner.instance.ResetAllThingTillCreated();


            LevelQuestion.instance.GetOptions();


        }

        void SetAllCharacterToWalk()
        {
            ItemMoveController[] k = GameObject.FindObjectsOfType<ItemMoveController>();
            foreach (var item in k)
            {
                item.canCheckNeighour = false;
                item.GetComponent<BoxCollider2D>().enabled = false;
                item.movingright = true;
                //item.speed = Math.Abs(item.speed);
                item.speed = 6f;
                
            }



        }




        public void ShowAnswerStatus(string _ans_status)
        {
            string answer = LevelQuestion.instance.answer_for_sequence;
            if (Answer_Number_Status != null)
            {
                Answer_Number_Status.transform.GetChild(0).GetComponent<Text>().text = _ans_status + "\nNum was " + answer;

                Answer_Number_Status.SetActive(true);
                Invoke("Disable_NumberStatus", 1.5f);
            }
        }

        void Disable_NumberStatus()
        {
            Answer_Number_Status.SetActive(false);
            currentUserNumber = "";
        }
        IEnumerator Enum_LoadScene(int _sceneIndex)
        {
            Time.timeScale = 1;
            if (TreeTransition.instance != null)
                TreeTransition.instance.Close();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(_sceneIndex);
        }
    }

    public static class ExtensionMeth  
    {

        public static void SetActive(this GameObject _obj,bool _val, float _time,int obj)
        {
            _obj.SetActive(_val);
            //GameObject obj = _obj;
            //Task.Delay((int)_time * 1000).ContinueWith(t => bar(obj));
            Task.Delay((int)_time * 1000).ContinueWith(t => bar(_obj));
            //Task.Delay((int)_time*1000).ContinueWith((t) => { Debug.LogWarning("Yes  We are in right way");});
            
            //return _obj;
        }

        static void bar(int dobj)
        {
            Debug.LogWarning("Please Do Some thing");
            Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAA");
            Debug.LogWarning("Name is " +   dobj*2);
            Debug.LogWarning("BBBBBBBBBBBBBBBBBBBBB");

            //_obj.SetActive(false);
            
        }
        static void bar(GameObject dobj)
        {
            Debug.LogWarning("Please Do Some thing");
            Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAA");
            Debug.LogWarning("Name is " +   dobj.name);
            Debug.LogWarning("BBBBBBBBBBBBBBBBBBBBB");

            //_obj.SetActive(false);
            
        }

        static void bar(float time)
        {
            Debug.LogWarning("Please Do Some thing");
            Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAA");
            Debug.LogWarning("time is " + time*1000);
            Debug.LogWarning("BBBBBBBBBBBBBBBBBBBBB");

            //_obj.SetActive(false);
            
        }
    }    

}