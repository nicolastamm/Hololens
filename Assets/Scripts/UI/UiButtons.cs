using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButtons : MonoBehaviour
{   
    enum State { Playing, Paused};
    State currentState;
    public PhysicsSimulation ps;
    public GameObject PlayButton;
    public GameObject LoadButton;
    public GameObject SaveButton;
    // Start is called before the first frame update
    void Start()
    {
        this.currentState = State.Paused;
        this.PlayButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Play";
        this.PlayButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconDone");
    }
    public void onPlayButtonPressed() { 
        if(this.currentState == State.Paused)
        {
            this.currentState = State.Playing;
            this.ps.Play();

            this.PlayButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Pause";
            this.PlayButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconClose");

        }
        else
        {
            this.currentState = State.Paused;
            this.ps.Pause();

            this.PlayButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Play";
            this.PlayButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconDone");

        }
    }
    public void onLoadButtonPressed() {
        Debug.Log("Not implemented");
    }
    public void onSaveButtonPressed() { 
        Debug.Log("Not implemented");

    }

}
