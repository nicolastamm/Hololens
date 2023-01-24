using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButtons : MonoBehaviour
{
    enum State { Playing, Paused };
    State currentState;
    [Serializable]
    public enum Option { Play = 1, Pause = 2, Toggle = 3 };
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
    public void onPlayButtonPressed(int option_int) {
        Option option = (Option)option_int;
        if (this.currentState == State.Paused && (option == Option.Play || option == Option.Toggle))
        {
            this.currentState = State.Playing;
            this.ps.Play();

            this.PlayButton.GetComponent<ButtonConfigHelper>().MainLabelText = "Pause";
            this.PlayButton.GetComponent<ButtonConfigHelper>().SetQuadIconByName("IconClose");

        }
        else if(this.currentState == State.Playing && (option == Option.Pause || option == Option.Toggle))
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
