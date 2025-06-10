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
        if (currentFuel > maxFuel)
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
        if (isDead) return;

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
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died");

        // Find all GameObjects tagged "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObj in players)
        {
            // Spawn death object at each player's position with rotation 90° on X
            if (objectToSpawnOnDeath != null)
            {
                GameObject spawnedObject = Instantiate(objectToSpawnOnDeath, playerObj.transform.position, Quaternion.Euler(90f, 0f, 0f));
                Destroy(spawnedObject, 2f);
            }

            // Play death sound at player's position
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, playerObj.transform.position);
            }

            // Disable PlayerMovement components
            PlayerMovement[] playerMovements = playerObj.GetComponentsInChildren<PlayerMovement>(true);
            foreach (var pm in playerMovements)
            {
                pm.enabled = false;
            }

            // Disable BasicMovement components
            BasicMovement[] basicMovements = playerObj.GetComponentsInChildren<BasicMovement>(true);
            foreach (var bm in basicMovements)
            {
                bm.enabled = false;
            }

            // Disable all MeshRenderers under this player (including children)
            MeshRenderer[] meshRenderers = playerObj.GetComponentsInChildren<MeshRenderer>(true);
            foreach (var mr in meshRenderers)
            {
                mr.enabled = false;
            }

            // Disable child named "MC_Hand_Model 1"
            Transform handModel = playerObj.transform.Find("MC_Hand_Model 1");
            if (handModel != null)
            {
                handModel.gameObject.SetActive(false);
            }

            // Disable child named "DefaultModel"
            Transform defaultModel = playerObj.transform.Find("DefaultModel");
            if (defaultModel != null)
            {
                defaultModel.gameObject.SetActive(false);
            }
        }

        // Start coroutine to wait 2 seconds then load "Hub" scene
        StartCoroutine(LoadHubSceneAfterDelay(2f));
    }

    private IEnumerator LoadHubSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Hub");
    }
}
