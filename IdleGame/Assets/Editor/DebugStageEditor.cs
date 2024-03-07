using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class DebugStageOpen
{
    static DebugStageOpen()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        string activeSceneName = EditorSceneManager.GetActiveScene().name;

        if (activeSceneName == "DebugStage" && !IsStageEditorOpen())
        {
            StageEditor.ShowWindow();
        }
    }

    private static bool IsStageEditorOpen()
    {
        var window = EditorWindow.GetWindow<StageEditor>(false, "Stage Test Setting", false);
        return window != null;
    }
}

public class StageEditor : EditorWindow
{
    private int stageNumber = 10;
    private bool bossProgress;
    private bool stageLoop;

    private int attackLevel = 1;
    private int weaponLevel = 1;

    [MenuItem("Tools/Stage Test Setting")]
    public static void ShowWindow()
    {
        GetWindow<StageEditor>("Stage Test Setting");
    }

    private void OnGUI()
    {
        // Foldout을 사용하여 섹션을 접거나 펼 수 있게 합니다.
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Stage Setting", EditorStyles.boldLabel);

        // 스테이지 설정
        stageNumber = EditorGUILayout.IntField("Stage", stageNumber);
        bossProgress = EditorGUILayout.Toggle("Boss Progress (bool)", bossProgress);
        stageLoop = EditorGUILayout.Toggle("Stage Loop (bool)", stageLoop);

        EditorGUILayout.EndVertical();

        // 플레이어 설정
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Player Setting", EditorStyles.boldLabel);

        attackLevel = EditorGUILayout.IntField("Attack Level", attackLevel);
        weaponLevel = EditorGUILayout.IntField("Weapon Level", weaponLevel);

        EditorGUILayout.EndVertical();

        // 데이터를 저장하는 로직을 추가합니다.
        if (GUILayout.Button("Save Settings"))
        {
            SaveStageSettings();
        }
    }

    private void SaveStageSettings()
    {
        // TODO : 여기에 설정을 저장하는 로직을 구현 (playerfrefs)
    }
}

#endif