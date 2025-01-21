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
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject healthBarParent;

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
            HandleDeath();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;
    }

    protected virtual void HandleDeath()
    {
        Destroy(gameObject);
    }
}
