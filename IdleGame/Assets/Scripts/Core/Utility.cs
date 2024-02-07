using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Utility : MonoBehaviour
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

            .Append(number.ToString().Substring(0, number.ToString().Length % 3 == 0 ? 3 : number.ToString().Length % 3))
            .Append(numSymbol[magnitudeIndex])
            .Append(number.ToString().Substring(number.ToString().Length % 3 == 0 ? 3 : number.ToString().Length % 3, 3));

        if (magnitudeIndex >= 2) sb.Append(numSymbol[magnitudeIndex - 1]);

        return sb.ToString();
    }

}