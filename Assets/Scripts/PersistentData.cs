using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

/// <summary>
/// This class is used to hold the persistent data such as positions and energy for the player and enemy 
/// </summary>
public class PersistentData : MonoBehaviour
{
    //declare static variables
    public static PersistentData current;
    public static bool created;

    //declare the players variables
    public Player player;
    public string playerName;    
    public Vector3 playerPos;
    public Vector4 playerColor;
    public float pScore;

    //declare the enemy variables
    public Vector3 enemyPos;
    public float enemyEnergy;

    //if true the game has been loaded from the database
    public bool loaded;

    private void Awake()
    {
        //This creates an object that will not be destroyed when switching between scenes
        if (!created)
        {
            current = this;
            DontDestroyOnLoad(transform.gameObject);
            created = true;
        }
    }
    /// <summary>
    /// This function returns a string from the input field the player enters there name from when starting the game
    /// </summary>
    public string GetName()
    {
        return playerName = GameObject.FindGameObjectWithTag("NameField").GetComponent<Text>().text;
    }

    /// <summary>
    /// This method contains the sql queries necessary to load in the player and the enemy then stores the variables.
    /// </summary>
    public void LoadGame()
    {
        loaded = true;
        playerName = player.name;
        //this creates the sql connection to the database
        using (var conn = new SqliteConnection(SQLFunctions.dbPath))
        {
            //opens connection to database
            conn.Open();
            //crates a command to be run in sql
            using (var cmd = conn.CreateCommand())
            {
                //defines the type of command and the sql command itself
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Player WHERE player='" + playerName + "';";

                //creates a reader to extract the data from the database
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //extracts the necessary information from the database ofr player
                    playerPos = new Vector3(reader.GetFloat(2), reader.GetFloat(3), reader.GetFloat(4));
                    playerColor = new Vector4(reader.GetFloat(5), reader.GetFloat(6), reader.GetFloat(7));
                    pScore = reader.GetFloat(8);
                }

            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM EnemyTable WHERE player='" + playerName + "';";

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    enemyEnergy = reader.GetFloat(2);
                    enemyPos.x = reader.GetFloat(3);
                    enemyPos.y = reader.GetFloat(4);
                    enemyPos.z = reader.GetFloat(5);

                }

            }
            //loads in the mushrooms for the player.
            LoadMush(playerName);

        }

    }

    /// <summary>
    /// This function loads in the mushrooms from the database
    /// </summary>
    /// <param name="playerName">This is the current players mushrooms to be loaded</param>
    public void LoadMush(string playerName)
    {
        var i = 0;
        using (var conn = new SqliteConnection(SQLFunctions.dbPath))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM MushroomTable WHERE player='" + playerName + "';";

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    GameLogic.Mushrooms("Mushroom" + i, PrimitiveType.Cube, Color.yellow, "Mushroom", new Vector3(reader.GetFloat(4), reader.GetFloat(5), reader.GetFloat(6)), reader.GetFloat(3), true);
                    PersistentData.current.loaded = true;
                    i++;
                }                
                SQLFunctions.current.DeletePlayer("MushroomTable", playerName);
            }
        }

    }
}
