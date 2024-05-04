public class PlayerInfo
{
    public static string RealmDark = "Dark";
    public static string RealmLight = "Light";

    public bool reloadRequired = false;

    public int id { get; set; }

    public string punPlayerId { get; set; }

    public string name { get; set; }

    public int goldCoins { get; set; }

    public int silverCoins { get; set; }

    public string firstLoggedIn { get; set; }

    public string lastLoggedIn { get; set; }

    public string username { get; set; }

    public string password { get; set; }

}