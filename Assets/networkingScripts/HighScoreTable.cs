using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreTable : MonoBehaviour
{
    public Transform HighScoreCreator;
    public Transform HighScoreTemplate;

    private void Awake()
    {
        HighScoreTemplate.gameObject.SetActive(false);

        float templateHeight = 20f;

        for(int i = 0; i < 10; i++)
        {
            Transform entryTransform = Instantiate(HighScoreTemplate, HighScoreCreator);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);
        }
    }
}
