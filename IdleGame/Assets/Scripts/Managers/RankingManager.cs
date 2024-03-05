using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class RankingManager
{
    private DatabaseReference reference;

    public void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            //var profile = Manager.Data.Profile;

            //// 사용자의 점수를 업데이트
            //UpdateUserScore(profile.Uid, profile.Nickname, profile.Stage_Chapter);
        });
    }

    // 사용자의 점수를 업데이트
    public void UpdateUserScore()
    {
        var profile = Manager.Data.Profile;

        var userScoreRef = reference.Child("leaderboard").Child(profile.Uid);
        userScoreRef.Child("name").SetValueAsync(profile.Nickname);
        userScoreRef.Child("score").SetValueAsync(profile.Stage_Chapter);
    }

    // 랭킹 조회
    public void GetLeaderboard(Transform contents)
    {
        Query leaderboardQuery = reference.Child("leaderboard").OrderByChild("score");
        leaderboardQuery.GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int rank = (int)snapshot.ChildrenCount;

                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        string userName = userSnapshot.Child("name").Value.ToString();
                        string userScore = userSnapshot.Child("score").Value.ToString();

                        var uiUserRank = Manager.Asset.InstantiatePrefab("UIUserRank", contents).GetComponent<UIUserRank>();
                        uiUserRank.Set(rank, userScore, userName);
                        rank--;
                    }
                }
                else
                {
                    Debug.Log("Leaderboard is empty.");
                }
            }
            else
            {
                Debug.LogError("Failed to get leaderboard: " + task.Exception);
            }
        });
    }
}
