
using System.Text;
using UnityEngine;

public class NumUnit
{
    public static string ConvertToString(int number) => Convert(number);
    
    public static string ConvertToString(long number) => Convert(number);

    private static string Convert(long number)
    {
        //만 자리 이하일 경우 그냥 반환
        if (number < 1_000)
        {
            return number.ToString();
        }
        string[] numSymbol = { "", "A ", "B ", "C ", "D ", "E " };
        int magnitudeIndex = (int)Mathf.Log10(number) / 3;

        StringBuilder sb = new StringBuilder()

            .Append(number.ToString().Substring(0, number.ToString().Length % 3 == 0 ? 3 : number.ToString().Length % 3))
            .Append(numSymbol[magnitudeIndex])
            .Append(number.ToString().Substring(number.ToString().Length % 3 == 0 ? 3 : number.ToString().Length % 3, 3));

        if (magnitudeIndex >= 2) sb.Append(numSymbol[magnitudeIndex - 1]);

        return sb.ToString();
    }
}
