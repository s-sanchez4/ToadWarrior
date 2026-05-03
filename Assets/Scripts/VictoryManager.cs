using UnityEngine;
using TMPro;

public class VictoryManager : MonoBehaviour
{
   
    public static VictoryManager Instance;

    public TextMeshProUGUI victoryText;
    public int totalBosses = 3;
    private int bossesDefeated = 0;

    void Awake()
    {
        
        Instance = this;
    }

    void Start()
    {
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }
    }

 public void BossDefeated()
{
    bossesDefeated++;
    Debug.Log("Current Boss Kill Count: " + bossesDefeated);

    if (bossesDefeated >= totalBosses)
    {
        ShowVictory();
    }
}
public void ShowVictory()
{
    if (victoryText != null)
    {
        
        victoryText.gameObject.SetActive(true); 
        
        victoryText.text = "You Are a Fearless Toad Warrior!";
        
        Debug.Log("Victory Box Checked and Text Set!");
    }
    else
    {
        Debug.LogError("The Victory Text slot is empty! Drag the TMP object into the script slot.");
    }
}
}
