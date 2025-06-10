using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Death Settings")]
    [SerializeField] private GameObject objectToSpawnOnDeath;
    [SerializeField] private AudioClip deathSound;

    private bool isDead = false;

    void Start()
    {
        currentFuel = maxFuel;
        currentHealth = maxHealth;
    }

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

        if (Input.GetKeyDown(KeyCode.H)) SpendHealth(10);
        if (Input.GetKeyDown(KeyCode.J)) GainHealth(10);
        if (Input.GetKeyDown(KeyCode.K)) GainFuel(100);
    }

    public void SpendFuel(int fuel)
    {
        currentFuel = Mathf.Max(0, currentFuel - fuel);
    }

    public void GainFuel(int fuel)
    {
        currentFuel = Mathf.Min(maxFuel, currentFuel + fuel);
    }

    public void SpendHealth(int health)
    {
        if (Time.time - lastHitTime < invincible || isDead) return;

        lastHitTime = Time.time;
        currentHealth -= health;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void GainHealth(int health)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + health);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObj in players)
        {
            // Spawn death object
            if (objectToSpawnOnDeath != null)
            {
                GameObject spawned = Instantiate(
                    objectToSpawnOnDeath,
                    playerObj.transform.position,
                    Quaternion.Euler(90f, 0f, 0f)
                );
                Destroy(spawned, 2f);
            }

            // Play sound
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, playerObj.transform.position);
            }

            // Disable movement scripts
            foreach (var pm in playerObj.GetComponentsInChildren<PlayerMovement>(true))
                pm.enabled = false;

            foreach (var bm in playerObj.GetComponentsInChildren<BasicMovement>(true))
                bm.enabled = false;

            // Disable all mesh renderers
            foreach (var mr in playerObj.GetComponentsInChildren<MeshRenderer>(true))
                mr.enabled = false;

            // Disable specific child models
            DisableChildByName(playerObj.transform, "MC_Hand_Model 1");
            DisableChildByName(playerObj.transform, "DefaultModel");
            DisableChildByName(playerObj.transform, "HeavyModel");
        }

        StartCoroutine(LoadHubSceneAfterDelay(2f));
    }

    private void DisableChildByName(Transform parent, string childName)
    {
        Transform child = parent.Find(childName);
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadHubSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Hub");
    }
}
