using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DotVisual : MonoBehaviour
{
    public Transform CenterPoint;
    public Image[] AllDots;
    public Transform[] AllChapters;

    public int currentProgress;
    public Color EnableColor, DisableColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < AllChapters.Length; i++)
        {
            if (AllChapters[i].transform.position.x < CenterPoint.position.x)
            {
                currentProgress = i + 1;
            }
        }
        for (int i = 0; i < currentProgress; i++)
        {
            AllDots[i].color = EnableColor;
        }
        for (int i = currentProgress; i < AllDots.Length; i++)
        {
            AllDots[i].color = DisableColor;
        }

    }



}
