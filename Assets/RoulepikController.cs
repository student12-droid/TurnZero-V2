using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Animator robotAnimator;

    // This detects when another collider enters the Robot's trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Trigger the animation
            robotAnimator.SetTrigger("DoBackflip");
        }
    }
}