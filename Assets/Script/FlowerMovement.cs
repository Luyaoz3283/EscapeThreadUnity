using UnityEngine;

public class FlowerMovement : MonoBehaviour
{
    public float baseSwaySpeed = 1f; // Base speed of swaying
    public float swayAmount = 10f; // Maximum sway angle
    public float rotationSpeed = 30f; // Base rotation speed
    public float windSpeed = 1f; // Controls the wind intensity
    public Vector3 windDirection = Vector3.right; // Wind direction (normalized vector)

    public float randomFrequency = 0.5f; // How often the wind changes direction
    public float randomIntensity = 0.5f; // Intensity of random movement variations
    private Vector3 randomOffset; // Adds randomness to movement

    private float timeSinceLastChange = 0f; // Tracks time for randomization

    void Start()
    {
        // Normalize initial wind direction
        windDirection = windDirection.normalized;
        GenerateRandomOffset();
    }

    void Update()
    {
        // Increment time
        timeSinceLastChange += Time.deltaTime;

        // Randomly adjust wind direction and speed
        if (timeSinceLastChange >= randomFrequency)
        {
            timeSinceLastChange = 0f;
            GenerateRandomOffset();
        }

        // Combine wind direction with random offset
        Vector3 effectiveWindDirection = (windDirection + randomOffset).normalized;

        // Adjust the sway speed based on wind speed
        float swaySpeed = baseSwaySpeed * windSpeed;

        // Calculate sway using Perlin noise for more natural movement
        float swayOffset = Mathf.PerlinNoise(Time.time * swaySpeed, 0) * 2 - 1; // Range: -1 to 1
        swayOffset *= swayAmount * windSpeed;

        // Apply swaying motion in the effective wind direction
        Vector3 swayRotation = effectiveWindDirection * swayOffset;
        transform.localRotation = Quaternion.Euler(swayRotation.x, swayRotation.y, swayRotation.z);

        // Continuous rotation around Y-axis, influenced by wind speed
        transform.Rotate(Vector3.up, rotationSpeed * windSpeed * Time.deltaTime, Space.World);
    }

    private void GenerateRandomOffset()
    {
        // Generate a small random offset for wind direction
        randomOffset = new Vector3(
            Random.Range(-randomIntensity, randomIntensity),
            0, // Keep wind on the XZ plane
            Random.Range(-randomIntensity, randomIntensity)
        );
    }
}
