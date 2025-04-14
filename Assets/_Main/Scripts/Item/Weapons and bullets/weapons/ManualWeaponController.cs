using UnityEngine;
using System.Collections.Generic;

public class ManualWeaponController : MonoBehaviour
{
    private Camera mainCamera;
    private List<Weapon> manualWeapons = new List<Weapon>();

    void Start()
    {
        mainCamera = Camera.main;

        Weapon[] allWeapons = GetComponentsInChildren<Weapon>(true); // ✅ include deeper children too
        foreach (var weapon in allWeapons)
        {
            if (weapon.isManualWeapon)
            {
                manualWeapons.Add(weapon);
            }
        }
    }


    void Update()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        foreach (var weapon in manualWeapons)
        {
            if (!weapon.gameObject.activeInHierarchy)
            {
                weapon.HideAimingLine();
                continue;
            }

            weapon.RotateTowardsTarget(mousePos);
            weapon.UpdateAimingLine(mousePos);

            if (weapon.CanFire())
            {
                weapon.Fire(mousePos);
            }
        }
    }

}
