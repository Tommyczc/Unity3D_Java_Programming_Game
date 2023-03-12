using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace javaCompiler
{
    public class tcpTerminal : MonoBehaviour
    {
        // Start is called before the first frame update
        public static bool MedicDoorOpen=false;
        public static bool frontDoor_bugfix=false;
        public static bool restArea_doorOpen=false;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void operationByTerminal(string  room,string operation) {
            room =room.Trim();
            operation=operation.Trim();
            switch (room)
            {
                case "medic":
                    if (operation == "doorOpen") {
                        if (!MedicDoorOpen) {
                            //GameObject interaction = GameObject.Find("Allinteraction");
                            MedicDoorOpen = true;
                            interactionData interaction = new interactionData();
                            interaction.location = room; interaction.mainContent = operation;

                            level1_allInteraction.theMission.Add(interaction);
                            //level1_allInteraction.theMission = (List<interactionData>)ArrayList.Synchronized(level1_allInteraction.theMission);
                            //Debug.Log("medic door open");
                        }
                    }
                    break;

                case "frontDoor":
                    if (operation == "bugfixed") {
                        level1_frontdoor_button.isSolved = true;
                        frontDoor_bugfix = true;
                    }
                    else if (operation == "open") {
                        if (!restArea_doorOpen) {
                            interactionData interaction = new interactionData();
                            interaction.location = room; interaction.mainContent = operation;

                            level1_allInteraction.theMission.Add(interaction);
                            level1_allInteraction.theMission = (List<interactionData>)ArrayList.Synchronized(level1_allInteraction.theMission);
                            restArea_doorOpen = true;
                        }
                    }
                    else if (operation=="close") {
                        if (restArea_doorOpen)
                        {
                            interactionData interaction = new interactionData();
                            interaction.location = room; interaction.mainContent = operation;

                            level1_allInteraction.theMission.Add(interaction);
                            level1_allInteraction.theMission = (List<interactionData>)ArrayList.Synchronized(level1_allInteraction.theMission);
                            restArea_doorOpen =false;
                        }
                    }

                    break;
            }
        }

        public static void updateAllState() { 
            MedicDoorOpen=false;
            frontDoor_bugfix = false;
            restArea_doorOpen = false;
        }
        
    }
}
