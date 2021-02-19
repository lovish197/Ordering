using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ordering
{
    public class Timer : MonoBehaviour
    {
        public static Timer instance;
        [SerializeField]
        float min, Second = 60;
        [SerializeField]
        string currenttime;
        [HideInInspector]
        public bool GamePause;
        public RangeForTimeComparison[] rangeForTimeComparison;
        public float ansTimer = 10f;
        public int incrementScore;

        public bool timeup;

        void OnEnable()
        {
            UserAnswer.OnGiveCorrectAnswer += IncreaseTimer;
        }
        void OnDisable()
        {
            UserAnswer.OnGiveCorrectAnswer -= IncreaseTimer;
        }

        private void Awake()
        {
            instance = this;

        }
        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            ansTimer -= Time.deltaTime;
            if (!GamePause)
            {
                TimerSet();
            }
            if (timeup)
            {
                min = -1;
                timeup = false;
            }
        }

        void TimerSet()
        {
            Second -= Time.deltaTime;
            if (Second <= 1)
            {
                min--;
                Second = 59;
            }

            if (min < 0)
            {
                //WinLose.instance.timeup = true;
                //WinLose.instance.WonStatus();
                LevelUI.instance.GameOver();
                Debug.Log("timeup ho gya h");
                currenttime = "00 : 00";
                GamePause = true;
            }
            else
            {
                currenttime = min + " : " + Mathf.RoundToInt(Second).ToString();
            }
        }

        public string GetCurrentTime()
        {
            return currenttime;
        }

        public void IncreaseTimer()
        {
            // FOR NOW WE DONT NEED TO INCREASE TIME IF NEEDED JUST DO UNCOMMENT THE CODE BELOW

            /*int timeincrease = 5;
            int tempsec = (int)((min * 60) + Second) + timeincrease;
            min = tempsec / 60;
            Second = tempsec % 60;*/
        }

        //[ContextMenu("Check Increment Score")]
        public int incrementScoreDecider()
        {
            for(int i = 0; i < rangeForTimeComparison.Length; i++)
            {
                if (ansTimer > rangeForTimeComparison[i].from && ansTimer < rangeForTimeComparison[i].to)
                {
                    incrementScore = rangeForTimeComparison[i].result;
                    return incrementScore;
                }
            }
            return 0;
        }
    }

    [System.Serializable]public class RangeForTimeComparison
    {
        public float from, to;
        public int result;
        public RangeForTimeComparison(float to, float from)
        {
            this.to = to;
            this.from = from;
        }
    }
}
