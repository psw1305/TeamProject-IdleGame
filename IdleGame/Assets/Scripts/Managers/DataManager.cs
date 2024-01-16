using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Google;
using TMPro;

public class TestData
{
    public string nickname;
    public int level;
    public int damage;
    public int hp;
    public int gold;

    public TestData()
    {
        this.level = Random.Range(1, 10);
        this.damage = Random.Range(10, 100);
        this.hp = Random.Range(100, 1000);
        this.gold = Random.Range(1000, 10000);
    }
}

public class DataManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference databaseRef;

    private readonly string googleWebAPI = "63711996831-51h1oa135lu5eop464eb9vt2hhg2jhj0.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;
    private bool isSignin = false;

    public TextMeshProUGUI noticeText;
    public TextMeshProUGUI uidText;
    public TextMeshProUGUI loginText;
    public Button signinBtn;

    [Header("User Data")]
    public string testUID;


    private void Awake()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                noticeText.text = "Firebase Initialize Failed";
                return;
            }

            noticeText.text = "Firebase Initialize Complete";
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

#if UNITY_EDITOR
            CreateUserInDatabase(testUID);
#elif UNITY_ANDROID
            configuration = new GoogleSignInConfiguration { WebClientId = googleWebAPI, RequestEmail = true, RequestIdToken = true };
#endif
        });
    }

    private void Start()
    {
        signinBtn.onClick.AddListener(OnSignIn);
    }

    private void OnSignIn()
    {
#if UNITY_EDITOR
        Debug.Log("SignIn Button Click");
#elif UNITY_ANDROID
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

    private void SignInWithGoogle(Task<GoogleSignInUser> task)
    {
        // 구글 로그인
        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                noticeText.text = "Google Sign-In Failed";
                return;
            }

            FirebaseUser user = task.Result;
            noticeText.text = "Google Sign-In Successful!";
            loginText.text = "LOGOUT";

            //로그인 성공 후 Realtime Database에 데이터 생성
            if (user != null)
            {
                CreateUserInDatabase(user.UserId);
            }
        });
    }

    private void CreateUserInDatabase(string userId)
    {
        DatabaseReference userRef = databaseRef.Child("users").Child(userId);

        // 해당 UserID의 노드가 이미 존재하는지 확인
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                noticeText.text = "Failed to check user data in database.";
                return;
            }

            uidText.text = userId;
            DataSnapshot snapshot = task.Result;

            // UserID의 노드가 이미 존재하는 경우
            if (snapshot.Exists)
            {
                // 이미 존재하는 데이터를 불러와서 사용할 수 있음
                TestData existingData = JsonUtility.FromJson<TestData>(snapshot.GetRawJsonValue());
                noticeText.text = "Load User Data";
            }
            else
            {
                // 새로운 데이터 생성
                TestData newData = new TestData(); // 혹은 사용자가 원하는 초기값을 설정
                string json = JsonUtility.ToJson(newData);

                // 데이터 생성
                userRef.SetRawJsonValueAsync(json).ContinueWithOnMainThread(createTask =>
                {
                    if (createTask.IsFaulted || createTask.IsCanceled)
                    {
                        noticeText.text = "Failed to create user data in database.";
                    }
                    else
                    {
                        noticeText.text = "Create User Data";
                    }
                });
            }
        });
    }

    public bool IsUserLoggedIn()
    {
        return auth.CurrentUser != null;
    }
}
