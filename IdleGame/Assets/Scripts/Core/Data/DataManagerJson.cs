using System;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager
{
    public delegate void MapIsChanged(string key, object from, object to);
    
    public delegate void DataIntChanged(int from, int to);
    public delegate void DataLongChanged(long from, long to);
    public delegate void DataFloatChanged(float from, float to);
    public delegate void DataDoubleChanged(double from, double to);
    public delegate void DataStringChanged(string from, string to);
    public delegate void DataBoolChanged(bool from, bool to);

    #region Sentinel

    [Serializable]
    private class Sentinel
    {
        [SerializeField] public Dictionary<string, bool> map;
        [SerializeField] public Dictionary<string, MapIsChanged> mapIsChanged;

        public void InitSentinel()
        {
            map = new Dictionary<string, bool>();
            mapIsChanged = new Dictionary<string, MapIsChanged>();
        }
    }

    #endregion

    #region DateSet

    [Serializable]
    private class DataSet
    {
        [SerializeField] public Dictionary<string, object> map;

        public void InitDataSet()
        {
            map = new Dictionary<string, object>();
        }
    }

    #endregion

    private DataSet data = null;
    private Sentinel sentinel = null;

    public DataManager()
    {
        data ??= new DataSet();
        sentinel ??= new Sentinel();

        data.InitDataSet();
        sentinel.InitSentinel();
    }

    #region Set Value => Key

    public void SetValue(string key, int value)
    {
        var from = GetInt(key);
        if (from == value) return;

        data.map[key] = Encrypt(value);
        sentinel.map[key] = true;
        if (sentinel.mapIsChanged.ContainsKey(key))
            sentinel.mapIsChanged[key]?.Invoke(key, from, value);
    }

    public void SetValue(string key, long value)
    {
        var from = GetLong(key);
        if (from == value) return;

        data.map[key] = Encrypt(value);
        sentinel.map[key] = true;
        if (sentinel.mapIsChanged.ContainsKey(key))
            sentinel.mapIsChanged[key]?.Invoke(key, from, value);
    }

    public void SetValue(string key, float value)
    {
        var from = GetFloat(key);
        if (from == value) return;

        data.map[key] = Encrypt(value);
        sentinel.map[key] = true;
        if (sentinel.mapIsChanged.ContainsKey(key))
            sentinel.mapIsChanged[key]?.Invoke(key, from, value);
    }

    public void SetValue(string key, double value)
    {
        var from = GetDouble(key);
        if (from == value) return;

        data.map[key] = Encrypt(value);
        sentinel.map[key] = true;
        if (sentinel.mapIsChanged.ContainsKey(key))
            sentinel.mapIsChanged[key]?.Invoke(key, from, value);
    }

    public void SetValue(string key, string value)
    {
        var from = GetString(key);
        if (from == value) return;

        data.map[key] = Encrypt(value);
        sentinel.map[key] = true;
        if (sentinel.mapIsChanged.ContainsKey(key))
            sentinel.mapIsChanged[key]?.Invoke(key, from, value);
    }

    public void SetValue(string key, bool value)
    {
        var from = GetBool(key);
        if (from == value) return;

        data.map[key] = Encrypt(value);
        sentinel.map[key] = true;
        if (sentinel.mapIsChanged.ContainsKey(key))
            sentinel.mapIsChanged[key]?.Invoke(key, from, value);
    }

    #endregion

    #region Get Value => Key

    public int GetInt(string key, int def = 0)
    {
        if (data.map.ContainsKey(key)) return DecryptInt(data.map[key]);

        data.map[key] = Encrypt(def);
        return def;
    }

    public long GetLong(string key, long def = 0)
    {
        if (data.map.ContainsKey(key)) return DecryptLong(data.map[key]);

        data.map[key] = Encrypt(def);
        return def;
    }

    public float GetFloat(string key, float def = 0.0f)
    {
        if (data.map.ContainsKey(key)) return DecryptFloat(data.map[key]);

        data.map[key] = Encrypt(def);
        return def;
    }

    public double GetDouble(string key, double def = 0.0)
    {
        if (data.map.ContainsKey(key)) return DecryptDouble(data.map[key]);

        data.map[key] = Encrypt(def);
        return def;
    }

    public string GetString(string key, string def = "")
    {
        if (data.map.ContainsKey(key)) return DecryptString(data.map[key]);

        data.map[key] = Encrypt(def);
        return def;
    }

    public bool GetBool(string key, bool def = false)
    {
        if (data.map.ContainsKey(key)) return DecryptBool(data.map[key]);

        data.map[key] = Encrypt(def);
        return def;
    }

    #endregion

    public void RemoveKey(string key)
    {
        if (data.map.ContainsKey(key))
        {
            data.map.Remove(key);
        }
    }

    public bool ValueIsChanged(string key)
    {
        if (!sentinel.map.ContainsKey(key)) return false;

        bool ret = sentinel.map[key];
        sentinel.map[key] = false;
        return ret;
    }

    public void SetMapIsChangedCallback(string key, MapIsChanged callback)
    {
        sentinel.mapIsChanged[key] = callback;
    }
}
