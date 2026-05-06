using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public GameObject hitEffect;

    public PolarityType polarity;
    public GameObject correctEffect;
    public GameObject wrongEffect;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        // รับ polarity จาก player ตอนยิง
        polarity = PlayerPolaritys.Instance.currentPolarity;
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyPolaritys enemyPol = other.GetComponent<EnemyPolaritys>();
        EnemyHealths hp = other.GetComponent<EnemyHealths>();

        if (enemyPol != null && hp != null)
        {
            if (enemyPol.polarity != polarity)
            {
                // ✔ ยิงถูก
                hp.TakeDamage(2);

                if (correctEffect != null)
                    Instantiate(correctEffect, transform.position, Quaternion.identity);

                Destroy(gameObject); // ลบเฉพาะตอนถูก
            }
            else
            {
                // ❌ ยิงผิด → เด้งกลับ
                Debug.Log("Wrong polarity");

                speed = -speed;

                if (wrongEffect != null)
                    Instantiate(wrongEffect, transform.position, Quaternion.identity);

                return; // 🔥 สำคัญมาก หยุดก่อนถึง Destroy
            }
        }
    }
}