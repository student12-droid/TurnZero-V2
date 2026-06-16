using UnityEngine;

public class AnimContr : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Triggered when the player enters the robot's range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Robot detects player and wakes up
            anim.Play("WakeUp");
        }
    }

    // Triggered when the player leaves the robot's range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Robot goes back to sleep when player leaves
            anim.Play("ShutDown");
        }
    }
}