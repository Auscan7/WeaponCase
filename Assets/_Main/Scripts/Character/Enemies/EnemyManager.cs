using UnityEngine;

public class EnemyManager : CharacterManager
{
    public Transform player { get; private set; } // Public getter, private setter

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;    
    }

    protected override void Update()
    {
        base.Update();
    }
}
