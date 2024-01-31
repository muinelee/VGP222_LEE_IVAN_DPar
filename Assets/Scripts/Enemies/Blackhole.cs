using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    // On player collision, disable player movement
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null && playerController.currentShip == collision.gameObject)
        {
            playerController.SetMovementEnabled(false);
        }
    }
}