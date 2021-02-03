using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Ordering
{
    public class UserAnswer : MonoBehaviour
    {
        public static UserAnswer instance;
        string ans_in_string;
        List<int> useransList;

        public static Action OnGiveCorrectAnswer;
        public static Action OnGiveWrongAnswer;

        private void Awake()
        {
            

            instance = this;
            OnGiveWrongAnswer = new Action(() => { });
            OnGiveCorrectAnswer = new Action(() => { });

            SetUserAnsDefault();
        }
        // Start is called before the first frame update
        void Start()
        {
            
            //SetUserAnsDefault();
        }

        //It is for Single click Answer like pick largest, pick smallest
        public void CheckUserAns(int ans)
        {
            if (ans == Ordering.LevelQuestion.instance.answer)
            {
                print("Correct Answer");
                CorrectAnswer();
            }
            else
            {
                print("Wrong Answer");
                WrongAnswer();
            }
        }

        /// <summary>
        /// This is used to check the answer when user have to select the anwer in some sequence, Valid only in Make Smallest, Make Largest
        /// </summary>
        /// <param name="ans"></param>
        public void CheckUserAns(string ans)
        { 
            ans_in_string += ans;
            int length = ans_in_string.Length;
            int rightanswer = 0;
            
            for (int i = 0; i < length; i++)
            {
                if (ans_in_string[i] == Ordering.LevelQuestion.instance.answer_for_sequence[i])
                {
                    rightanswer++;

                    if (length == rightanswer&&length ==Ordering.LevelQuestion.instance.answer_for_sequence.Length)
                    {
                        print("Correct Answer");
                        CorrectAnswer();
                    }
                     
                    
                }
                else
                {

                    print("Wrong_Answer");
                    WrongAnswer();
                    break;
                }
            }
            
        }



        public void CheckUserAnsForAscendingDescending(int ans)
        {
            //if (useransList != null) { useransList = new List<int>(); }
            useransList.Add(ans);
            int length = useransList.Count;
            int rightanswer = 0;
            
            for (int i = 0; i < length; i++)
            {
                if (useransList[i] == Ordering.LevelQuestion.instance.Total_Options[i])
                {
                    rightanswer++;

                    if (length == rightanswer && length ==Ordering.LevelQuestion.instance.Total_Options.Count)
                    {
                        print("Correct Answer");
                        CorrectAnswer();
                    }
                     
                    
                }
                else
                {

                    print("Wrong_Answer");
                    WrongAnswer();
                    break;
                }
            }
            
        }

        public void CheckAnsForGreaterThan(int _ans)
        {
            if (_ans > LevelQuestion.instance.first_num)
            {
                print("correctAnswer");
                if (IsAllCorrectAnsHasBeenColleced())
                {
                    print("Finally Correct Answer");
                    CorrectAnswer();
                }
            }
            else
            {
                print("Wrong Answer");
                WrongAnswer();
            }
        }
        public void CheckAnsForLessThan(int _ans)
        {
            if (_ans < LevelQuestion.instance.first_num)
            {
                print("correctAnswer");
                if (IsAllCorrectAnsHasBeenColleced(2))
                {
                    print("Finally Correct Answer");
                    CorrectAnswer();
                }
            }
            else
            {
                print("Wrong Answer");
                WrongAnswer();
            }
        }
        public void CheckAnsForRange(int _ans)
        {
            if (_ans > LevelQuestion.instance.first_num && _ans < LevelQuestion.instance.second_num )
            {
                print("correctAnswer");
                if (IsAllCorrectAnsHasBeenColleced(2,1))
                {
                    print("Finally Correct Answer");
                    CorrectAnswer();
                }
            }
            else
            {
                print("Wrong Answer");
                WrongAnswer();
            }
        }

        bool IsAllCorrectAnsHasBeenColleced()
        {
            Item[] allitems = GameObject.FindObjectsOfType<Item>();
            int largenumber = LevelQuestion.instance.first_num;
            for (int i = 0; i < allitems.Length; i++)
            {
                if (allitems[i].number > largenumber && !allitems[i].hasBeenClicked)
                {
                    return false;
                }
            }
            return true;
        }
        bool IsAllCorrectAnsHasBeenColleced(int g)
        {
            Item[] allitems = GameObject.FindObjectsOfType<Item>();
            int Smallnumber = LevelQuestion.instance.first_num;
            for (int i = 0; i < allitems.Length; i++)
            {
                if (allitems[i].number < Smallnumber && !allitems[i].hasBeenClicked)
                {
                    return false;
                }
            }
            return true;
        }

        bool IsAllCorrectAnsHasBeenColleced(int g,int s)
        {
            Item[] allitems = GameObject.FindObjectsOfType<Item>();
            int Smallnumber = LevelQuestion.instance.first_num;
            int largenumber = LevelQuestion.instance.second_num;
            for (int i = 0; i < allitems.Length; i++)
            {
                if (allitems[i].number > Smallnumber && allitems[i].number < largenumber && !allitems[i].hasBeenClicked)
                {
                    return false;
                }
            }
            return true;
        }

        public void SetUserAnsDefault()
        {
            ans_in_string = "";
            useransList = new List<int>();
            
        }

        public void CorrectAnswer()
        {
            SetUserAnsDefault();
            //Ordering.GameManager.instance.PlayerGiveCorrectAnswer();
            OnGiveCorrectAnswer?.Invoke();
        }

        public void WrongAnswer()
        {
            SetUserAnsDefault();
            LevelUI.instance.WrongAnswer();
            OnGiveWrongAnswer?.Invoke();
            //Ordering.GameManager.instance.PlayerGiveWrongAnswer();

        }

    }

    
}
