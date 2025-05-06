using UnityEngine;

public class Dismount : MonoBehaviour
{
    public Transform whereToSpawn;
    private PlayerManager playerManager;
    public GameObject hand;
    public GameObject objectToUntag;
    public GameObject objectToDestroy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            GameObject projectile = Instantiate(hand, whereToSpawn.position, whereToSpawn.rotation);
            playerManager.currentHealth = 1;
            
            if (objectToUntag != null)
            {
                //objectToUntag.tag = "Dead";
                //objectToUntag.layer = LayerMask.NameToLayer("Robot");
            }
            Destroy(objectToDestroy);
        }
    }
}
