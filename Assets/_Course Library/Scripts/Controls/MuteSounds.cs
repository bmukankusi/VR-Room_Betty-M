using UnityEngine;
using UnityEngine.UI; // Required for Toggle
using TMPro;        // Required if you are using TextMeshPro for the toggle label

public class MuteSounds : MonoBehaviour
{
    [Tooltip("Reference to the UI Toggle component.")]
    public Toggle muteToggle;

    [Tooltip("Reference to the TextMeshProUGUI component on the toggle's label, if you want to update its text.")]
    public TextMeshProUGUI toggleLabelText;

    [Header("Label Text")]
    [Tooltip("Text to display when sound is muted (toggle is ON).")]
    public string mutedLabel = "All sounds muted";
    [Tooltip("Text to display when sound is unmuted (toggle is OFF).")]
    public string unmutedLabel = "Sound On";

    private float previousVolume = 1.0f; // Stores the volume before muting

    void Start()
    {
        if (muteToggle == null)
        {
            Debug.LogError("Mute Toggle reference is not set in the Inspector!", this);
            return;
        }

        // Initialize previousVolume with the current AudioListener volume at start.
        previousVolume = AudioListener.volume;

        // Set the initial state of the toggle based on the current AudioListener volume.
        // If volume is 0 or very close to 0, consider it muted.
        bool isCurrentlyMuted = AudioListener.volume <= 0.001f;
        muteToggle.isOn = isCurrentlyMuted; // Set the toggle's visual state

        // Update the label text immediately
        UpdateToggleLabel(isCurrentlyMuted);

        // Add a listener to the toggle's onValueChanged event.
        // This makes sure our script's method is called when the toggle state changes.
        muteToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    /// <summary>
    /// This method is called when the Toggle's value changes (checked/unchecked).
    /// It automatically receives the new boolean state of the toggle.
    /// </summary>
    /// <param name="isOn">The new state of the toggle (true if checked, false if unchecked).</param>
    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            // If toggle is ON (checked), mute the sound
            if (AudioListener.volume > 0.001f) // Only store if not already muted
            {
                previousVolume = AudioListener.volume; // Store current volume before muting
            }
            AudioListener.volume = 0f; // Set master volume to zero
            Debug.Log("Sound Muted.");
        }
        else
        {
            // If toggle is OFF (unchecked), unmute the sound
            AudioListener.volume = previousVolume; // Restore previous volume
            Debug.Log("Sound Unmuted. Volume restored to: " + AudioListener.volume);
        }

        // Update the label text to reflect the new state
        UpdateToggleLabel(isOn);
    }

    /// <summary>
    /// Updates the text of the toggle's label based on its current state.
    /// </summary>
    /// <param name="isOn">The current state of the toggle.</param>
    private void UpdateToggleLabel(bool isOn)
    {
        if (toggleLabelText != null)
        {
            toggleLabelText.text = isOn ? mutedLabel : unmutedLabel;
        }
    }

    void OnDestroy()
    {
        // Important: Remove the listener when the object is destroyed to prevent memory leaks.
        if (muteToggle != null)
        {
            muteToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }
}