using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class register : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField confirmPassword;
    public Button Register;
    // Start is called before the first frame update
    void Start()
    {
        Register.onClick.AddListener(RegisterUser);
    }

    void RegisterUser()
    {
        if(password.text == confirmPassword.text)
        {
            StartCoroutine(main.Instance.Web.RegisterUser(username.text, password.text));
        }
        else
        {
            Debug.Log("passwords dont match");
        }
    }
}
