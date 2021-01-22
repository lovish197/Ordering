using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
namespace Ordering
{
    public class LevelQuestion : MonoBehaviour
    {
        public static LevelQuestion instance;

        #region Public Variable
        public int answer;
        public string answer_for_sequence;
        [Tooltip("Below two are useful only for\n Greater than\nLess than\nRangeBetween")]
        public int first_num, second_num;
        [Tooltip("Maximum number of options")]
        public int Max_Options;
        public bool Random_Question;
        public enum MixGameType {No_Mix, Mix_Pick, Mix_Make, Mix_AscenDescen, MixGreatLess }
        public MixGameType mixGameType;
        public enum GameType { Pick_Smallest, Pick_Largest, Make_Smallest, Make_Largest, Ascending_Order, Descending_Order,GreaterThan,LessThan,RangeBetween }
        public GameType gameType;
        public enum OptionTpye { Simple_Random, Same_Digit_count, Combination_Of_A_Number, Comb_Of_A_Number_Other_number };
        public OptionTpye optionTpye;

        public enum NumberOfDigit {Single_Digit=1,Two_Digit=2,Three_Digit=3,Four_Digit=4 }
        public NumberOfDigit numerOfDigit;
        public List<int> Total_Options;

        public static Action OnQuestionGenerate;
        #endregion

        #region Monobehaviour Functions
        public void Awake()
        {
            instance = this;
        }
        private void Start()
        {
           
            GetOptions();
            

        }

        //private void SetDataFromDataManager()
        //{
        //    switch (DataManager.Number__Of_Digit)
        //    {
        //        case DataManager.DigitsCount._2_digit:
        //            numerOfDigit = NumberOfDigit.Two_Digit;
        //            break;
        //        case DataManager.DigitsCount._3_digit:
        //            numerOfDigit = NumberOfDigit.Three_Digit;
        //            break;
        //        case DataManager.DigitsCount._4_digit:
        //            numerOfDigit = NumberOfDigit.Four_Digit;
        //            break;
        //        case DataManager.DigitsCount._5_digit:
        //            numerOfDigit = NumberOfDigit.Four_Digit;
        //            break;

        //        default:
        //            numerOfDigit = NumberOfDigit.Two_Digit;
        //            break;
        //    }
        //}
        #endregion

