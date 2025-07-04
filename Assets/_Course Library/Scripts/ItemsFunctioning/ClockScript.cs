using UnityEngine;
using System; // Required for DateTime

public class AnalogClock : MonoBehaviour
{
    [Header("Clock Hands")]
    [Tooltip("Assign the GameObject representing the hour hand.")]
    public Transform hourHand;
    [Tooltip("Assign the GameObject representing the minute hand.")]
    public Transform minuteHand;
    [Tooltip("Assign the GameObject representing the second hand.")]
    public Transform secondHand;

    // Optional: Adjust the hand rotation directions if needed
    // Positive values for clockwise rotation from the initial orientation
    [Header("Rotation Offsets (Degrees)")]
    [Tooltip("Adjust if your clock hands are not pointing upwards at 12 o'clock initially.")]
    public float hourOffset = 0f;
    public float minuteOffset = 0f;
    public float secondOffset = 0f;

    void Update()
    {
        // Get the current real-world time
        DateTime currentTime = DateTime.Now;

        // Calculate rotations for each hand
        float seconds = currentTime.Second + (float)currentTime.Millisecond / 1000f;
        float secondAngle = seconds * 6f + secondOffset;

        float minutes = currentTime.Minute + seconds / 60f;
        float minuteAngle = minutes * 6f + minuteOffset;

        float hours = currentTime.Hour % 12 + minutes / 60f; // Use % 12 for 12-hour format
        float hourAngle = hours * 30f + hourOffset;

        // Apply rotations to the hands
        // *** CHANGE MADE HERE: Rotation is now around the X-axis ***
        // Ensure that the hands rotate clockwise from your perspective when looking at the clock face.
        // You might need to adjust the sign (+/-) of the angle based on your model's initial orientation
        // and which direction is considered "clockwise" for your X-axis rotation.
        if (secondHand != null)
        {
            secondHand.localRotation = Quaternion.Euler(-secondAngle, 0f, 0f); // X-axis, Y-axis, Z-axis
        }
        if (minuteHand != null)
        {
            minuteHand.localRotation = Quaternion.Euler(-minuteAngle, 0f, 0f); // X-axis, Y-axis, Z-axis
        }
        if (hourHand != null)
        {
            hourHand.localRotation = Quaternion.Euler(-hourAngle, 0f, 0f);     // X-axis, Y-axis, Z-axis
        }
    }
}