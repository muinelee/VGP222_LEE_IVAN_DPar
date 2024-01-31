using UnityEngine;

public class EnemyGreen : Enemy
{
    private Transform playerTransform;
    public float followSpeed = 10.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        FindPlayerShip();
        FollowPlayer();
    }

    void FindPlayerShip()
    {
        if (playerTransform == null)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null && playerController.currentShip != null)
            {
                playerTransform = playerController.currentShip.transform;
            }
        }
    }

    void FollowPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 position = transform.position;

            position.x = Mathf.Lerp(position.x, playerTransform.position.x, Time.deltaTime * followSpeed);
            position.y -= Time.deltaTime * followSpeed;

            transform.position = position;
        }
    }
}