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
            int timeincrease = 5;
            int tempsec = (int)((min * 60) + Second) + timeincrease;
            min = tempsec / 60;
            Second = tempsec % 60;
        }
    }

}
