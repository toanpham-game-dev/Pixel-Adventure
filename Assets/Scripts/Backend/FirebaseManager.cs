using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    private FirebaseFirestore db;

    public Action OnDataLoaded;
    public Action OnLoggedOut;

    public bool IsDataLoaded { get; private set; }
    public bool IsInitialized { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool forceLogoutInEditor = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        await InitFirebase();

        ForceLogoutLocal();

        //#if UNITY_EDITOR
        //        if (forceLogoutInEditor)
        //        {
        //            ForceLogoutLocal();
        //        }
        //#endif
    }

    // ================= INIT =================
    private async Task InitFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError("Firebase init failed");
            return;
        }

        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        currentUser = auth.CurrentUser;

        IsInitialized = true;

        Debug.Log("Firebase Ready");
    }

    // ================= LOGIN CHECK =================
    public bool IsLoggedIn()
    {
        return auth != null && auth.CurrentUser != null;
    }

    // ================= AUTO LOGIN =================
    public async void AutoLogin()
    {
        if (!IsInitialized) await InitFirebase();

        currentUser = auth.CurrentUser;

        if (currentUser != null)
        {
            Debug.Log("Auto login OK");
            await LoadOrCreateData();
        }
    }

    // ================= GUEST LOGIN =================
    public async void OnGuestLogin()
    {
        if (!IsInitialized) await InitFirebase();

        try
        {
            AuthResult result = await auth.SignInAnonymouslyAsync();
            currentUser = result.User;

            Debug.Log("Guest login success");

            await LoadOrCreateData();
        }
        catch (Exception e)
        {
            Debug.LogError("Login fail: " + e.Message);
        }
    }

    // ================= LOAD / CREATE DATA =================
    private async Task LoadOrCreateData()
    {
        if (currentUser == null) return;

        DocumentReference docRef = db.Collection("users").Document(currentUser.UserId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            CloudSaveData data = snapshot.ConvertTo<CloudSaveData>();
            ApplyCloudToLocal(data);
        }
        else
        {
            CloudSaveData newData = GetLocalData();
            await SaveCloudData(newData);
        }

        IsDataLoaded = true;
        OnDataLoaded?.Invoke();
    }

    // ================= APPLY CLOUD =================
    private void ApplyCloudToLocal(CloudSaveData data)
    {
        PlayerPrefs.SetInt("UnlockedLevel", data.UnlockedLevel);

        if (data.LevelStars != null)
        {
            foreach (var kv in data.LevelStars)
            {
                PlayerPrefs.SetInt("LevelStar_" + kv.Key, kv.Value);
            }
        }

        PlayerPrefs.Save();
    }

    // ================= LOCAL DATA =================
    public CloudSaveData GetLocalData()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        Dictionary<string, int> stars = new Dictionary<string, int>();

        for (int i = 1; i <= 50; i++)
        {
            int star = PlayerPrefs.GetInt("LevelStar_" + i, 0);
            if (star > 0)
            {
                stars[i.ToString()] = star;
            }
        }

        return new CloudSaveData
        {
            UnlockedLevel = unlockedLevel,
            LevelStars = stars,
            LastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    // ================= SAVE =================
    public async Task SaveCloudData(CloudSaveData data)
    {
        if (currentUser == null) return;

        await db.Collection("users")
            .Document(currentUser.UserId)
            .SetAsync(data);

        Debug.Log("Cloud saved");
    }

    // ================= LOGOUT =================
    public void Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
        }

        currentUser = null;
        IsDataLoaded = false;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Logout complete");

        OnLoggedOut?.Invoke();
    }

    // ================= DEBUG FORCE LOGOUT =================
    private void ForceLogoutLocal()
    {
        if (auth != null && auth.CurrentUser != null)
        {
            auth.SignOut();
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Force logout + reset data");
    }

    // ================= GOOGLE LOGIN BUTTON =================
    public async void OnGoogleLogin()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;

            await LoginWithGoogle();
        }
    }

    public async Task LoginWithGoogle()
    {
        try
        {

            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                RequestEmail = true,
                WebClientId = "1055467947637-p6bfhcsdvne5ak0tel7dj4hahl7taf2k.apps.googleusercontent.com"
            };

            Debug.Log("Đang gọi bảng đăng nhập Google...");
            GoogleSignInUser googleUser = await GoogleSignIn.DefaultInstance.SignIn();

            Debug.Log("Lấy Token thành công! Đang gửi lên Firebase...");

            Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

            currentUser = await auth.SignInWithCredentialAsync(credential);

            Debug.Log($"Google Login thành công: {currentUser.Email}");
        }
        catch (Exception e)
        {
            Debug.LogError("Google Login fail: " + e.Message);
            Debug.LogError("FULL ERROR: " + e.ToString());
        }
    }
}