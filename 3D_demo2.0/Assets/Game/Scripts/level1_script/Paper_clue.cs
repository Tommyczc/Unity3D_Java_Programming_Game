using Michsky.UI.MTP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper_clue : MonoBehaviour
{
    public string theText;
    public GameObject thePaper;
    public GameObject canvas;
    private Animator anim;
    private GameObject textObject;
    private GameObject login_layer;
    // Start is called before the first frame update
    void Start()
    {
        anim = canvas.GetComponent<Animator>();
        anim.SetInteger("UI_open", 0);
        canvas.SetActive(false);


        Transform[] myTransforms = canvas.gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in myTransforms)
        {
            if (child.name == "Text") {
                textObject = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (textObject!=null) {
            if (textObject.GetComponent<TextItem>().text != theText)
            {
                textObject.GetComponent<TextItem>().text = theText;
                textObject.GetComponent<TextItem>().UpdateText();
            }
            canvas.SetActive(true);
            anim.SetInteger("UI_open", 1);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (thePaper!=null && login_layer==null) {
                login_layer = Instantiate(thePaper, null, true);
                GameObject canvas = GameObject.Find("Canvas_level_two");
                login_layer.transform.parent = canvas.transform;
                login_layer.transform.localScale = Vector2.one;     // original size
                login_layer.transform.localPosition = Vector3.zero; // the 

                anim.SetInteger("UI_open", 2);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (textObject != null)
        {
            if (textObject.GetComponent<TextItem>().text != theText)
            {
                textObject.GetComponent<TextItem>().text = theText;
                textObject.GetComponent<TextItem>().UpdateText();
            }
            if (anim.GetInteger("UI_open") != 2) { 
                anim.SetInteger("UI_open", 2); 
            }
            
            //canvas.SetActive(false);
            
        }
    }
}
