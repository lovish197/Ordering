using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameSettings : MonoBehaviour
{
    public static  GameSettings instance;
    public static bool IsSoundOn;
    public Sprite SoundOn,SoundOff;
    public Image AudioButtonImage;
    public static bool hogyasound;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] AllSource = GameObject.FindObjectsOfType<AudioSource>();



        if (!hogyasound) { IsSoundOn = true; hogyasound = true; }
        


        if (IsSoundOn)
        {
            foreach (var item in AllSource)
            {
                item.volume = 1f;
            }
            if (AudioButtonImage!= null)
            {
                AudioButtonImage.sprite = SoundOn;
            }
        }
        else
        {
            foreach (var item in AllSource)
            {
                item.volume = 0f;
            }
            if (AudioButtonImage != null)
            {
                AudioButtonImage.sprite = SoundOff;
            }
        }

        

        
    }

    [ContextMenu("SoundSetting")]
   public  void SoundSetting()
    {
        IsSoundOn = !IsSoundOn;
        
        
        AudioSource[] AllSource = GameObject.FindObjectsOfType<AudioSource>();
        if (IsSoundOn)
        {
            foreach (var item in AllSource)
            {
                item.volume = 1f;
            }
            if (AudioButtonImage != null)
            {

                AudioButtonImage.sprite = SoundOn;
            }
        }
        else
        {
            foreach (var item in AllSource)
            {
                item.volume = 0f;
            }
            if (AudioButtonImage != null)
            {

                AudioButtonImage.sprite = SoundOff;
            }
        }

        
    }

    
}
