using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public static main Instance; // static instance for singelton

    public web Web;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Web = GetComponent<web>();
    }
}
