using System;
using System.Linq;
using System.Text;
using UnityEngine;

public class SessionManager
{
    private static GameUserProfile Profile = new();
    private string KEY_USER_DATA = "KEY_USER_DATA";
    private bool isGuest = false;

    public void Initialize()
    {
        isGuest = true;
        HandleGuestSession();
    }

    private void HandleGuestSession()
    {
        GetUserProfile();

        string guestId = Profile == null ? GenerateID() : Profile.username.Split("-")[1];

        if (Profile == null)
        {
            Profile = new()
            {
                uid = SystemInfo.deviceUniqueIdentifier,
                createdDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                username = $"Guest-{guestId}"
            };

            SaveUserProfile();
        }
    }

    public string GenerateID()
    {
        StringBuilder builder = new();
        Enumerable
           .Range(65, 26)
            .Select(e => ((char)e).ToString())
            .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
            .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
            .OrderBy(e => Guid.NewGuid())
            .Take(11)
            .ToList().ForEach(e => builder.Append(e));

        return builder.ToString();
    }

    public void SaveUserProfile()
    {
        Manager.Data.Save(KEY_USER_DATA, Profile);
    }

    public void GetUserProfile()
    {
        if (Manager.Data.Exists(KEY_USER_DATA))
        {
            Profile = Manager.Data.Load<GameUserProfile>(KEY_USER_DATA, Profile);
        }
        else
        {
            Profile = null;
        }
    }
}
