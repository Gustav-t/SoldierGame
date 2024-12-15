using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using static Soldier;

public class InfantryScript : MonoBehaviour, ISoldier
{
    public string Personality { get; private set; }

    public CircleCollider2D collider1;
    public CircleCollider2D collider2;

    public GameObject bulletPrefab;
    public GameObject firePoint;

    private ManagerScript managerScript;

    private Rigidbody2D rb;

    private float speed;
    public float health = 3;
    public float enemyDamage = 1;
    private float immunityTime = 2f;

    private Vector3 targetPosition;

    private float stopDistance = 0.1f;
    private float reloadTime = 0.5f;
    private float reloadingTimer = 0;
    private float bulletSpeed = 15;
    private float rotationSpeed = 100;

    private static readonly string[] PersonalityOptions =
    {
        "Aggresive",
        "Carefull"
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Personality = PersonalityOptions[UnityEngine.Random.Range(0, PersonalityOptions.Length)];
        Debug.Log("Infantry " + gameObject.name + " has personality " + Personality);

        if (Personality == "Aggresive")
        {
            speed = 10;
        }
        else if (Personality == "Carefull")
        {
            speed = 10;
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

        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0,0, angel + 90f));

        bullet.transform.rotation = targetRotation;

        reloadingTimer = reloadTime;

        RotateFirePoint(enemy);
    }

    private void RotateFirePoint(GameObject enemy)
    {
        // Calculate the direction to the enemy
        Vector3 direction = enemy.transform.position - firePoint.transform.position;

        // Get the angle in degrees for 2D rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the firePoint, affecting only the z-axis
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Smoothly rotate towards the target
        firePoint.transform.rotation = Quaternion.Slerp(firePoint.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && reloadingTimer <= 0)
        {
            Shoot(collision.gameObject);

            StartCoroutine(ResetColliderCooldown());
        }
    }

    private IEnumerator ResetColliderCooldown()
    {
        collider2.enabled = false;
        yield return new WaitForSeconds(0.1f);
        collider2.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && immunityOn == false)
        {
            health -= enemyDamage;

            StartCoroutine(immunity(immunityTime));

            ManagerScript managerScript = FindObjectOfType<ManagerScript>();
            if (managerScript != null)
            {
                managerScript.HealthBarUpdate(1);
            }

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private bool immunityOn = false;
    private IEnumerator immunity(float time)
    {
        immunityOn = true;
        yield return new WaitForSeconds(time);
        immunityOn = false;
    }
}