        #region Public Methods
        [ContextMenu("GetOptions")]
        public  void GetOptions()
        {
            if (Random_Question)
            {
                SetRandomQuestion();
            }
            Total_Options.Clear();


            if (mixGameType != MixGameType.No_Mix) { SetMixQuestionAsPerGamType(); }


            if (gameType == GameType.Pick_Largest || gameType == GameType.Pick_Smallest)
            {
                switch (optionTpye)
                {
                    case OptionTpye.Simple_Random:
                        Total_Options = Get_Options_With_Different_Numbers();
                        break;
                    case OptionTpye.Same_Digit_count:
                        Total_Options = Get_Options_With_Same_Digits();

                        break;
                    case OptionTpye.Combination_Of_A_Number:
                        Total_Options = Get_Options_With_Combination();


                        break;
                    case OptionTpye.Comb_Of_A_Number_Other_number:
                        Total_Options = Get_Options_With_Combination_Different_number();
                        break;
                    default:
                        Total_Options = Get_Options_With_Different_Numbers();
                        break;
                }


                answer = GetAnswer();
                answer_for_sequence = answer.ToString();
            }

            if (gameType == GameType.Ascending_Order || gameType == GameType.Descending_Order)
            {
                answer = 1;

                switch (numerOfDigit)
                {
                    case NumberOfDigit.Single_Digit: Total_Options = Get_Options_With_Single_Digit(); break;

                    case NumberOfDigit.Two_Digit:
                        Total_Options = Get_Options_With_Two_Digit();



                        break;
                    case NumberOfDigit.Three_Digit:
                        Total_Options = Get_Options_With_Three_Digit();

                        break;
                    case NumberOfDigit.Four_Digit:
                        Total_Options = Get_Options_With_Four_Digit();

                        break;
                    default:
                        Total_Options = Get_Options_With_Single_Digit();

                        break;
                }
                LimitOfOptions();

                Total_Options.Sort();
                if (gameType == GameType.Descending_Order)
                { Total_Options.Reverse(); }

            }

            if (gameType == GameType.Make_Smallest || gameType == GameType.Make_Largest)
            {
               Total_Options = Get_Options_With_Make_Number();
                answer = int.Parse(answer_for_sequence);
            }

            if (gameType == GameType.RangeBetween)
            {
                switch (numerOfDigit)
                {
                    case NumberOfDigit.Single_Digit: Total_Options = Get_Options_With_Two_Digit_ForRange();
                        break;
                    case NumberOfDigit.Two_Digit:   Total_Options = Get_Options_With_Two_Digit_ForRange();
                        break;
                    case NumberOfDigit.Three_Digit: Total_Options = Get_Options_With_Three_Digit_ForRange();
                        break;
                    default:
                        Total_Options = Get_Options_With_Two_Digit_ForRange();
                        break;
                }
                LimitOfOptions();
            }
            if (gameType == GameType.GreaterThan || gameType == GameType.LessThan)
            {
                switch (numerOfDigit)
                {
                    case NumberOfDigit.Single_Digit: Total_Options = Get_Options_With_Double_Digit_ForGreterLess();
                        break;
                    case NumberOfDigit.Two_Digit:   Total_Options = Get_Options_With_Double_Digit_ForGreterLess();
                        break;
                    case NumberOfDigit.Three_Digit: Total_Options = Get_Options_With_Three_Digit_ForGreterLess();
                        break;
                    default:
                        Total_Options = Get_Options_With_Double_Digit_ForGreterLess();
                        break;
                }
                LimitOfOptions();
            }

        

            OnQuestionGenerate?.Invoke();

        }

        void SetMixQuestionAsPerGamType()
        {
            if (mixGameType == MixGameType.Mix_Pick) { int r = UnityEngine.Random.Range(0, 2); if (r == 1) { gameType = GameType.Pick_Largest; } else { gameType = GameType.Pick_Smallest; }  }
            if (mixGameType == MixGameType.Mix_Make) { int r = UnityEngine.Random.Range(0, 2); if (r == 1) { gameType = GameType.Make_Smallest; } else { gameType = GameType.Make_Largest; }  }
            if (mixGameType == MixGameType.Mix_AscenDescen) { int r = UnityEngine.Random.Range(0, 2); if (r == 1) { gameType = GameType.Ascending_Order; } else { gameType = GameType.Descending_Order; }  }
            if (mixGameType == MixGameType.MixGreatLess) { int r = UnityEngine.Random.Range(0, 2); if (r == 1) { gameType = GameType.GreaterThan; } else { gameType = GameType.LessThan; } }
        }


        public List<int> Get_Options_With_Double_Digit_ForGreterLess()
        {
            List<int> number_list = new List<int>();
            first_num = UnityEngine.Random.Range(40, 60);
            int total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num+1,first_num+40 );

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }

