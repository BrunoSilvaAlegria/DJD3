using UnityEngine;
using System.Collections;

public class RobotStatus : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public bool canTakeOver = false;

    public int healthToTakeOver;
    [SerializeField] private GameObject objectToDestroy;
    public float invincible = 1.0f; // Time in seconds before the enemy can be hit again

    private float lastHitTime = -Mathf.Infinity;

    public int fuelPerHit = 25;

    private PlayerManager playerManager;

    [Header("Burning Damage Settings")]
    public float timeTillHit = 1.0f;      // How often the damage is applied
    public float burnTimer = 5.0f;        // Total duration of the burn
    public int burningDamage = 5;         // Amount of damage per tick
    public int initialBurningDamage = 2;

    public GameObject fire;

    private Coroutine burnCoroutine;

    void Start()
    {
        fire.active = false;
        currentHealth = maxHealth;
        playerManager = FindAnyObjectByType<PlayerManager>();
        canTakeOver = true;
    }

    void Update()
    {
        //if (currentHealth <= healthToTakeOver)
            //canTakeOver = true;
    }

    public void GetHit(int damage)
    {
        if (Time.time - lastHitTime < invincible)
        {
            Debug.Log("Hit ignored due to invincibility");
            return;
        }

        lastHitTime = Time.time;
        if(fire.active == false)
        {
            playerManager.GainFuel(fuelPerHit);
        }
        currentHealth -= damage;
        Debug.Log($"Robot hit for {damage} damage");

        if (currentHealth <= 0)
        {
            Debug.Log("Robot killed");
            Destroy(objectToDestroy);
        }
    }

    public void StartOrResetBurn()
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
        burnCoroutine = StartCoroutine(BurnOverTime());
    }

    private IEnumerator BurnOverTime()
    {
        float elapsed = 0f;

        while (elapsed < burnTimer)
        {
            fire.active = true;
            GetHit(initialBurningDamage);
            yield return new WaitForSeconds(timeTillHit);
            GetHit(burningDamage);
            elapsed += timeTillHit;
        }
        fire.active = false;
        burnCoroutine = null;
    }
}
