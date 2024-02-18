using System.Collections.Generic;
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

            var profile = Manager.Data.Profile;

            // 사용자의 점수를 업데이트
            UpdateUserScore(profile.Uid, profile.Nickname, profile.Stage_Chapter);
        });
    }

    // 사용자의 점수를 업데이트
    private void UpdateUserScore(string userId, string userName, int userScore)
    {
        DatabaseReference userScoreRef = reference.Child("leaderboard").Child(userId);
        userScoreRef.Child("name").SetValueAsync(userName);
        userScoreRef.Child("score").SetValueAsync(userScore);
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

                        var uiUserRank = Manager.Asset.InstantiatePrefab("UserRank", contents).GetComponent<UIUserRank>();
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
