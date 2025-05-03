using System.Collections;
using UnityEngine;

public class HeavyCombat : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public Transform targetObject;
    public Transform pinPoint;

    public MonoBehaviour scriptToDisable1;
    public MonoBehaviour scriptToDisable2;

    public float runDuration = 3f;
    public float runCooldown = 5f;

    public float knockbackDistance = 3f;
    public float knockbackDuration = 0.2f;

    private float currentSpeed = 0f;
    private bool isActionActive = true;
    private bool heavyRun = false;
    private bool isPining = false;

    private float runTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isInCooldown = false;
    public int damage = 1;

    private CharacterController characterController;

    private Collider pinnedRobot;
    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    void Start()
    {
        characterController = targetObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleCooldowns();

        if (isActionActive && Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(1) && !isInCooldown)
        {
            heavyRun = true;
            runTimer += Time.deltaTime;

            if (runTimer >= runDuration)
            {
                StartCooldown();
            }

            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

            if (scriptToDisable1 != null && scriptToDisable1.enabled)
                scriptToDisable1.enabled = false;

            if (scriptToDisable2 != null && scriptToDisable2.enabled)
                scriptToDisable2.enabled = false;
        }
        else
        {
            // When exiting heavy run
            if (isPining)
            {
                if (pinnedRobot != null)
                {
                    // Store world position and rotation
                    Vector3 worldPos = pinnedRobot.transform.position;
                    Quaternion worldRot = pinnedRobot.transform.rotation;

                    // Detach
                    pinnedRobot.transform.SetParent(originalParent);
                    pinnedRobot.transform.position = worldPos;
                    pinnedRobot.transform.rotation = worldRot;

                    // Knockback
                    StartCoroutine(ApplyKnockback(pinnedRobot.transform));
                }

                pinnedRobot = null;
                originalParent = null;
                isPining = false;
            }

            heavyRun = false;

            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);

            if (scriptToDisable1 != null && !scriptToDisable1.enabled)
                scriptToDisable1.enabled = true;

            if (scriptToDisable2 != null && !scriptToDisable2.enabled)
                scriptToDisable2.enabled = true;
        }

        if (characterController != null)
        {
            Vector3 moveDirection = targetObject.forward * currentSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        int layer = collision.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Robot") && heavyRun && !isPining)
        {
            pinnedRobot = collision;
            originalParent = pinnedRobot.transform.parent;
            originalLocalPosition = pinnedRobot.transform.localPosition;
            originalLocalRotation = pinnedRobot.transform.localRotation;
            Pin();
        }

        // Destroy pinned robot if it hits terrain while pinned
        if (isPining && pinnedRobot != null && layer == LayerMask.NameToLayer("Terrain"))
        {
            Destroy(pinnedRobot.gameObject);
            pinnedRobot = null;
            isPining = false;
        }
    }

    void Pin()
    {
        Debug.Log("Robot Pined");

        if (pinnedRobot != null)
        {
            pinnedRobot.transform.SetParent(pinPoint);
        }

        isPining = true;
    }

    void HandleCooldowns()
    {
        if (isInCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= runCooldown)
            {
                isInCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    void StartCooldown()
    {
        isInCooldown = true;
        runTimer = 0f;
        heavyRun = false;
    }

    private IEnumerator ApplyKnockback(Transform target)
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0f;
        direction.Normalize();

        Vector3 start = target.position;
        Vector3 end = start + direction * knockbackDistance;

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            target.position = new Vector3(
                Mathf.Lerp(start.x, end.x, elapsed / knockbackDuration),
                target.position.y,
                Mathf.Lerp(start.z, end.z, elapsed / knockbackDuration)
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = new Vector3(end.x, target.position.y, end.z);
    }
}
