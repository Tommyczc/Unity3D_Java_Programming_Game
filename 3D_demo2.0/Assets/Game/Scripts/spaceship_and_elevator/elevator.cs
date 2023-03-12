using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject computer;
    public GameObject elevator_obj;
    public GameObject detector;

    private GameObject player;
    private bool playerIn=false;
    private bool elevatorOut = false;
    public float elevator_move_speed=1f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playthefuckinganimaton() {
        StartCoroutine(elevator_up());
    }

    private IEnumerator elevator_up() {

            while (true)
            {
                if (detector.gameObject.GetComponent<detector>().isDetect == false)
                {
                    elevator_obj.gameObject.transform.position += Vector3.up * Time.deltaTime * elevator_move_speed;
                    player.gameObject.transform.position += Vector3.up * Time.deltaTime * elevator_move_speed;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                else
                {
                    break;

                }
            }
        yield return null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerIn = true;
            computer.gameObject.GetComponent<InterfaceAnimManager>().startAppear();
        }


    }

    private void OnTriggerStay(Collider other) {
        if (playerIn == false) {
            if (other.gameObject.tag == "Player")
            {
                playerIn = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            playerIn = false;
            //elevatorOut = true;
            computer.gameObject.GetComponent<InterfaceAnimManager>().startDisappear();
        }
        Debug.Log(other.gameObject.tag);
    }
}
