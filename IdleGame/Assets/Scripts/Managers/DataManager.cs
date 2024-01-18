using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;
using Google;

public class DataManager
{
    #region Fields - Firebase

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private DatabaseReference databaseRef;

    #endregion
    #region Fields - Google Signin

    private readonly string googleWebAPI = "63711996831-51h1oa135lu5eop464eb9vt2hhg2jhj0.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    #endregion

    #region Properties

    public string GuestID { get; private set; }

    #endregion

    #region Init

    public void Initialize(string guestID = "GUEST_ID")
    {
        GuestID = guestID;

        FirebaseAppInit();
    }

    /// <summary>
    /// Firebase App 초기화, 구글 로그인 세팅
    /// </summary>
    private void FirebaseAppInit()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                DebugNotice.Instance.Notice("Firebase Initialize Failed");
                return;
            }

            DebugNotice.Instance.Notice("Firebase Initialize Complete");
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);

            ReadUserDataFromFirestore();

            //databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

            // 모바일 
            //#if UNITY_EDITOR
            //            CreateUserInDatabase(testUID);
            //#elif UNITY_ANDROID
            //            configuration = new GoogleSignInConfiguration { WebClientId = googleWebAPI, RequestEmail = true, RequestIdToken = true };
            //#endif
        });
    }

    // TODO => 추후 로비 씬에서 로그인 버튼으로 옮길 예정
    private void OnSignIn()
    {
#if UNITY_EDITOR
        DebugNotice.Instance.Notice("SignIn Button Click");
#elif UNITY_ANDROID
        bool isSignin = false;

        if (!isSignin)
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(SignInWithGoogle);
        }
        else
        {
            GoogleSignIn.DefaultInstance.SignOut();
            loginText.text = "GOOGLE LOGIN";
        }

        isSignin = !isSignin;
#endif
    }

    #endregion

    #region Firestore

    private void ReadUserDataFromFirestore()
    {
        DocumentReference docRef = db.Collection("users").Document(GuestID);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                DebugNotice.Instance.Notice("Failed to read data: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                // 데이터가 존재할 경우
                Dictionary<string, object> data = snapshot.ToDictionary();
                DebugNotice.Instance.Notice($"Player data Load : {GuestID}");

                // 게임 시작
            }
            else
            {
                // 데이터가 존재하지 않을 경우
                DebugNotice.Instance.Notice("Player data not found.");
                CreateUserDataInFirestore();
            }
        });
    }

    private void CreateUserDataInFirestore()
    {
        Dictionary<string, object> userData = new()
        {
            { "username", $"Guest-{GuestID}" },
            { "uid", SystemInfo.deviceUniqueIdentifier },
            { "gold", 0 },
            { "gems", 0 },
            { "hp_Stat_Level", 1 },
            { "hp_Stat_Value", 1000 },
            { "hp_UpgradeCost", 10 },
            { "hpRecovery_Stat_Level", 1 },
            { "hpRecovery_Stat_Value", 30 },
            { "hpRecovery_UpgradeCost", 10 },
            { "atkDamage_Stat_Level", 1 },
            { "atkDamage_Stat_Value", 10 },
            { "atkDamage_UpgradeCost", 10 },
            { "atkSpeed_Stat_Level", 1 },
            { "atkSpeed_Stat_Value", 500 },
            { "atkSpeed_UpgradeCost", 10 },
            { "critChance_Stat_Level", 1 },
            { "critChance_Stat_Value", 500 },
            { "critChance_UpgradeCost", 10 },
            { "critDamage_Stat_Level", 1 },
            { "critDamage_Stat_Value", 1000 },
            { "critDamage_UpgradeCost", 10 },
            { "stage", 1 },
            { "stage_Level", 1 },
        };

        db.Collection("users").Document(GuestID).SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                DebugNotice.Instance.Notice("Failed to create player data: " + task.Exception);
            }
            else
            {
                DebugNotice.Instance.Notice("Player data created successfully.");

                // 새로 생성 시 게임시작
            }
        });
    }

    #endregion

    #region Save

    // 데이터 세이브
    public void Save<T>(string id, T obj)
    {
    }

    #endregion


    #region Load

    // 데이터 로드
    public T Load<T>(string id, T value)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new System.ArgumentNullException("id");
        }

        return value;
    }

    #endregion

    #region Exist

    // 데이터 체크
    public bool Exists(string identifier)
    {
        return true;
    }

    #endregion

    #region Google Signin Methods

    private void SignInWithGoogle(Task<GoogleSignInUser> task)
    {
        // 구글 로그인
        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                DebugNotice.Instance.Notice("Google Sign-In Failed");
                return;
            }

            FirebaseUser user = task.Result;
            DebugNotice.Instance.Notice("Google Sign-In Successful!");

            //로그인 성공 후 Realtime Database에 데이터 생성
            if (user != null)
            {
                CreateUserInDatabase(user.UserId);
            }
        });
    }

    #endregion

    private void CreateUserInDatabase(string userId)
    {
        DatabaseReference userRef = databaseRef.Child("users").Child(userId);

        // 해당 UserID의 노드가 이미 존재하는지 확인
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                DebugNotice.Instance.Notice("Failed to check user data in database.");
                return;
            }

            DataSnapshot snapshot = task.Result;

            // UserID의 노드가 이미 존재하는 경우
            if (snapshot.Exists)
            {
                // 이미 존재하는 데이터를 불러와서 사용할 수 있음
                GameUserProfile existingData = JsonUtility.FromJson<GameUserProfile>(snapshot.GetRawJsonValue());
                DebugNotice.Instance.Notice("Load User Data");
            }
            else
            {
                // 새로운 데이터 생성
                GameUserProfile newData = new(); // 혹은 사용자가 원하는 초기값을 설정
                string json = JsonUtility.ToJson(newData);

                // 데이터 생성
                userRef.SetRawJsonValueAsync(json).ContinueWithOnMainThread(createTask =>
                {
                    if (createTask.IsFaulted || createTask.IsCanceled)
                    {
                        DebugNotice.Instance.Notice("Failed to create user data in database.");
                    }
                    else
                    {
                        DebugNotice.Instance.Notice("Create User Data");
                    }
                });
            }
        });
    }

    private void UserDataBaseOrderByKey()
    {
        databaseRef.Child("users").OrderByKey().GetValueAsync().ContinueWithOnMainThread(snapshotTask =>
        {
            if (snapshotTask.IsCompleted && !snapshotTask.IsFaulted)
            {
                DataSnapshot snapshot = snapshotTask.Result;
                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string uid = userSnapshot.Key;
                    // 여기서 uid에 해당하는 사용자 데이터를 가져와서 처리
                    // ...
                }
            }
        });
    }
}
