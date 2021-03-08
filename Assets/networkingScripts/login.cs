using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class login : MonoBehaviour
{
    public static login instance;

    public delegate void LoadSceneDelegate();
    public LoadSceneDelegate loadSceneDelegate;
    public InputField userNameInput;
    public InputField passwordInput;
    public Button loginButton, Sign_Up;
    public GameObject Registeration_Panel;
    public static string UserName;

    public Text loginStatus;

    private void Awake()
    {
        instance = this;
        loadSceneDelegate += LoadScene;
    }
    // Start is called before the first frame update 
    void Start()
    {
        //loginButton.onClick.AddListener(Login);
        Sign_Up.onClick.AddListener(() => { Registeration_Panel.SetActive(true); });
    }

    public void Login()
    {
        StartCoroutine(main.Instance.Web.Login(userNameInput.text, passwordInput.text));
        UserName = userNameInput.text; // referenece of user loggedin for further use
        loginStatus.text = main.Instance.Web.LoginStatus; // retuen and print the login status on canvas
        SavePlayerInfo(UserName);
        //Debug.LogWarning("login status" + main.Instance.Web.LoginStatus);
        //if (main.Instance.Web.LoginSuccessful)
        //    {
        //        SceneManager.LoadScene("Ascending2Digit");  // only execute is login is successful
        //    }
        //    else
        //    {
        //        loginStatus.text = "Username or Password is incorrect, try again";  // flash if login is failed
        //    }
    }

    private void SavePlayerInfo(string userName)
    {
        PhotonNetwork.NickName = userName;

        Debug.Log("Photon newteork nick name : " + PhotonNetwork.NickName);
        // might need to add player prefs to save the nickname to the system.
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Photon Test");  // only execute is login is successful
    }

    public void FlashError()
    {
        loginStatus.text = "Username or Password is incorrect, try again";  // flash if login is failed
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
