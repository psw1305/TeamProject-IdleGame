using UnityEngine;
using UnityEngine.UI;

public class QuestClearEffect : MonoBehaviour
{
    [SerializeField] private Image image;

    private Color color;

    public Image Image => image;

    private void Start()
    {
        color = image.color;
    }

    private void Update()
    {
        if (Manager.Quest.CurrentQuest.objectiveValue <= Manager.Quest.CurrentQuest.currentValue)
        {
            color.a = 0.5f;
            image.color = color;
        }
        else
        {
            color.a = 0f;
            image.color = color;
        }
    }
}