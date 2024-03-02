using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;

public class LobbyScene : MonoBehaviour
{
    #region Fields - Firebase

    private FirebaseAuth auth;

    #endregion

    #region Fields - Google Signin

    private readonly string googleWebAPI = "63711996831-51h1oa135lu5eop464eb9vt2hhg2jhj0.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    #endregion

    [Header("UI")]
    [SerializeField] private Button guestLogin;
    [SerializeField] private Button googleLogin;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private DownloadPopup downloadPanel;

    #region Initialize

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = googleWebAPI, RequestEmail = true, RequestIdToken = true };

        guestLogin.onClick.AddListener(OnGuestLogin);
        googleLogin.onClick.AddListener(OnGoogleLogin);

        FirebaseAppInit();
    }

    /// <summary>
    /// Firebase App 초기화, 구글 로그인 세팅
    /// </summary>
    private void FirebaseAppInit()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted) return;

            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser != null)
            {
                NextDownloadPanel();
            }
            else
            {
                loginPanel.SetActive(true);
            }
        });
    }

    #endregion

    #region Login Button Events

    private void OnGuestLogin()
    {
        NextDownloadPanel();
    }

    private void OnGoogleLogin()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(LoginWithGoogle);
    }

    /// <summary>
    /// 로그인 완료 후 => 리소스 다운로드 시작
    /// </summary>
    private void NextDownloadPanel()
    {
        loginPanel.SetActive(false);
        downloadPanel.gameObject.SetActive(true);

        StartCoroutine(downloadPanel.DownloadRoutine());
    }

    #endregion

    #region Google Login

    private void LoginWithGoogle(Task<GoogleSignInUser> task)
    {
        // 구글 로그인
        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted) return;

            FirebaseUser user = task.Result;

            if (user != null)
            {
                NextDownloadPanel();
            }
        });
    }

    #endregion
}