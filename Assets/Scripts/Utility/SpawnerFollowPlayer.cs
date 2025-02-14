using UnityEngine;

public class SpawnerFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private Transform MenuBG;

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;
            transform.position = player.position;
        }

        if (MenuBG != null)
        {
            Vector3 targetPosition = MenuBG.position;
            transform.position = MenuBG.position;
        }
    }
}
