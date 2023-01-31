using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;

/// <summary>
/// Takes care of the Menu UI, summoned with the right palm + gaze
/// 
/// Known Limitations:
/// Right now the save/load button do nothing, but in the future:
/// Save button -> open panel that allows you to save the current objects of scene into the filesystem
/// Load button -> panel that allows you to choose one of the saved objects and load into scene
/// </summary>
public class UiButtons : MonoBehaviour
{
    /// <summary>
    /// Enum should be better integrated with the int input for the play/pause press
    /// </summary>
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
    /// <summary>
    /// Switch between play and pause states
    /// </summary>
    /// <param name="option_int">Chooses the command using the enum (int)</param>
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
    /// <summary>
    /// To be implemented, see known limitations
    /// </summary>
    public void onLoadButtonPressed() {
        Debug.Log("Not implemented");
    }
    /// <summary>
    /// To be implemented, see known limitations
    /// </summary>
    public void onSaveButtonPressed() { 
        Debug.Log("Not implemented");

    }

}
