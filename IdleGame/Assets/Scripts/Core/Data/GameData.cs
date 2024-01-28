using UnityEngine;
using UnityEditor;

/// <summary>
/// 전체적인 게임의 진행을 관리
/// Game Data 위주로 로드,저장
/// </summary>
public class GameData : BehaviourSingleton<GameData>
{
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameData))]
public class GameSystem_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var o = (GameData)target;
    }
}
#endif