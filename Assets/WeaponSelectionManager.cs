using System.Collections;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour
{
    GameTimeManager gameTimeManager;

    public GameObject weaponSelectionPanel;
    public GameObject gameTimeText;
    public GameObject Tutorial;
    public GameObject XPLevel;
    public GameObject PlayerHP;

    private void Awake()
    {
        gameTimeManager = GetComponent<GameTimeManager>();
        gameTimeManager.timerRunning = false;
        PauseManager.instance.PauseGame();
    }

    public void Battle()
    {
        PauseManager.instance.UnPauseGame();
        weaponSelectionPanel.SetActive(false);
        Tutorial.SetActive(true);
        StartCoroutine(ActivateDelayed());
    }

    private IEnumerator ActivateDelayed()
    {
        yield return new WaitForSeconds(6);
        gameTimeText.SetActive(true);
        gameTimeManager.timerRunning = true;
        XPLevel.SetActive(true);
        PlayerHP.SetActive(true);
    }
}
