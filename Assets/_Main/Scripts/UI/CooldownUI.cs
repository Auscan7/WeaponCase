using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay; // Top radial fill
    [SerializeField] private Image weaponIcon;       // Static icon

    private Coroutine cooldownRoutine;

    public void SetWeaponIcon(Sprite icon)
    {
        weaponIcon.sprite = icon;
    }

    public void StartCooldown(float duration)
    {
        //if (cooldownRoutine != null) StopCoroutine(cooldownRoutine);
        cooldownRoutine = StartCoroutine(CooldownCoroutine(duration));
    }

    private IEnumerator CooldownCoroutine(float duration)
    {
        float time = 0f;
        cooldownOverlay.fillAmount = 1f;

        while (time < (1 / duration))
        {
            time += Time.deltaTime;
            cooldownOverlay.fillAmount = 1f - (time * duration);
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
    }
}
