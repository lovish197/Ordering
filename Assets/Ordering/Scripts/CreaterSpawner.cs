using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ordering
{

    public class CreaterSpawner : MonoBehaviour
    {
        public static CreaterSpawner instance;

        public GameObject []ItemPrefab;
        public Transform[] AllPoints;
        public List<bool> AllPointsValue;
        GameObject prefab;

        List<Item> Allitems;
        void Awake()
        {
            instance = this;
            GameObject.Find("foreground").GetComponent<SpriteRenderer>().sortingOrder = 17;
            Allitems = new List<Item>();
            //prefab = ItemPrefab;
            AssignRandomPrefab();
            AllPointsValue = new List<bool>(new bool[AllPoints.Length]);
        }

        private void AssignRandomPrefab()
        {
            int r = UnityEngine.Random.Range(0, ItemPrefab.Length);
            prefab = ItemPrefab[r];
        }

        void Start()
        {
            
        }


        void OnEnable()
        {
            LevelQuestion.OnQuestionGenerate += CreateRedFlower;
        }
        void OnDisable()
        {
            LevelQuestion.OnQuestionGenerate -= CreateRedFlower;
        }

        public void ResetAllThingTillCreated()
        {
            DestroyAllItemsFromScene();
            for (int i = 0; i < AllPointsValue.Count; i++)
            {
                AllPointsValue[i] = false;
            }
        }


        private void DestroyAllItemsFromScene()
        {
            
            for (int i = 0; i <Allitems.Count; i++)
            {if(Allitems[i]!=null)
                Destroy(Allitems[i].gameObject);
            }

           
        }
        void CreateRedFlower()
        {
            for (int i = 0; i < AllPointsValue.Count; i++)
            {
                AllPointsValue[i] = false;
            }
            Allitems = new List<Item>();

            int n = LevelQuestion.instance.Total_Options.Count;
            AssignRandomPrefab();
            //print("n is " + n);
            for (int i = 0; i < n; i++)
            {
                GameObject obj = Instantiate(prefab);
                Allitems.Add(obj.GetComponent<Item>());
                SetPosition(obj);
                obj.GetComponent<Item>().number = LevelQuestion.instance.Total_Options[i];
            }
            
        }

        private void SetPosition(GameObject _obj)
        {
           
            int r = UnityEngine.Random.Range(0, AllPointsValue.Count);

            if (!AllPointsValue[r])
            {
                _obj.transform.position = AllPoints[r].position;
                AllPointsValue[r] = true;
            }
            else
            {
                SetPosition(_obj);
            }
            
        }

        

        // Update is called once per frame
        void Update()
        {

        }
    }
}
