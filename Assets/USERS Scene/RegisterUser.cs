using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{

    public InputField NewUsernameInput;
    public InputField NewUserPasswordInput;
    public Button SubmitButton;
    void Start()
    {
        SubmitButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.instance.Web.RegisterUser(NewUsernameInput.text, NewUserPasswordInput.text));
        });
    }





   


// Update is called once per frame
void Update()
    {
        
    }
}
