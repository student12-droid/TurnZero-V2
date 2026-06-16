// Back button out of the ping pong 
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public GameObject PingPongEnviornment;
    public GameObject mainMenuPanel; 

    public void OnBackButton()
    {
        PingPongEnviornment.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}