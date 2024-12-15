using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private float movementSpeed = 10;
    private float moveTimer = 1f;
    private bool moveCooldown = false;

    private float health = 100;
    public float hitScore = 1;

    private Rigidbody2D rb;

    public ManagerScript managerScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCooldown == false)
        {
            StartCoroutine(MoveTimer());
        }
    }

    private IEnumerator MoveTimer()
    {
        moveCooldown = true;
        yield return new WaitForSeconds(moveTimer);
        Move();
        moveCooldown = false;
    }

    private void Move()
    {
        GameObject target = GameObject.FindWithTag("Soldier");

        if (target != null)
        {
            Debug.Log("target is not null");

            rb.velocity = Vector3.zero; // Stop current movement

            Vector3 targetPosition = target.transform.position; // Get target's position
            targetPosition.z = 0;

            // Calculate direction toward the target
            Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;

            // Set Rigidbody2D velocity
            rb.velocity = direction * movementSpeed;
        }
        else
        {
            Debug.LogWarning("Target not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health--;
            Debug.Log(health);
            managerScript.UpdateScore(hitScore);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Soldier"))
        {
            rb.velocity = Vector3.zero;

            Vector3 targetPosition = collision.gameObject.transform.position;
            targetPosition.z = 0;

            Vector2 direction = ((Vector2)targetPosition - rb.position).normalized * -1f;

            rb.velocity = direction * movementSpeed * 1.5f;
        }
    }
}
