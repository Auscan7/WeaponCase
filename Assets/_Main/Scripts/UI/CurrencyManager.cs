using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Currency Settings")]
    [HideInInspector]public int Currency;
    public TMPro.TextMeshProUGUI CurrencyText;

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        Currency = PlayerPrefs.GetInt("Currency", 0);
        UpdateCurrencyText();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    AddCurrency(100);
        //}

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    AddCurrency(-10);
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    PlayerPrefs.DeleteAll();
        //}
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        PlayerPrefs.SetInt("Currency", Currency);
        UpdateCurrencyText();
    }

    private void UpdateCurrencyText()
    {
        CurrencyText.text = Currency.ToString();
    }
}
