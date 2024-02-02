using UnityEngine;
using TMPro;

public class UIUserRank : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Set(int rank, string score, string name)
    {
        rankText.text = rank.ToString();
        scoreText.text = score;
        nameText.text = name;
    }
}
