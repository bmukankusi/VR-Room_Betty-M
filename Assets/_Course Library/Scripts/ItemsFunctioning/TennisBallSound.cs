using UnityEngine;
using UnityEngine.Audio; // Required for AudioSource
using System.Collections; // Required for MonoBehaviour

public class TennisBallSound : MonoBehaviour
{
   
    public AudioClip bounceSound;
    private AudioSource audioSource;

    [Header("Sound Properties")]
    [Tooltip("Minimum velocity required for a sound to play.")]
    public float minImpactVelocity = 0.5f;

    [Tooltip("Maximum velocity for full volume. Impacts above this won't get louder.")]
    public float maxImpactVelocity = 10.0f;

    [Tooltip("The base volume for the sound.")]
    [Range(0f, 1f)] 
    public float baseVolume = 0.7f;

    [Tooltip("How much the volume scales with impact velocity.")]
    [Range(0f, 2f)]
    public float volumeScaleFactor = 0.1f;

    [Tooltip("How much the pitch scales with impact velocity.")]
    [Range(0f, 0.1f)]
    public float pitchScaleFactor = 0.05f;

    [Tooltip("Random pitch variation to make sounds less repetitive.")]
    [Range(0f, 0.2f)]
    public float randomPitchVariation = 0.1f;

    void Awake()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource for playing one-shot sounds.
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1.0f; // Make it a 3D sound for VR
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic; // Realistic falloff
        audioSource.maxDistance = 20f; 
        audioSource.minDistance = 1f;
    }

    /// <summary>
    /// Called when this collider/rigidbody has begun touching another rigidbody/collider.
    /// This is where we detect collisions and play sounds.
    /// </summary>
    /// <param name="collision">Contains information about the collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        // Ensure we have a sound assigned and the collision object is not null.
        if (bounceSound == null || collision.gameObject == null)
        {
            Debug.LogWarning("Bounce sound not assigned or collision object is null.", this);
            return;
        }

        // Get the relative velocity of the collision.
        // This indicates how hard the objects hit each other.
        float impactVelocity = collision.relativeVelocity.magnitude;

        // Check if the impact velocity is above the minimum threshold to play a sound.
        if (impactVelocity < minImpactVelocity)
        {
            return; // Impact was too soft, don't play sound.
        }

        // Calculate the normalized impact strength (0 to 1) based on min/max velocity.
        float normalizedImpact = Mathf.InverseLerp(minImpactVelocity, maxImpactVelocity, impactVelocity);

        // Calculate the volume based on normalized impact and base volume.
        // We use Mathf.Clamp01 to ensure the volume stays between 0 and 1.
        float finalVolume = Mathf.Clamp01(baseVolume + (normalizedImpact * volumeScaleFactor));

        // Calculate the pitch based on normalized impact and add random variation.
        float finalPitch = 1.0f + (normalizedImpact * pitchScaleFactor) + Random.Range(-randomPitchVariation, randomPitchVariation);
        finalPitch = Mathf.Clamp(finalPitch, 0.5f, 2.0f); // Clamp pitch to a reasonable range

        // Set the AudioSource's pitch before playing.
        audioSource.pitch = finalPitch;

        // Play the bounce sound once at the calculated volume.
        // PlayOneShot is good for collision sounds as it allows multiple sounds
        // to overlap without stopping the previous one (e.g., rapid bounces).
        audioSource.PlayOneShot(bounceSound, finalVolume);

        // Optional: Log collision details for debugging
        // Debug.Log($"Collision with {collision.gameObject.name} at impact velocity: {impactVelocity:F2}. Playing sound with volume: {finalVolume:F2}, pitch: {finalPitch:F2}");
    }
}
