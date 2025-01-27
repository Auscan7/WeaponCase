using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    public float speed = 2f;
    public float lifeTime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
