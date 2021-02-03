using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ordering
{

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public AudioSource CorrectAnswer, WrongAnswer;
        void OnEnable()
        {
            UserAnswer.OnGiveCorrectAnswer += () => { CorrectAnswer.Play(); };
            UserAnswer.OnGiveWrongAnswer += () => { WrongAnswer.Play(); };
        }

        void OnDisable()
        {
            UserAnswer.OnGiveCorrectAnswer -= () => { CorrectAnswer.Play(); };
            UserAnswer.OnGiveWrongAnswer -= () => { WrongAnswer.Play(); };
        }

        void Awake()
        {
            instance = this;
        }
        
        
    }
}
