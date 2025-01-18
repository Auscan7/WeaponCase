using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Health Bar")]
    [SerializeField] public float currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarParent;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (currentHealth == maxHealth)
        {
            healthBarParent.SetActive(false);
        }
        else
        {
            healthBarParent.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
