using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public int difficultyLevel;
    public Button button;

    void Start()
    {
        int unlocked = DifficultyManager.instance.unlockedDifficulty;
        button.interactable = difficultyLevel <= unlocked;

        button.onClick.AddListener(() => {
            DifficultyManager.instance.SetDifficulty(difficultyLevel);
            Debug.Log("Selected difficulty: " + difficultyLevel);
        });
    }
}
