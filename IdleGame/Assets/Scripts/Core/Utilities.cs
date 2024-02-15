using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class Utilities
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        return obj.GetComponent<T>() ?? obj.AddComponent<T>();
    }

    public static string GenerateID()
    {
        StringBuilder builder = new();
        Enumerable
            .Range(65, 26)
            .Select(e => ((char)e).ToString())
            .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
            .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
            .OrderBy(e => Guid.NewGuid())
            .Take(11)
            .ToList().ForEach(e => builder.Append(e));

        return builder.ToString();
    }

    public static string ConvertToString(int number) => Convert(number);
    public static string ConvertToString(long number) => Convert(number);
    private static string Convert(long number)
    {
        //만 자리 이하일 경우 그냥 반환
        if (number < 1_000)
        {
            return number.ToString();
        }
        string[] numSymbol = { "", "A ", "B  ", "C ", "D ", "E " };
        int magnitudeIndex = (int)Mathf.Log10(number) / 3;
        StringBuilder sb = new StringBuilder()
        
            .Append((number * Mathf.Pow(0.001f, magnitudeIndex)).ToString("N2"))
            .Append(numSymbol[magnitudeIndex]);

        return sb.ToString();
    }

    #region Download

    /// <summary>
    /// 네트워크가 연결되어 있는가?
    /// </summary>
    /// <returns></returns>
    public static bool IsNetworkValid()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    /// <summary>
    /// 현재 캐시에 여유 공간이 있는가?
    /// </summary>
    /// <param name="requiredSize">필요 최소 공간</param>
    /// <returns></returns>
    public static bool IsDiskSpaceEnough(long requiredSize)
    {
        return Caching.defaultCache.spaceFree >= requiredSize;
    }

    public static long OneGB = 1000000000;
    public static long OneMB = 1000000;
    public static long OneKB = 1000;

    /// <summary>
    /// 바이트 사이즈에 맞게끔 적절한 단위 표시
    /// </summary>
    /// <param name="byteSize"></param>
    /// <returns></returns>
    public static SizeUnits GetProperByteUnit(long byteSize)
    {
        if (byteSize >= OneGB)
        {
            return SizeUnits.GB;
        }
        else if (byteSize >= OneMB) 
        {
            return SizeUnits.MB;
        }
        else if (byteSize >= OneKB)
        {
            return SizeUnits.KB;
        }

        return SizeUnits.Bytes;
    }

    /// <summary>
    /// 바이트를 단위 사이즈에 맞게 숫자로 변환
    /// </summary>
    /// <param name="byteSize"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static long ConvertByteByUnit(long byteSize, SizeUnits unit)
    {
        return (long)(byteSize / (double)Math.Pow(1024, (long)unit));
    }

    /// <summary>
    /// 바이트를 단위와 함께 출력이 가능한 문자열 형태로 변환
    /// </summary>
    /// <param name="byteSize"></param>
    /// <param name="unit"></param>
    /// <param name="appendUnit"></param>
    /// <returns></returns>
    public static string GetConvertedByteString(long byteSize, SizeUnits unit, bool appendUnit = true)
    {
        string unitStr = appendUnit ? unit.ToString() : string.Empty;
        return $"{ConvertByteByUnit(byteSize, unit).ToString("0.00")}{unitStr}";
    }

    #endregion
}