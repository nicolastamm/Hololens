using UnityEngine;
using TMPro;
/// <summary>
/// Tutorial script, spawned at game start.
/// It explains the game to the player through text.
/// Supports voice commands.
/// 
/// Known limitations:
/// Right now the tutorial is text only, in the future integrate hand coach to also visual explain the gestures,
/// and automatically go to next text after the gesture is applied by user
/// </summary>

public class TutorialScript : MonoBehaviour
{
    
    public GameObject YesButton;
    public GameObject NoButton;
    public GameObject NextButton;
    public TextMeshProUGUI Text;

    private int index;
    private void Start()
    {
        index = 0;
    }
    /// <summary>
    /// If yes is pressed, we start the tutorial text
    /// </summary>
    public void onYesPressed()
    {
        this.YesButton.SetActive(false);
        this.NoButton.SetActive(false);
        this.Next();
    }
    /// <summary>
    /// If no is pressed, we disable the tutorial object
    /// </summary>
    public void onNoPressed()
    {
        this.YesButton.SetActive(false);
        this.NoButton.SetActive(false);

        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// Iterates through the tutorial, until it reaches the end and disables the tutorial
    /// </summary>
    public void Next()
    {
        this.NextButton.SetActive(true);
        if (index == 0)
        {
            Text.text = "<size=42><b>Game Description</b></size>\r\n\r\nWelcome to \"VirtualArchitects\". Engineer virtual buildings in AR with your friends!";
            index++;
        }
        else if (index == 1)
        {
            Text.text = "<size=42><b>Toolbox</b></size>\r\n\r\nThe toolbox is the most important part of your engineering experience. To teleport it, stretch your left hand forward and focus your gaze on your open palm.";
            index++;
        }
        else if(index == 2)
        {
            Text.text = "<size=42><b>Voice Commands</b></size>\r\n\r\nGreat, you teleported the toolbox on your hand. Feel free to move the hand away and the toolbox will remain in the same place.\r\nAdditionally, you can use voice commands. Say \"Toolbox\" to teleport using your voice.";
            index++;
        }
        else if(index == 3)
        {
            Text.text = "<size=42><b>Voice Commands</b></size>\r\n\r\nVoice commands are available for all the buttons. Try saying \"Next\" to move on to the next part of the tutorial.";
            index++;
        }
        else if(index == 4)
        {
            Text.text = "<size=42><b>Toolbox</b></size>\r\n\r\nInside the toolbox you will find the primitive objects for construction. Dragging them out of the toolbox, spawns them in the virtual world.";
            index++;
        }
        else if(index == 5)
        {
            Text.text = "<size=42><b>Toolbox</b></size>\r\n\r\nYou can also change the appearance of the object before spawning it, using the objects to the right of the toolbox.";
            index++;
        }
        else if(index == 6)
        {
            Text.text = "<size=42><b>Toolbox</b></size>\r\n\r\nYou can also change the color of the object before spawning it, by pressing the squished Cylinder in the top left.";
            index++;
        }
        else if(index == 7)
        {
            Text.text = "<size=42><b>Toolbox</b></size>\r\n\r\nAfter spawning an object, you can duplicate or remove it by dragging it into one of the boxes in the bottom or middle of the toolbox.";
            index++;
        }
        else if(index == 8)
        {
            Text.text = "<size=42><b>Gameplay</b></size>\r\n\r\nStack the object to create the desired building. Once you are done, say \"Start Simulation\" to start a real simulation.";
            index++;
        }
        else if(index == 9)
        {
            Text.text = "<size=42><b>Gameplay</b></size>\r\n\r\nYou can say \"Stop Simulation\" to stop the simulation and modify your building.";
            index++;
        }
        else if(index == 10)
        {
            Text.text = "<size=42><b>Menu</b></size>\r\n\r\nYou can find the Play/Pause, as well as additional functionality in the Menu. To summon the menu, focus your gaze on the open right palm.";
            index++;
        }
        else if(index == 11)
        {
            this.gameObject.SetActive(false);
        }

    }

}
