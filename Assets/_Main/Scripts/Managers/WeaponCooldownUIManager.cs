using System.Collections.Generic;
using UnityEngine;

public class WeaponCooldownUIManager : MonoBehaviour
{
    [SerializeField] private GameObject cooldownSlotPrefab;
    [SerializeField] private Transform slotContainer;

    private Dictionary<string, CooldownUI> cooldownSlots = new();

    public static WeaponCooldownUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AttachWeapon(string weaponName, Sprite weaponIcon)
    {
        if (cooldownSlots.ContainsKey(weaponName))
            return;

        var slot = Instantiate(cooldownSlotPrefab, slotContainer);
        slot.SetActive(true);

        var cooldownUI = slot.GetComponent<CooldownUI>();
        cooldownUI.SetWeaponIcon(weaponIcon);

        cooldownSlots[weaponName] = cooldownUI;
    }

    public void TriggerCooldown(string weaponName, float cooldown)
    {
        if (cooldownSlots.TryGetValue(weaponName, out var cooldownUI))
        {
            cooldownUI.StartCooldown(cooldown);
        }
    }
}
