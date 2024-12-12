using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Soldier;

public class InfatryScript : MonoBehaviour, ISoldier
{
    public string Personality { get; private set; }

    private Rigidbody2D rb;
    private float speed;
    private Vector3 targetPosition;
    private float stopDistance = 0.1f;

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
    }
}