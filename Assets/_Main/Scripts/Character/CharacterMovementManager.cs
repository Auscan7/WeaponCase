using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour
{
    protected CharacterManager character;

    [Header("Flags")]
    public bool canMove = true;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual void Move(Vector2 direction, float speed)
    {
        if (canMove)
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }
}