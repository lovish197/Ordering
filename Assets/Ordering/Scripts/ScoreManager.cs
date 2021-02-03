using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ordering
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;
        public int currentScore;
        public int incrementScore;
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
            IncrementCurrentScore(incrementScore);
        }



    }

}
