using UnityEngine;

public class NPCManager : BehaviourSingleton<NPCManager>
{
    [SerializeField] private GameObject npc;

    public int StartTutorial { get; private set; } 
    public int ShopTutorial { get; private set; } 

    private void Start()
    {
        StartTutorial = PlayerPrefs.GetInt("StartTutorial", 0);
        ShopTutorial = PlayerPrefs.GetInt("ShopTutorial", 0);
    }

    public void ActiveNPC()
    {
        npc.SetActive(true);
    }
}