using UnityEngine;
using UnityEngine.UI;



public class Login : MonoBehaviour
{
   public InputField UsernameInput;
   public InputField PasswordInput;
   public Button LoginButton;

    void Start()
   {
        LoginButton.onClick.AddListener(() =>
        {
            StartCoroutine ( Main.instance.Web.Login(UsernameInput.text, PasswordInput.text));
      });
   }
}
