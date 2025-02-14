using UnityEngine;

public class Wrench : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.wrenchPickUpSFX);

            UpgradeManager.Instance.playerCurrentHealth += Mathf.RoundToInt((UpgradeManager.Instance.playerMaxHealth / 10) * 1.5f);

            if (UpgradeManager.Instance.playerCurrentHealth > UpgradeManager.Instance.playerMaxHealth)
            {
                UpgradeManager.Instance.playerCurrentHealth = UpgradeManager.Instance.playerMaxHealth;
            }

            Destroy(gameObject);
        }
    }
}
