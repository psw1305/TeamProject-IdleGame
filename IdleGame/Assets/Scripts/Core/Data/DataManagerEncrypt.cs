using System;

public partial class DataManager
{
    #region Encrypt

    private int Encrypt(int value)
    {
        return value;
    }
    private long Encrypt(long value)
    {
        return value;
    }

    private float Encrypt(float value)
    {
        return value;
    }

    private double Encrypt(double value)
    {
        return value;
    }

    private string Encrypt(string value)
    {
        return value;
    }

    private bool Encrypt(bool value)
    {
        return value;
    }

    #endregion

    #region Decrypt

    private int DecryptInt(object value)
    {
        if (value == null) return 0;
        return Convert.ToInt32(value);
    }

    private long DecryptLong(object value)
    {
        if (value == null) return 0;
        return (long)value;
    }

    private float DecryptFloat(object value)
    {
        if (value == null) return 0.0f;
        return Convert.ToSingle(value);
    }

    private double DecryptDouble(object value)
    {
        if (value == null) return 0.0;
        return (double)value;
    }

    private string DecryptString(object value)
    {
        if (value == null) return "";
        return (string)value;
    }

    private bool DecryptBool(object value)
    {
        if (value == null) return false;
        return (bool)value;
    }

    #endregion
}
