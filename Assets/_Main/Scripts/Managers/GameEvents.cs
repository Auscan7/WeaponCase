using System;
using Unity.VisualScripting;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> OnWeaponActivated;

    public static void WeaponActivated(GameObject weapon)
    {
        OnWeaponActivated?.Invoke(weapon);
    }
}
