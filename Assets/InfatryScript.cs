using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using static Soldier;

public class InfatryScript : MonoBehaviour, ISoldier
{
    public string Personality { get; private set; }

    public GameObject bulletPrefab;
    public GameObject firePoint;
    private Rigidbody2D rb;
    private float speed;
    private Vector3 targetPosition;
    public float firePointDistance = 0.5f;
    private float stopDistance = 0.1f;
    private float reloadTime = 0.5f;
    private float reloadingTimer = 0;
    private float bulletSpeed = 15;
    private float rotationSpeed = 10;

    private static readonly string[] PersonalityOptions =
    {
        "Aggresive",
        "Carefull"
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Personality = PersonalityOptions[Random.Range(0, PersonalityOptions.Length)];
        Debug.Log("Infantry " + gameObject.name + " has personality " + Personality);

        if (Personality == "Aggresive")
        {
            speed = 10;
        }
        else if (Personality == "Carefull")
        {
            speed = 5;
        }
    }

    public void Move()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0; // Ensure z is 0 for 2D

        Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;

        rb.velocity = direction * speed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Move();
        }

        if (Vector2.Distance(rb.position, targetPosition) <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            targetPosition = Vector3.zero;
        }

        reloadingTimer -= Time.deltaTime;
    }

    public void Shoot(GameObject enemy)
    {
        //Bullet
        Vector3 direction = (enemy.transform.position - firePoint.transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = direction * bulletSpeed;

        reloadingTimer = reloadTime;

        RotateFirePoint(enemy, bullet);
    }

    private void RotateFirePoint(GameObject enemy, GameObject bullet)
    {
        Vector3 directionToTarget = targetPosition - transform.position;

        // Calculate the rotation to face the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate towards the target (optional)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5f * Time.deltaTime);

        bullet.transform.rotation = targetRotation;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && reloadingTimer <= 0)
        {
            Shoot(collision.gameObject);
        }
    }
}