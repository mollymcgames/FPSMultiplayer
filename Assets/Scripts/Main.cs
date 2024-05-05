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
                default:
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Main.instance.Web.Login(UsernameInput.text, PasswordInput.text));
        }

    }
}

   