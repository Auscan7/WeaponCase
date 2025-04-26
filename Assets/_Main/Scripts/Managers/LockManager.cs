using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockManager : MonoBehaviour
{
    public bool isLocked;
    public int unlockPrice;

    public Image lockedImage;
    public Image boatPreviewImage;
    public TextMeshProUGUI priceText;

    private string lockKey;

    private void Awake()
    {
        lockKey = "Boat_" + gameObject.name + "_Locked";
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(lockKey))
        {
            isLocked = PlayerPrefs.GetInt(lockKey) == 1;
        }
        else
        {
            // First time running, use whatever isLocked was set to in inspector
            PlayerPrefs.SetInt(lockKey, isLocked ? 1 : 0);
        }

        UpdateLockVisuals();
    }

    public bool UnlockBoat()
    {
        if (!isLocked) return false;

        if (CurrencyManager.Instance.Currency >= unlockPrice)
        {
            CurrencyManager.Instance.AddCurrency(-unlockPrice);
            isLocked = false;
            PlayerPrefs.SetInt(lockKey, 0);
            UpdateLockVisuals();
            Debug.Log($"{gameObject.name} unlocked!");
            return true;
        }
        else
        {
            Debug.Log($"Insufficient funds to unlock {gameObject.name}. Need {unlockPrice} Pearls.");

            return false;
        }
    }

    public void UpdateLockVisuals()
    {
        if (lockedImage != null)
            lockedImage.gameObject.SetActive(isLocked);

        if (boatPreviewImage != null)
            boatPreviewImage.color = isLocked ? new Color(1f, 1f, 1f, 0.5f) : Color.white;

        if (priceText != null)
        {
            priceText.text = $"{unlockPrice} Pearls";
            priceText.gameObject.SetActive(isLocked);
        }
    }
}
