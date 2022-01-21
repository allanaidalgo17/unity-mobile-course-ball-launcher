using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] [Range(0f, 1f)] private float detachDelay;
    [SerializeField] [Range(0f, 2f)] private float respawnDelay;

    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    void Update()
    {
        if (currentBallRigidbody == null)
        {
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
            return;
        }

        isDragging = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;
        currentBallRigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.transform.position, Quaternion.identity);

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.bodyType = RigidbodyType2D.Dynamic;
        currentBallRigidbody = null;
        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
