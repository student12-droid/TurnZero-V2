using UnityEngine;

public class RobotFreeAnim : MonoBehaviour
{
    private Animator anim;
    private bool isPlayerInRange = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Only run the animation logic if the player has been detected
        if (isPlayerInRange)
        {
            RunAutomaticRoutine();
        }
    }

    // Triggered when an object enters the robot's detection zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Triggered when an object leaves the detection zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ResetAnimations();
        }
    }

    private void RunAutomaticRoutine()
    {
        // Start the routine with the roll animation (ball mode)
        anim.SetBool("Roll_Anim", true);
        
        // Add additional logic here to sequence other animations
        // like walking or opening after the roll starts
    }

    private void ResetAnimations()
    {
        // Stop all animations when the player is out of range
        anim.SetBool("Roll_Anim", false);
        anim.SetBool("Walk_Anim", false);
        anim.SetBool("Open_Anim", false);
    }
}