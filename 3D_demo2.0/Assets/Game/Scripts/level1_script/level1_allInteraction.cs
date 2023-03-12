using javaCompiler;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level1_allInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("states")]
    [SerializeField]
    private GameObject Medic_door;
    [SerializeField]
    private Animator Medic_door_animator;
    [SerializeField]
    private GameObject medic_interact_with_door;
    [SerializeField]
    private GameObject restAreaDoorState;

    //private GameObject medic_interact_with_door_animator;
    public static List<interactionData> theMission=new List<interactionData>();

    [Header("UI")]
    [SerializeField]
    private GameObject loadScene;
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private GameObject computer_function;


    void Start()
    {
        
        Medic_door = GameObject.Find("beginRoom/SM_Bld_Wall_Doorframe_02 (10)/SM_Bld_Wall_Door_01");
        medic_interact_with_door = GameObject.Find("beginRoom/interact_with_door_cube");
        loadScene = GameObject.Find("loadScene");
        gameOver = GameObject.Find("dont_destroy/Canvas_level_one/game_lost");
        restAreaDoorState = GameObject.Find("beginRoom/corridor/Blue Vortex/Cube");
        computer_function = GameObject.Find("dont_destroy/Computer_functions");
        if (Medic_door != null) {
            Medic_door_animator= Medic_door.GetComponent<Animator>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (theMission.Count>0) {
            foreach (interactionData interaction in theMission.ToArray()) {


                if (interaction.location=="medic") {
                    if (interaction.mainContent=="doorOpen") {

                        if (Medic_door_animator != null)
                        {
                            Medic_door_animator.SetInteger("open", 1);
                            if (medic_interact_with_door != null)
                            {
                                medic_interact_with_door.GetComponent<lock_aniactivate>().openState = false;
                                showTask(2f,10f,"Task 2","Define a pressButton method and open the door.");
                            }
                        }

                    }
                }

                if (interaction.location == "frontDoor") {
                    if (interaction.mainContent=="bugfixed") {
                        //front door open is enable
                    }
                    else if (interaction.mainContent == "open") {
                        restAreaDoorState.GetComponent<level1_frontdoor_button>().openFrontDoor();
                        Debug.Log("rest area door is opened");
                    }
                    else if (interaction.mainContent == "close")
                    {
                        restAreaDoorState.GetComponent<level1_frontdoor_button>().closeFrontDoor();
                        Debug.Log("rest area door is close");
                    }
                }
                theMission.Remove(interaction);
            }
        }
        /*
        if (tcpTerminal.MedicDoorOpen) {
            if (Medic_door_animator!=null) {
                Medic_door_animator.SetInteger("open",1);
                if (medic_interact_with_door != null)
                {
                    medic_interact_with_door.GetComponent<lock_aniactivate>().openState=false;
                } 
            }
        }
        */
    }

    public void game_lost()
    {
        if (gameOver!=null) {
            if (computer_function.GetComponent<level0_computer_functions>().computer_open) {
                computer_function.GetComponent<level0_computer_functions>().shutdown_computer();
                Debug.Log("shutdo");
            }
            gameOver.gameObject.GetComponent<ModalWindowManager>().OpenWindow();
            Time.timeScale = 0;
        }
    }

    public void showTask(float time,float exetime, string title, string description) {
        loadScene.GetComponent<level0_load_scene>().showTaskPanel(time,exetime,title,description);
    }
}
