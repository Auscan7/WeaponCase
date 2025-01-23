using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    protected override void ShowFloatingDamage(float damage)
    {
        if (floatingDamagePrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(xOffsetRange.x, xOffsetRange.y), Random.Range(yOffsetRange.x, yOffsetRange.y), 0);

            GameObject floatingText = Instantiate(floatingDamagePrefab, transform.position + Vector3.up + randomOffset, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = damage.ToString("F0");
                textComponent.color = Color.red;
            }
        }
        else
        {
            Debug.LogWarning("Floating Damage Prefab is not assigned in the inspector.");
        }
    }
}
