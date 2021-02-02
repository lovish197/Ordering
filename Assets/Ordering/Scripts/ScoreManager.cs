using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ordering
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;
        public int currentScore;
        void OnEnable()
        {
            UserAnswer.OnGiveCorrectAnswer += Correct_Answer;
        }
        void OnDisable()
        {
            UserAnswer.OnGiveCorrectAnswer -= Correct_Answer;
        }
        private void Awake()
        {
            instance = this;
            // code here to get the current score from database with respect to the logged in user 
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }

        public void IncrementCurrentScore(int score_)
        {
            currentScore += score_;

        }

        void Correct_Answer()
        {
            IncrementCurrentScore(Timer.instance.incrementScoreDecider()); // return increment score as per time
            Timer.instance.ansTimer = 10f;

            // for trial purpose
        }



    }

}
