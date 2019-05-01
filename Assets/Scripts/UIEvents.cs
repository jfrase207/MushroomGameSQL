using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// This class deals with all the User Interface function mainly used for UI buttons
/// </summary>
public class UIEvents : MonoBehaviour
{
    //The button varibales needed
    public bool StartButton;
    Text nameField;
    public Player player = null;
    //A modal window popup
    public GameObject Modal;

    /// <summary>
    /// when the script is enabled it finds the current 
    /// </summary>
    private void OnEnable()
    {
        player = null;
    }

    private void Update()
    {
        //if its the start button this is attached to then it prevents the player interacting with the button until a name is entered
        if (StartButton)
            if(nameField.text == "")
                GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// If the player assigned to this script is not null it is added to the persisten data when the button is pressed
    /// else the modal popup is activated telling the player to create a new plaer.
    /// </summary>
    public void SetPlayer()
    {        
        if (player != null)
        {
            Debug.Log("happening");
            PersistentData.current.player = player;
            PersistentData.current.LoadGame();
            SceneOpener("Game");
        }
        else
        {
            Modal.SetActive(true);           
        }
    }
    /// <summary>
    /// This allows a scene to be opened based on its name
    /// </summary>
    /// <param name="_scene">This is the name of the scene to be loaded</param>
    public void SceneOpener(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
    /// <summary>
    /// This starts a new game then adds the players name to the persistent data object and opens the game scene
    /// </summary>
    public void StartNewGame()
    {        
            PersistentData.current.GetName();
            SceneOpener("Game");      
    }
    /// <summary>
    /// This function deletes a player from the databases tables and removes them from the player list then refreshes the player list.
    /// </summary>
    public void DeletePlayer()
    {
        if (player != null)
        {            
            SQLFunctions.current.DeletePlayer("Player", player.name);
            SQLFunctions.current.DeletePlayer("EnemyTable", player.name);
            SQLFunctions.current.DeletePlayer("MushroomTable", player.name);
            Load.current.Players.Remove(player);
            player = null;
            Load.current.GetPlayers();         
        }
    }
   
}

