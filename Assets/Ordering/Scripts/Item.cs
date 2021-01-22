using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Ordering
{

    public class Item : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro NumberText;
        public int number;
        public bool hasBeenClicked;
        public SpriteRenderer Sr;
        public ItemMoveController itemMoveController;
        public Animator MushroomAnim;
        private AudioSource PickUpAudio;

        float start_fade_value;
        // Start is called before the first frame update
        void Start()
        {
            start_fade_value = 0f;
            PickUpAudio = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!Timer.instance.GamePause)
            {
                MushroomAnim.enabled = true;
                NumberText.text = number.ToString();
                Sr.flipX = !itemMoveController.movingright;
            }
            else
            {
                MushroomAnim.enabled = false;
            }

            if (start_fade_value <= 1f)
            {

                start_fade_value += Time.deltaTime;

                Color _c = transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>().color;
                _c.a = start_fade_value;
                transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>().color = _c;
            }

        }


        void OnMouseDown()
        {
            if (!Timer.instance.GamePause)
            {
                //print("Yes Mouse Down");
                hasBeenClicked = true;
                itemMoveController.canmove = false;
                itemMoveController.moveToInsectMouth = true;
                MushroomAnim.SetTrigger("die");
                if (PickUpAudio != null) PickUpAudio.Play();

            //    CheckAnswer();
            }
            
        }


      public  void CheckAnswer()
        {

            switch (LevelQuestion.instance.gameType)
            {
                case LevelQuestion.GameType.Pick_Smallest:      CheckAnswerDirect(number);
                    break;
                case LevelQuestion.GameType.Pick_Largest:       CheckAnswerDirect(number);
                    break;
                case LevelQuestion.GameType.Make_Smallest:      CheckAnswerForMakeSmallestLargest(number);
                    break;
                case LevelQuestion.GameType.Make_Largest:       CheckAnswerForMakeSmallestLargest(number);
                    break;
                case LevelQuestion.GameType.Ascending_Order:    CheckAnswerForAscendingDescending(number);
                    break;
                case LevelQuestion.GameType.Descending_Order:   CheckAnswerForAscendingDescending(number);
                    break;
                case LevelQuestion.GameType.GreaterThan:        CheckAnswerForGreaterthan(number);
                    break;
                case LevelQuestion.GameType.LessThan:           CheckAnswerForLessthan(number);
                    break;
                case LevelQuestion.GameType.RangeBetween:       CheckAnswerForRange(number);
                    break;
                default:                                        CheckAnswerDirect(number);
                    break;
            }

        }
                    

        void CheckAnswerDirect(int _num)
        {
            UserAnswer.instance.CheckUserAns(_num);
        }
        void CheckAnswerForMakeSmallestLargest(int _num)
        {
            UserAnswer.instance.CheckUserAns(_num.ToString());
        }
        void CheckAnswerForAscendingDescending(int _num)
        {
            UserAnswer.instance.CheckUserAnsForAscendingDescending(_num);
        }
        void CheckAnswerForGreaterthan(int _num)
        {
            UserAnswer.instance.CheckAnsForGreaterThan(_num);
        }
        void CheckAnswerForLessthan(int _num)
        {
            UserAnswer.instance.CheckAnsForLessThan(_num);
        }
        void CheckAnswerForRange(int _num)
        {
            UserAnswer.instance.CheckAnsForRange(_num);
        }

    }
}
