using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Transform player; // Reference to the player following the spline
    public Transform[] birds; // Array of bird Transforms (children of FlightPath)
    public float flyToPositionSpeed = 5f; // Speed at which birds fly to their designated positions
    public float forwardOffset = 2f; // Amount to move birds forward in front of the player
    public float leftOffset = 0.5f; // Amount to move birds to the left of the player
    public float downOffset = 0.5f; // Amount to move birds down relative to the player
    public float triggerThreshold = 0.5f; // Threshold to determine when the trigger is pressed enough

    private Vector3[] targetPositions; // Array to store the target positions for each bird
    private bool[] isFlyingToPosition;
    private bool[] isFollowingPlayer;

    private void Start()
    {
        int birdCount = birds.Length;
        isFlyingToPosition = new bool[birdCount];
        isFollowingPlayer = new bool[birdCount];
        targetPositions = new Vector3[birdCount];

        // Set target positions relative to the player's forward, left, and down directions
        targetPositions[0] = new Vector3(3.28999996f - leftOffset, 1f - downOffset, 1f); // Bird 1
        targetPositions[1] = new Vector3(1f - leftOffset, 1f - downOffset, -0.0390000008f); // Bird 2
        targetPositions[2] = new Vector3(-1.77999997f - leftOffset, 1f - downOffset, 1f); // Bird 3
    }

    private void LateUpdate()
    {
        // By the time LateUpdate is called, the player's position should be fully updated

        for (int i = 0; i < birds.Length; i++)
        {
            // Check if the corresponding Oculus controller button or trigger has been pressed
            if ((i == 0 && OVRInput.GetDown(OVRInput.Button.One)) || // A button
                (i == 1 && OVRInput.GetDown(OVRInput.Button.Two)) || // B button
                (i == 2 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > triggerThreshold)) // Trigger
            {
                isFlyingToPosition[i] = true;
            }

            if (isFlyingToPosition[i])
            {
                FlyToPosition(birds[i], i);
            }

            if (isFollowingPlayer[i])
            {
                FollowPlayer(birds[i], i);
            }
        }
    }

    private void FlyToPosition(Transform bird, int index)
    {
        // Move the bird to its target position relative to the player's current position with a forward offset
        Vector3 forwardPosition = player.position + player.forward * forwardOffset;
        Vector3 targetPosition = forwardPosition + player.TransformPoint(targetPositions[index]) - player.position;
        bird.position = Vector3.MoveTowards(bird.position, targetPosition, flyToPositionSpeed * Time.deltaTime);

        // Rotate the bird to face the direction of movement along the spline
        Vector3 direction = (targetPosition - bird.position).normalized;
        if (direction != Vector3.zero)
        {
            bird.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(bird.position, targetPosition) < 0.1f)
        {
            isFlyingToPosition[index] = false;
            isFollowingPlayer[index] = true;
        }
    }

    private void FollowPlayer(Transform bird, int index)
    {
        // Keep the bird at the relative target position as the player moves, with a forward, left, and down offset
        Vector3 forwardPosition = player.position + player.forward * forwardOffset;
        Vector3 targetPosition = forwardPosition + player.TransformPoint(targetPositions[index]) - player.position;
        bird.position = targetPosition;

        // Make the bird face the direction of movement along the spline
        Vector3 direction = player.forward; // Assume player.forward represents the direction along the spline
        if (direction != Vector3.zero)
        {
            bird.rotation = Quaternion.LookRotation(direction);
        }
    }
}
