using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    #region Fields

    private readonly Dictionary<Type, Dictionary<string, UnityEngine.Object>> _objects = new();

    #endregion

    #region Init

    private void Start()
    {
        Init();
    }

    protected virtual void Init() { }

    #endregion

    #region Properties

    protected void SetUI<T>() where T : UnityEngine.Object => Binding<T>(gameObject);
    protected T GetUI<T>(string componentName) where T : UnityEngine.Object => GetComponent<T>(componentName);

    #endregion

    #region Binding

    /// <summary>
    /// UnityEngine.Object 타입의 컴포넌트들을 부모 오브젝트의 자식들 중에서 찾아서 딕셔너리에 저장
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    public void Binding<T>(GameObject parent) where T : UnityEngine.Object
    {
        T[] objects = parent.GetComponentsInChildren<T>(true);

        // 중복된 이름을 가진 컴포넌트들을 하나의 키로 묶기
        // 각 그룹에서 첫 번째로 등장하는 컴포넌트를 선택하여 딕셔너리에 저장
        Dictionary<string, UnityEngine.Object> objectDict = objects
            .GroupBy(comp => comp.name)
            .ToDictionary(group => group.Key, group => group.First() as UnityEngine.Object);

        _objects[typeof(T)] = objectDict;
        AssignComponentsDirectChild<T>(parent);
    }

    /// <summary>
    /// parent 내에서 컴포넌트의 이름과 일치하는 컴포넌트가 없는 경우, 
    /// 해당 자식을 찾아서 _objects 딕셔너리에 있는 컴포넌트들을 할당
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    private void AssignComponentsDirectChild<T>(GameObject parent) where T : UnityEngine.Object
    {
        if (!_objects.TryGetValue(typeof(T), out var objects)) return;

        // 각 컴포넌트에 대해 반복
        foreach (var key in objects.Keys.ToList())
        {
            // 이미 할당된 경우 스킵
            if (objects[key] != null) continue;

            // GameObject 타입인지 확인 후, 적절한 FindComponent 메서드 호출
            UnityEngine.Object component = typeof(T) == typeof(GameObject)
                ? FindComponentDirectChild<GameObject>(parent, key)
                : FindComponentDirectChild<T>(parent, key);

            // 찾은 컴포넌트가 null이 아니라면 할당하고, 그렇지 않다면 실패 로그 출력
            if (component != null)
            {
                objects[key] = component;
            }
            else
            {
                Debug.Log($"Binding failed for Object : {key}");
            }
        }
    }

    /// <summary>
    /// 직계 자식들 중에서 이름이 특정한 조건과 일치하는 컴포넌트 반환
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    /// <param name="name">주어진 이름과 일치하는 첫째 자식 이름</param>
    private T FindComponentDirectChild<T>(GameObject parent, string name) where T : UnityEngine.Object
    {
        return parent.transform
            .Cast<Transform>()
            .FirstOrDefault(child => child.name == name)
            ?.GetComponent<T>();
    }

    /// <summary>
    /// 함수가 저장된 딕셔너리에서 특정 타입과 이름에 해당하는 컴포넌트를 가져오는 역할
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    public T GetComponent<T>(string componentName) where T : UnityEngine.Object
    {
        if (_objects.TryGetValue(typeof(T), out var components) && components.TryGetValue(componentName, out var component))
        {
            return component as T;
        }

        return null;
    }

    #endregion
}
