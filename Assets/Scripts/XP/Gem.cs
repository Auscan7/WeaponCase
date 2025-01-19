using UnityEngine;

public class Gem : MonoBehaviour
{
    public int xpValue = 1; // Amount of XP this gem gives

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerLevelSystem.instance.AddXP(xpValue); // Pass gem's XP value
            Destroy(gameObject);
        }
    }
}
