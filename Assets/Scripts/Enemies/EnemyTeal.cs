using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeal : Enemy
{
    public Transform playerTransform;
    public float followSpeed = 10.0f;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        FollowPlayer();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Destruction()
    {
        base.Destruction();
    }

    void FollowPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 position = transform.position;

            // Adjust position horizontally (X-axis) to follow the player
            position.x = Mathf.Lerp(position.x, playerTransform.position.x, Time.deltaTime * followSpeed);

            // Move downwards on the Y-axis
            position.y -= Time.deltaTime * followSpeed;

            transform.position = position;
        }
    }
}
