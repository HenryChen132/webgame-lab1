using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Bullet bulletPrefab;     //Bullet Prefab
    [SerializeField] private Transform firePoint;     
    [SerializeField] private Transform yawSource;     

    [Header("Settings")]
    [SerializeField] private float fireCooldown = 0.15f;

    private float nextFireTime;

    private void Update()
    {
        if (Mouse.current == null) return;
        if (Time.time < nextFireTime) return;

        // 左键开火
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Fire();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private void Fire()
    {
        if (!bulletPrefab || !firePoint || !yawSource) return;

        Bullet b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector3 dir = yawSource.forward;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) dir = transform.forward;

        b.Fire(dir);
    }
}
