using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MovingWallY : NetworkBehaviour
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

        float targetZ = transform.position.z + moveDistance; // Cambiado a Z
        float startTime = Time.time;

        while (Time.time < startTime + moveSpeed)
        {
            float t = (Time.time - startTime) / moveSpeed;
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, targetZ, t)); // Cambiado a Z
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, targetZ); // Asegura que la posición final sea exacta

        yield return new WaitForSeconds(0.1f); // Pausa opcional entre movimientos para mejorar la transición

        targetZ = transform.position.z - moveDistance; // Cambiado a Z
        startTime = Time.time;

        while (Time.time < startTime + moveSpeed)
        {
            float t = (Time.time - startTime) / moveSpeed;
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, targetZ, t)); // Cambiado a Z
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, targetZ); // Asegura que la posición final sea exacta

        isMoving = false;
    }
}
