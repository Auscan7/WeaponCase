using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    public Transform manualSlot;
    public List<Transform> autoWeaponSlots;

    public List<Transform> usedAutoSlots = new List<Transform>();

    public void AttachWeapon(Weapon weapon)
    {
        if (!weapon.shouldMountToSlot) return; // Skip mounting if weapon shouldn't be mounted

        if (weapon.isManualWeapon)
        {
            weapon.transform.SetParent(manualSlot);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Transform freeSlot = GetFreeAutoSlot();
            if (freeSlot != null)
            {
                weapon.transform.SetParent(freeSlot);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                usedAutoSlots.Add(freeSlot);
            }
            else
            {
                Debug.LogWarning("No available auto weapon slots!");
            }
        }
    }

    private Transform GetFreeAutoSlot()
    {
        foreach (Transform slot in autoWeaponSlots)
        {
            if (!usedAutoSlots.Contains(slot))
                return slot;
        }
        return null;
    }

    public void ClearSlots() // Optional if you need to reset on reload
    {
        usedAutoSlots.Clear();
    }
}
