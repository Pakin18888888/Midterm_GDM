using UnityEngine;
using System.Collections;

public class MagnetEffect : MonoBehaviour
{
    public float radius = 5f;
    public float speed = 10f;

    private bool isActive = false;

    public void Activate(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(MagnetRoutine(duration));
    }

    IEnumerator MagnetRoutine(float duration)
    {
        isActive = true;

        yield return new WaitForSeconds(duration);

        isActive = false;
    }

    void Update()
    {
        if (!isActive) return;

        Collider2D[] coins = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var c in coins)
        {
            if (c.CompareTag("Coin"))
            {
                c.transform.position = Vector3.MoveTowards(
                    c.transform.position,
                    transform.position,
                    speed * Time.deltaTime
                );
            }
        }
    }
}