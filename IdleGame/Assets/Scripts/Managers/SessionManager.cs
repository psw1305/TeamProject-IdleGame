using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Messaging;
using Firebase.Extensions;
using Google;

public class SessionManager
{
    #region Fields - Firebase

    private FirebaseAuth auth;
    private FirebaseFirestore db;

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
                Debug.LogError("Firebase Initialize Failed");
                return;
            }

            Debug.Log("Firebase Initialize Complete");
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);

            ReadUserDataFromFirestore();

            //FirebaseMessaging.TokenReceived += OnTokenReceived;
            //FirebaseMessaging.MessageReceived += OnMessageReceived;

            //databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

            // 모바일 
            //#if UNITY_EDITOR
            //            CreateUserInDatabase(testUID);
            //#elif UNITY_ANDROID
            //            configuration = new GoogleSignInConfiguration { WebClientId = googleWebAPI, RequestEmail = true, RequestIdToken = true };
            //#endif
        });
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
                Debug.LogError("Google Sign-In Failed");
                return;
            }

            FirebaseUser user = task.Result;
            Debug.Log("Google Sign-In Successful!");

            //로그인 성공 후 Realtime Database에 데이터 생성
            if (user != null)
            {
                //CreateUserInDatabase(user.UserId);
            }
        });
    }

    #endregion

    #region Firestore

    /// <summary>
    /// Firestore 내의 데이터를 읽어오기
    /// </summary>
    private void ReadUserDataFromFirestore()
    {
        DocumentReference docRef = db.Collection("users").Document(GuestID);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to read data: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            // 데이터가 존재할 경우
            if (snapshot.Exists)
            {
                Debug.Log($"Player data Load : {GuestID}");
                Manager.Data.SetUserProfile(snapshot.ConvertTo<GameUserProfile>());
            }
            // 데이터가 존재하지 않을 경우 => 새로운 유저 데이터 생성
            else
            {
                Debug.LogError("Player data not found.");
                CreateUserDataInFirestore();
            }
        });
    }

    /// <summary>
    /// 새로 데이터 생성해서 Firestore 내의 저장
    /// </summary>
    private void CreateUserDataInFirestore()
    {
        GameUserProfile userProfile = Manager.Data.CreateUserProfile(GuestID);

        db.Collection("users").Document(GuestID).SetAsync(userProfile).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to create user data: " + task.Exception);
            }
            else
            {
                Debug.LogError("Player data created successfully.");
            }
        });
    }

    public void UpdateUserData(Dictionary<string, object> updateFields)
    {
        Debug.Log("Data Save");
        //db.Collection("users").Document(GuestID).UpdateAsync(updates);
        db.Collection("users").Document(GuestID).SetAsync(updateFields, SetOptions.MergeAll);
    }

    #endregion

    #region Message

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        if (e != null)
        {
            Debug.LogError($"Token received: {e.Token}");
        }
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e != null && e.Message != null && e.Message.Notification != null)
        {
            Debug.LogError($"From : {e.Message.From}, Title : {e.Message.Notification.Title}, Text : {e.Message.Notification.Body}");
        }
    }

    #endregion
}
