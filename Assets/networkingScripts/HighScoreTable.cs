using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreTable : MonoBehaviour
{
    public Transform HighScoreCreator;
    public Transform HighScoreTemplate;

    private void Awake()
    {
        HighScoreTemplate.gameObject.SetActive(false); // not active to avoid over lapping and
                                                       // necessary in canvas because of reference 

        float templateHeight = 40f;

        for(int i = 0; i < 4; i++)
        {
            Transform entryTransform = Instantiate(HighScoreTemplate, HighScoreCreator);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);
        }
    }
}
