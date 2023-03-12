using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level1_frontDoor_steamfx : MonoBehaviour
{
    public ParticleSystem steam_left;
    public ParticleSystem steam_right;
    // Start is called before the first frame update

    private void Start()
    {
        pauseSteamfx();

    }
    public void playSteamfx()
    {
        steam_left.gameObject.SetActive(true);
        steam_right.gameObject.SetActive(true);
        steam_left.Play();
        steam_right.Play();
    }

    public void pauseSteamfx()
    {

        steam_left.Pause();
        steam_right.Pause();
        steam_left.gameObject.SetActive(false);
        steam_right.gameObject.SetActive(false);
    }


}
