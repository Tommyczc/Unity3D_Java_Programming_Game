using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detector : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject audio_play;
    private bool detected=false;
    public bool isDetect { get { return detected; }}
    void Start()
    {
        audio_play = GameObject.FindWithTag("AudioManager").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            detected = true;
            if (audio_play != null)
            {

                audio_play.GetComponent<AudioManager>().turnOff();
            }
            //Debug.Log(other.gameObject.tag);
            //elevatorOut = true;
            //computer.gameObject.GetComponent<InterfaceAnimManager>().startDisappear();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (audio_play != null) {
                audio_play.GetComponent<AudioManager>().turnOff();
                audio_play.GetComponent<AudioManager>().playAudio("Audio_level0_lifting_us",0.2f);
            }
        }
    }
}
