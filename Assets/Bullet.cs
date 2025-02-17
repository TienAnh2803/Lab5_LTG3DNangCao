using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float baseSpeed = 20f;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public int baseDamage = 50;
    public int minDamage = 30;
    public int maxDamage = 70;
    private float speed;
    private int damage;
    private Rigidbody rb;
    public float rotationSpeed = 200f; // Tốc độ xoay của đạn
    private Transform target; // Mục tiêu để đạn theo dõi
    public GameObject explosionEff;

    public void Initialize(float distance, Transform enemyTarget)
    {
        // Tính toán speed và damage dựa trên khoảng cách
        float normalizedDistance = Mathf.Clamp01(distance / 20f); // Giả sử phạm vi tối đa là 20 đơn vị
        
        speed = Mathf.Lerp(minSpeed, maxSpeed, normalizedDistance);
        damage = Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, normalizedDistance));
        
        rb = GetComponent<Rigidbody>();
        target = enemyTarget;
    }

    void Update()
    {
        if (target != null)
        {
            // Tính hướng đến mục tiêu
            Vector3 direction = target.position - transform.position;
            
            // Tạo rotation mới hướng về mục tiêu
            Quaternion rotation = Quaternion.LookRotation(direction);
            
            // Xoay đạn mượt mà về phía mục tiêu
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotation,
                rotationSpeed * Time.deltaTime
            );
            
            // Cập nhật velocity theo hướng mới
            rb.velocity = transform.forward * speed;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Enemy")) // Kiểm tra nếu va chạm với quái
        {
            Instantiate(explosionEff, transform.position, Quaternion.identity);
            other.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject); // Xóa đạn sau khi va chạm
        }
    }
}
