using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] float groundCheckOffset = 1;

    [Header("Flags")]
    public bool canMove = true;
    public bool isGrounded = true;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleGroundCheck();
    }

    protected void HandleGroundCheck()
    {
        Vector2 groundCheckPosition = (Vector2)character.transform.position + Vector2.down * groundCheckOffset;
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition, groundCheckSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        Vector2 groundCheckPosition = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheckPosition, groundCheckSphereRadius);
    }
}