            total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num-40, first_num);

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }
                
            

            return number_list;
        }

        public List<int> Get_Options_With_Three_Digit_ForGreterLess()
        {
            List<int> number_list = new List<int>();
            first_num = UnityEngine.Random.Range(401, 600);
            int total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num+10,first_num+400 );

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }

            total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num-400, first_num-2);

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }
                
            

            return number_list;
        }

        public List<int> Get_Options_With_Three_Digit_ForRange()
        {
            List<int> number_list = new List<int>();
            first_num = UnityEngine.Random.Range(201, 500);
            second_num = UnityEngine.Random.Range(701, 800);
            int total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num+10,second_num );

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }

            total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num-100, first_num-2);

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }
                
            

            return number_list;
        }

        public List<int> Get_Options_With_Two_Digit_ForRange()
        {
            List<int> number_list = new List<int>();
            first_num = UnityEngine.Random.Range(21, 50);
            second_num = UnityEngine.Random.Range(70, 80);
            int total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num+1,second_num );

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }

            total_option = UnityEngine.Random.Range(2, 4);
            for (int i = 0; i < total_option; i++)
            {
                    Again:

                int rand = UnityEngine.Random.Range(first_num-10, first_num-2);

                if (number_list.Contains(rand))
                    goto Again;
                else number_list.Add(rand);
            }
                
            

            return number_list;
        }







        public int GetAnswer()
        {
            int ans = 0;
            List<int> all_option = new List<int>();
            LimitOfOptions();

            for (int i = 0; i < Total_Options.Count; i++)
            {
                all_option.Add(Total_Options[i]);
            }
            all_option.Sort();
            switch (gameType)
            {
                case GameType.Pick_Smallest:
                    ans = all_option[0];
                    break;
                case GameType.Pick_Largest:
                    ans = all_option[all_option.Count - 1];

                    break;
                case GameType.Make_Smallest:
                    break;
                case GameType.Make_Largest:
                    break;
                default:
                    break;
            }
            return ans;

        }

        /// <summary>
        /// Used to limit the total options
        /// </summary>
        private void LimitOfOptions()
        {
            while (Total_Options.Count > Max_Options)
            {
                Total_Options.RemoveAt(Total_Options.Count - 1);
            }
        }

        void SetRandomQuestion()
        {
            int r = UnityEngine.Random.Range(1, 5);
            switch (r)
            {
                case 1: gameType = GameType.Pick_Smallest; break;
                case 2: gameType = GameType.Pick_Largest;  break;
                case 3: gameType = GameType.Make_Smallest; break;
                case 4: gameType = GameType.Make_Smallest; break;
                default:gameType = GameType.Make_Largest;  break;
            }

            r = UnityEngine.Random.Range(2, 5);
            switch (r)
            {
                case 1:optionTpye =   OptionTpye.Simple_Random; break;
                case 2:optionTpye =   OptionTpye.Same_Digit_count; break;
                case 3:optionTpye =   OptionTpye.Combination_Of_A_Number; break;
                case 4:optionTpye =   OptionTpye.Comb_Of_A_Number_Other_number; break;
                default: optionTpye = OptionTpye.Simple_Random; break;
            }

            r = UnityEngine.Random.Range(1, 5);
            switch (r)
            {
                case 1: numerOfDigit = NumberOfDigit.Single_Digit; break;
                case 2: numerOfDigit = NumberOfDigit.Two_Digit; break;
                case 3: numerOfDigit = NumberOfDigit.Three_Digit; break;
                case 4: numerOfDigit = NumberOfDigit.Four_Digit; break;
                default: numerOfDigit = NumberOfDigit.Single_Digit; break;

            }
        }

        public List<int> Get_Options_With_Same_Digits()
        {
            List<int> number_list = new List<int>();

            

            switch (numerOfDigit)
            {
                case NumberOfDigit.Single_Digit:
                    number_list = Get_Options_With_Single_Digit();
                    break;
                case NumberOfDigit.Two_Digit:
                    number_list = Get_Options_With_Two_Digit();
                    break;
                case NumberOfDigit.Three_Digit:
                    number_list = Get_Options_With_Three_Digit();
                    break;
                case NumberOfDigit.Four_Digit:
                    number_list = Get_Options_With_Four_Digit();
                    break;

                default:
                    number_list = Get_Options_With_Single_Digit();
                    break;
            }



            return number_list;
        }

        public List<int> Get_Options_With_Combination()
        {
            List<int> number_list = new List<int>();
            List<int> temp = new List<int>();
            int r=0;
            switch (numerOfDigit)
            {

                case NumberOfDigit.Two_Digit:
                    number_list = PrintCombination(UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10));
                    
                    break;
                    

                case NumberOfDigit.Three_Digit:
                    number_list = PrintCombination(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(0, 10));
                    r = UnityEngine.Random.Range(3, 7);
                    //Options(r);
                    break;

                case NumberOfDigit.Four_Digit:
                    number_list = PrintCombination(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10));
                    r = UnityEngine.Random.Range(4, 7);
                    //Options(r);
                    break;

                default:
                    number_list = PrintCombination(UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10)); break;

            }
            void Options(int h)
            {
                
                for (int i = 0; i < h; i++)
                { int j = UnityEngine.Random.Range(0, number_list.Count);
                    temp.Add(number_list[j]);
                    number_list.RemoveAt(j);
                }
            }
            //if (numerOfDigit == NumberOfDigit.Two_Digit|| numerOfDigit == NumberOfDigit.Single_Digit)
            //{ return number_list; }
            //else return temp;
            return number_list;

        }


        public List<int> Get_Options_With_Make_Number()
        {
            int max_value=0;
            List<int> all_options = new List<int>();
            
            switch (numerOfDigit)
            {
                
                case NumberOfDigit.Two_Digit: max_value = 2; break;
                    
                case NumberOfDigit.Three_Digit:
                    max_value = 3; break;

                case NumberOfDigit.Four_Digit:
                    max_value = 4; break;

                default:
                    max_value = 2; break;

            }
            AGAIN_QUESTION_RELOAD:
            all_options.Clear();
            for (int i = 0; i <max_value; i++)
            {
                all_options.Add(UnityEngine.Random.Range(0, 10));
            }
            int samenumber = 0;

            for (int i = 0; i < all_options.Count-1; i++)//checking here all options are same or not
            {
                if (all_options[i] == all_options[i + 1])
                { samenumber++; }
            }
            if (samenumber == all_options.Count-1) goto AGAIN_QUESTION_RELOAD; //if all options are same then question should be reload again

            List<int> k = new List<int>();
            //k = PrintCombination(all_options.ToArray());
            for (int i = 0; i < all_options.Count; i++)
            {
                k.Add(all_options[i]);
                
            }

            k.Sort();
            for (int i = 0; i < k.Count; i++)
            {
                print(k[i]);
            }


            #region  //................This is used to get answer when we don't want zero at left most place.................//
            List<int> getcomb = PrintCombination(k.ToArray());
            getcomb.Sort();
            string temp_string="";
            for (int i = 0; i < getcomb.Count; i++)
            {

                if (getcomb[i].ToString().Length == max_value)
                {
                    temp_string = getcomb[i].ToString();
                    break;
                }

            }
            //......................................................................................................//
            #endregion
            string g = "";
            for (int i = 0; i < k.Count; i++)
            {
                g += k[i].ToString();
            }

            

            switch (gameType)
            {
                
                case GameType.Make_Smallest: //answer = k[0];
                    //answer = int.Parse(g);
                    //answer_for_sequence = g ;
                    answer_for_sequence = temp_string ;
                    break;
                case GameType.Make_Largest:
                    //answer = k[k.Count-1];
                    //answer = int.Parse(ReverseString(g));
                    answer_for_sequence = ReverseString(g);
                    //answer_for_sequence = ReverseString(getcomb[getcomb.Count-1].ToString());
                    //answer_for_sequence = getcomb[getcomb.Count - 1].ToString();

                    break;
                
            }

            string  ReverseString(string str)
            {
                string s="";
                for (int i = str.Length-1; i >= 0; i--)
                {
                    s += str[i];
                }
                return s;
            }

            return all_options;
        }

        [ContextMenu("Find Combination")]
        void G()
        {
            List<int> l = new List<int>();
            l = PrintCombination(new int[] { 3,0,5});
            for (int i = 0; i < l.Count; i++)
            {
                print(l[i]);
            }
            
        }


        public  List<int> PrintCombination(params int []num)
        {
            List<int> number_list = new List<int>();
            string temp_="";
            
            int j, i;
            //int len = num.Length;
            //int n = UnityEngine.Random.Range(3, 7);
            int n = num.Length;
            int temp;
            for (j = 1; j <= n; j++)
            {
                for (i = 0; i < n - 1; i++)
                {
                    
                    temp = num[i];
                    num[i] = num[i + 1];
                    num[i + 1] = temp;
                    //temp_ += num[i].ToString();
                    //if ((i+1) == (n - 1)) temp_ += num[i + 1].ToString();
                    for (int w = 0; w < n; w++)
                    {
                        temp_ += num[w].ToString();
                    }

                    number_list.Add(int.Parse(temp_));
                    temp_ = "";
                }
                
            }

            List<int> distinct = number_list.Distinct().ToList();

            return distinct;
        }
        


        public List<int> Get_Options_With_Different_Numbers()
        {
            List<int> number_list =new List<int>();
            int total_option = UnityEngine.Random.Range(2, 6);
            for (int i = 0; i < total_option; i++)
            {
                Again:
                int rand = UnityEngine.Random.Range(1, 9999);

                if (number_list.Contains(rand)) goto Again;
                else number_list.Add(rand);

            }


            return number_list;
        }

        public List<int> Get_Options_With_Single_Digit()
        {
            List<int> number_list = new List<int>();
            int total_option = UnityEngine.Random.Range(2, 6);
            for (int i = 0; i < total_option; i++)
            {
                Again:
                int rand = UnityEngine.Random.Range(1,10);

                if (number_list.Contains(rand)) goto Again;
                else number_list.Add(rand);

            }


            return number_list;
        }

        public List<int> Get_Options_With_Two_Digit()
        {
            List<int> number_list = new List<int>();
            int total_option = UnityEngine.Random.Range(2, 6);
            for (int i = 0; i < total_option; i++)
            {
                Again:
                int rand = UnityEngine.Random.Range(10,100);

                if (number_list.Contains(rand)) goto Again;
                else number_list.Add(rand);

            }


            return number_list;
        }

        public List<int> Get_Options_With_Three_Digit()
        {
            List<int> number_list = new List<int>();
            int total_option = UnityEngine.Random.Range(2, 6);
            for (int i = 0; i < total_option; i++)
            {
                Again:
                int rand = UnityEngine.Random.Range(100,1000);

                if (number_list.Contains(rand)) goto Again;
                else number_list.Add(rand);

            }


            return number_list;
        }

        public List<int> Get_Options_With_Four_Digit()
        {
            List<int> number_list = new List<int>();
            int total_option = UnityEngine.Random.Range(2,6);
            for (int i = 0; i < total_option; i++)
            {
                Again:
                int rand = UnityEngine.Random.Range(1000,10000);

                if (number_list.Contains(rand)) goto Again;
                else number_list.Add(rand);

            }


            return number_list;
        }

        public List<int> Get_Options_With_Combination_Different_number()
        {
            List<int> number_list = new List<int>();
            List<int> list1 = new List<int>();
            list1 =  Get_Options_With_Same_Digits();
            List<int> list2 = new List<int>();
            list2 = Get_Options_With_Combination();

            List<int> temp = new List<int>();
            for (int i = 0; i < list1.Count; i++)
            {
                temp.Add(list1[i]);
            }
            for (int i = 0; i < list2.Count; i++)
            {
                if (!temp.Contains(list2[i]))
                {
                    temp.Add(list2[i]);
                }
                
            }
            int total_option=0;
            if (temp.Count > 7) { total_option = UnityEngine.Random.Range(3, 7); }

            else { total_option = UnityEngine.Random.Range(3, total_option); }    

            
            for (int i = 0; i < total_option; i++)
            {

                int rand_index = UnityEngine.Random.Range(0, temp.Count);

                number_list.Add(temp[rand_index]);
                temp.RemoveAt(rand_index);


            }


            return number_list;
        }

        #endregion

    }//End of Class

}

