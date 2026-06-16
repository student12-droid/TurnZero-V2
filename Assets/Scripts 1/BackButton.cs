// Back button out of the ping pong 
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public GameObject PingPongEnviornment;
    public GameObject BattlePanel; 

    public void OnBackButton()
    {
        PingPongEnviornment.SetActive(false);
        BattlePanel.SetActive(true);
    }
}