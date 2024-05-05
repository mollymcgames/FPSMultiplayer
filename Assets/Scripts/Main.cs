using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Main : MonoBehaviour
{
    public static Main instance;

    public Web Web;

    private EventSystem eventSystemHandle;
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField NewUsernameInput;
    public InputField NewPasswordInput;

    private void Start()
    {
        instance = this;
        Web =  GetComponent<Web>();
    }

    [System.Obsolete]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            eventSystemHandle = EventSystem.current.GetComponent<EventSystem>();
            switch (eventSystemHandle.currentSelectedGameObject.name)
            {
                case "UsernameInput":
                    eventSystemHandle.SetSelectedGameObject(GameObject.Find("PasswordInput"));
                    break;
                case "PasswordInput":
                    eventSystemHandle.SetSelectedGameObject(GameObject.Find("UsernameInput"));
                    break;
                case "NewUsername":
                    eventSystemHandle.SetSelectedGameObject(GameObject.Find("Password"));
                    break;
                case "Password":
                    eventSystemHandle.SetSelectedGameObject(GameObject.Find("NewUsername"));
                    break;

                default:
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (eventSystemHandle != null)
            {
                string currentPlace = eventSystemHandle.currentSelectedGameObject.name;
                if (currentPlace != null)
                {
                    if (currentPlace == "NewUsername" || currentPlace == "Password")
                        StartCoroutine(Main.instance.Web.RegisterUser(NewUsernameInput.text, NewPasswordInput.text));
                    else
                        StartCoroutine(Main.instance.Web.Login(UsernameInput.text, PasswordInput.text));
                }
            }
        }

    }
}

   