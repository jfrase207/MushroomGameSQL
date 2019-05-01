using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

/// <summary>
/// This class deals with the load scene it creates the players on the buttons where the information is retrieved from the database
/// </summary>
public class Load : MonoBehaviour {

    //singleton
    public static Load current;
    //the list of players
    public List<Player> Players = new List<Player>();
    //the list off buttons being used on the loading screen
    public List<GameObject> buttons = new List<GameObject>();
    
    // Use this for initialization
    void Start () {
        current = this;
        GetPlayers();
	}	

    /// <summary>
    /// This method retrieves all the different players and their data from the database
    /// </summary>
    public void GetPlayers()
    {
        //fisrt clear the list
        Players.Clear();
        using (var conn = new SqliteConnection("URI=file:" + Application.persistentDataPath + "/exampleDatabase.db"))
        {
            //open connection
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                //create query
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Player;";

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //read the players and each players data and add them to a player classs then add the player class to the list
                    Player player = new Player
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        position = new Vector3(reader.GetFloat(2), reader.GetFloat(3), reader.GetFloat(4)),
                        playerColor = new Vector4(reader.GetFloat(5), reader.GetFloat(6), reader.GetFloat(7),1),                        
                        energy = reader.GetFloat(8)
                    };                   
                    //add player to the list                                 
                    Players.Add(player);                    
                }

                AddPlayerButtons();
                
            }
        }
    }

    /// <summary>
    /// This methid adds the loaded in player list to the 5 different buttons on the load screen
    /// </summary>
    public void AddPlayerButtons()
    {
        int index = 0;
        //loop through buttons to clear them of current datas
        foreach (var button in buttons)
        {
            button.GetComponentInChildren<Text>().text = "NEW GAME";
            button.GetComponentInChildren<UIEvents>().player = null;
        }
        //loop through player list and add each player that exists to one of the 5 buttons.
        foreach (var player in Players)
        {
            Debug.Log(index);
            buttons[index].GetComponent<Button>().interactable = true;
            buttons[index].GetComponentInChildren<Text>().text = player.name;
            buttons[index].GetComponentInChildren<UIEvents>().player = player;
            index++;
        }

    }

}
/// <summary>
/// This Class is used to create a player and all the information needed for the players data to persist
/// </summary>
[System.Serializable]
public class Player
{
    public int id;
    public string name;
    public Vector3 position;
    public Vector4 playerColor;
    public float energy;
}