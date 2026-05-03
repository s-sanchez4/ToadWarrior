using UnityEngine;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    // Adding 'Instance' makes it easy for any script to find this specific manager
    public static VictoryManager Instance;

    public TextMeshProUGUI victoryText;
    public int totalBosses = 3;
    private int bossesDefeated = 0;

    void Awake()
    {
        // This ensures there is only ever ONE VictoryManager
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
        // This is the line that 'checks the box' for you
        victoryText.gameObject.SetActive(true); 
        
        // This ensures the letters actually render immediately
        victoryText.text = "You Are a Fearless Toad Warrior!";
        
        Debug.Log("Victory Box Checked and Text Set!");
    }
    else
    {
        Debug.LogError("The Victory Text slot is empty! Drag the TMP object into the script slot.");
    }
}
}
