using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.IK;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    public GameObject healthBar;
    public TextMeshProUGUI scoreText;
    private float score = 0;

    private Vector3 originalHealthBarScale;
    private float maxHealth;
    private float currentHealth;
    private float healthBarScale;

    private InfantryScript infantryScript;

    // Start is called before the first frame update
    void Start()
    {
        infantryScript = FindObjectOfType<InfantryScript>();
        if (infantryScript == null)
        {
            Debug.LogError("InfantryScript not found in the scene!");
            return;
        }

        originalHealthBarScale = healthBar.transform.localScale;

        currentHealth = infantryScript.health;
        maxHealth = currentHealth;
        HealthBarUpdate(0);

        scoreText.text = "Current score: " + score;
    }

    public void UpdateScore(float addScore)
    {
        score = score + addScore;
        scoreText.text = "Current score: " + score;
    }

    public void HealthBarUpdate(float damage)
    {
        currentHealth -= damage;
        float healthProportion = currentHealth / maxHealth;
        healthBar.transform.localScale = new Vector3(originalHealthBarScale.x * healthProportion, originalHealthBarScale.y, originalHealthBarScale.z);
    }
}
