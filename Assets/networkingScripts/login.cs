using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class login : MonoBehaviour
{
    public InputField userNameInput;
    public InputField passwordInput;
    public Button loginButton, Sign_Up;
    public GameObject Registeration_Panel;

    // Start is called before the first frame update 
    void Start()
    {
        loginButton.onClick.AddListener(Login);
        Sign_Up.onClick.AddListener(() => { Registeration_Panel.SetActive(true); });
    }

    void Login()
    {
        StartCoroutine(main.Instance.Web.Login(userNameInput.text, passwordInput.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
