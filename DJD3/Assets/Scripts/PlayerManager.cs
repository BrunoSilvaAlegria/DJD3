using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;
    public int maxHealth;
    public int currentHealth;
    private float lastHitTime = -Mathf.Infinity;
    public float invincible;
    [Header("Fuel")]
    public Slider fuelSlider;
    public int maxFuel;
    public int currentFuel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentFuel = maxFuel;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (fuelSlider.value != currentFuel)
        {
            fuelSlider.value = currentFuel;
        }

        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = currentHealth;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SpendHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GainHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GainFuel(100);
        }
    }

    public void SpendFuel(int fuel)
    {
        currentFuel -= fuel;
        if (currentFuel < 0)
        {
            currentFuel = 0;
        }
    }

    public void GainFuel(int fuel)
    {
        currentFuel += fuel;
        if(currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

        public void SpendHealth(int health)
    {
        if (Time.time - lastHitTime < invincible)
        {
            Debug.Log("Hit ignored due to invincibility");
            return;
        }
        lastHitTime = Time.time;
        currentHealth -= health;
        
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void GainHealth(int health)
    {
        currentHealth += health;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
