using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MovingWall : NetworkBehaviour
{
    public float moveDistance = 1.0f;
    public float moveSpeed = 1.0f;

    private Vector3 initialPosition;
    private bool isMoving = false;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        if (other.CompareTag("Bullet"))
        {
            if (!isMoving)
            {
                RequestMoveServerRPC();
            }
        }
    }
    [ServerRpc]
    private void RequestMoveServerRPC()
    {
        RequestMoveClientRPC();
    }

    [ClientRpc]
    private void RequestMoveClientRPC()
    {
        StartCoroutine(MoveWall());
    }
    private IEnumerator MoveWall()
    {
        isMoving = true;

        float targetX = transform.position.x + moveDistance;
        float startTime = Time.time;

        while (Time.time < startTime + moveSpeed)
        {
            float t = (Time.time - startTime) / moveSpeed;
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetX, t), transform.position.y, transform.position.z);
            yield return null;
        }

      transform.position = new Vector3(targetX, transform.position.y, transform.position.z); 

       yield return new WaitForSeconds(0.1f);

        targetX = transform.position.x - moveDistance;
        startTime = Time.time;

        while (Time.time < startTime + moveSpeed)
        {
            float t = (Time.time - startTime) / moveSpeed;
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetX, t), transform.position.y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z); 

        isMoving = false;
    }
}

