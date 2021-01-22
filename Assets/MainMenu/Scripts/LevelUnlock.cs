using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour
{
    public GameObject AllTopics;
    public GameObject Buttons_Parent;
    [HideInInspector]
    public Button[] Level_Button; //Used for eggs

    public Button[] Topic_Button; //Used for eggs
    
    public Sprite Unlock_Sprite, Locked_sprite,Golden_Sprite;
    public int temporary_LevelReached;
    public int levelStartIndex;//Used to set the first index for first egg as per build index
    public Color Font_Color;

    Button LastEggButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        Level_Button = new Button[Buttons_Parent.transform.childCount];
        Topic_Button = new Button[AllTopics.transform.childCount];
        //temporary_LevelReached = GameObject.FindObjectOfType<TimeManager>().current_day; // This is used when unlock system work acc. to days.
        
        //temporary_LevelReached = PlayerPrefs.GetInt("LevelReached", 0) + 1; //This is used when level unlock system work from wining the levels.
        for (int i = 0; i < Buttons_Parent.transform.childCount; i++)
        {
            Level_Button[i] = Buttons_Parent.transform.GetChild(i).GetComponent<Button>();
            Level_Button[i].GetComponentInChildren<Text>().color = Font_Color;
        }
        for (int i = 0; i < AllTopics.transform.childCount; i++)
        {
            Topic_Button[i] = AllTopics.transform.GetChild(i).GetComponent<Button>();
            Topic_Button[i].gameObject.SetActive(false);
            //Topic_Button[i].onClick.RemoveListener(() => GameObject.FindObjectOfType<Menu>().LoadTheSceneByIndex(0));
        }

         ButttonStatus();

    }

    [ContextMenu("Reset Level_Locker")]
    public void ButttonStatus()
    {

        for (int i = 0; i < Level_Button.Length; i++)
        {
            int num =  i;
            Level_Button[i].onClick.AddListener(() => LevelSelect(num));
            Level_Button[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
            Level_Button[i].GetComponentInChildren<Text>().fontSize = 45;
            Level_Button[i].GetComponentInChildren<Text>().color = Font_Color;

            int k = levelStartIndex + i;
            //Topic_Button[i].onClick.AddListener(() => GameObject.FindObjectOfType<Menu>().LoadTheSceneByIndex(k));
            //Level_Button[i].GetComponent<ButtonPuzzle>().SetInfo(QuestionManager.instance.all_previous_question[firstlevelNumber + i]);
        }

        for (int i = 0; i < temporary_LevelReached; i++)
        {
            Level_Button[i].interactable = true;
            Level_Button[i].image.sprite = Unlock_Sprite;
            //Level_Button[i].image.color = Color.yellow;
            Level_Button[i].image.color = Color.white;
            Level_Button[i].GetComponentInChildren<Text>().enabled = true;
            if (i == temporary_LevelReached - 1)
            {
                Level_Button[i].image.sprite = Golden_Sprite;
            }
            //Level_Button[i].image.color = Color.yellow;


        }
        for (int i = temporary_LevelReached; i < Level_Button.Length; i++)
        {
            Level_Button[i].interactable = false;
            Level_Button[i].image.sprite = Locked_sprite;
            Level_Button[i].image.color = Color.white;
            Level_Button[i].GetComponentInChildren<Text>().enabled = false;

            //Level_Button[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    

    void LevelSelect(int levelnum)
    {
        //string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        //Data.instance.SetLevelNum(levelnum);
        //Menu.instance.StartTheLevel(LevelName);
        //Here We will load level acc to level button.
        Debug.Log("LevelNumber " + levelnum + " & Button Name " + name);
        
        

        StartCoroutine(DisablePreviousOpenEgg(levelnum));
        

    }

    IEnumerator DisablePreviousOpenEgg(int levelnum)
    {

        if (LastEggButtonPressed != null)
        {
            LastEggButtonPressed.gameObject.GetComponent<Animator>().SetTrigger("out");

            //yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0f);

            LastEggButtonPressed.gameObject.SetActive(false);
        }

        if (LastEggButtonPressed != Topic_Button[levelnum])
        {
            Topic_Button[levelnum].gameObject.SetActive(true); Topic_Button[levelnum].gameObject.GetComponent<Animator>().Rebind();
            Topic_Button[levelnum].gameObject.GetComponent<Animator>().SetTrigger("in");
            LastEggButtonPressed = Topic_Button[levelnum];
        }
        else
        {
            LastEggButtonPressed = null;
        }
        
        
        
    }

}
