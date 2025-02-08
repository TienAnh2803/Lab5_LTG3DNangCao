using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    private Rigidbody rb;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // Đạn bay thẳng
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Enemy")) // Kiểm tra nếu va chạm với quái
        {
            other.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject); // Xóa đạn sau khi va chạm
        }
    }
}
