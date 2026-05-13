using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float speed = 5f;
    public float moveSpeed = 10f;

    public float bottomY = -3.827f;
    public float topY = 0.2f;

    private float targetY;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f; // ยิงทุก 0.25 วิ

    private float nextFireTime = 0f;
    public float shootDuration = 2.5f; // ยิงได้นานกี่วิ
    public float cooldownTime = 3f;    // พักกี่วิ
    private float shootTimer = 0f;
    public float maxHeat = 100f;
    public float heatPerShot = 10f;
    public float coolSpeed = 30f;

    private float currentHeat = 0f;
    private bool isOverheated = false;
    private bool isCoolingDown = false;
    private bool isShooting = false;
    public Image shootButtonImage;
    public TextMeshProUGUI cooldownText;

    public GameObject muzzleFlash;

    void Start()
    {
        targetY = bottomY;

        // 🔥 เพิ่มอันนี้
        transform.position = new Vector3(
            transform.position.x,
            bottomY,
            0
        );
    }

    bool isTopLane = false;

    void Update()
    {
        if (!GameManagers.Instance.isRunning) return;

        HandleShooting();

        // ❄️ ลด heat ถ้าไม่ได้ยิง
        if (!isShooting && !isOverheated)
        {
            currentHeat -= coolSpeed * Time.deltaTime;
            currentHeat = Mathf.Clamp(currentHeat, 0, maxHeat);
        }
        UpdateUI();


        // if (Input.GetMouseButtonDown(0))
        // {
        //     Shoot();

        //     isTopLane = !isTopLane;
        //     targetY = isTopLane ? topY : bottomY;
        // }

        float newY = Mathf.Lerp(transform.position.y, targetY, moveSpeed * Time.deltaTime);

        // 🔥 ถ้าใกล้แล้ว → ล็อกเลย
        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            newY = targetY;
        }

        transform.position = new Vector3(
            transform.position.x + speed * Time.deltaTime,
            newY,
            0
        );
    }

    void UpdateUI()
    {
        float t = currentHeat / maxHeat;

        if (isOverheated)
        {
            shootButtonImage.color = Color.black;

            cooldownText.gameObject.SetActive(true);
        }
        else
        {
            // 🔥 ไล่สี ขาว → แดง
            shootButtonImage.color = Color.Lerp(Color.white, Color.red, t);

            cooldownText.gameObject.SetActive(false);
        }
    }

    public void Stop()
    {
        speed = 0f;
    }
    public void StartShooting()
    {
        if (isCoolingDown) return;
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }
    void HandleShooting()
    {
        if (isCoolingDown) return;
        if (!isShooting) return;

        shootTimer += Time.deltaTime;

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // 🔥 ยิงนานเกิน → cooldown
        if (shootTimer >= shootDuration && !isCoolingDown)
        {
            StartCoroutine(CooldownRoutine());
        }
    }
    IEnumerator CooldownRoutine()
    {
        if (isCoolingDown) yield break;
        isCoolingDown = true; // 🔥 เพิ่ม
        isOverheated = true;
        isShooting = false;

        float cooldownTimer = cooldownTime;

        while (cooldownTimer > 0)
        {
            cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();

            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        cooldownText.text = "";

        currentHeat = 0;
        shootTimer = 0f;

        isOverheated = false;
        isCoolingDown = false; // 🔥 สำคัญ
    }
    void Shoot()
    {
        if (isCoolingDown) return;

        Vector3 pos = firePoint.position;
        pos.y = targetY;

        Instantiate(bulletPrefab, pos, Quaternion.identity);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);

        currentHeat += heatPerShot;

        // 🔥 heat เต็ม → cooldown
        if (currentHeat >= maxHeat && !isCoolingDown)
        {
            currentHeat = maxHeat;
            StartCoroutine(CooldownRoutine());
        }
    }
    public void GoTopLane()
    {
        targetY = topY;
        isTopLane = true;
    }

    public void GoBottomLane()
    {
        targetY = bottomY;
        isTopLane = false;
    }

    public bool IsTopLane()
    {
        return isTopLane;
    }

    public void ToggleLane()
    {
        if (isTopLane)
        {
            GoBottomLane();
        }
        else
        {
            GoTopLane();
        }
    }
}