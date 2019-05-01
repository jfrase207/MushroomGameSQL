using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// This function handles the majority of the sql functions and commands
/// </summary>
public class SQLFunctions : MonoBehaviour
{
    //declare sigleton and necesarry variables
    public static SQLFunctions current;
    public static string dbPath;    
    string HighScoreTable;
    public bool clearTables;

    //on start the database path is defined and tables are created
    private void Start()
    {
        current = this;
        dbPath = "URI=file:" + Application.persistentDataPath + "/exampleDatabase.db";        
        CreatePlayerTable();
        CreateEnemySchema();
        CreateMushroomSchema();
        Debug.Log(dbPath);       
        new SqliteConnection();    
    }

    private void Update()
    {
        //if this is true all tables are dropped from the database
        if (clearTables)
        {
            clearTable();
            clearTables = false;
        }      
    }
    
    /// <summary>
    /// This methods creates the table for the player in the database
    /// </summary>
    public void CreatePlayerTable()
    {
        //connection to database created and opened
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            //the sql command is defined and executed
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Player' ( " +
                                  "  'id' INTEGER PRIMARY KEY, " +
                                  "  'player' TEXT NOT NULL, " +
                                  "  'positionX' FLOAT NOT NULL," +
                                  "  'positionY' FLOAT NOT NULL," +
                                  "  'positionZ' FLOAT NOT NULL," +
                                  "  'colorR' FLOAT NOT NULL," +
                                  "  'colorG' FLOAT NOT NULL," +
                                  "  'colorB' FLOAT NOT NULL," +
                                   "  'energy' FLOAT NOT NULL" +
                                  ");";

                var result = cmd.ExecuteNonQuery();                
            }
        }  
    }

    /// <summary>
    /// This methods creates the table for the enemy in the database
    /// </summary>
    public void CreateEnemySchema()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'EnemyTable' ( " +
                                  "  'id' INTEGER PRIMARY KEY, " +
                                  "  'player' TEXT NOT NULL, " +                                  
                                  "  'energy' FLOAT NOT NULL, " +
                                  "  'positionX' FLOAT NOT NULL," +
                                  "  'positionY' FLOAT NOT NULL," +
                                  "  'positionZ' FLOAT NOT NULL" +                             
                                  ");";

                var result = cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// This methods creates the table for the mushrooms in the database
    /// </summary>
    public void CreateMushroomSchema()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'MushroomTable' ( " +
                                  "  'id' INTEGER PRIMARY KEY, " +
                                  "  'player' TEXT NOT NULL, " +
                                  "  'mushroomname' TEXT NOT NULL, " +
                                  "  'age' FLOAT NOT NULL," +
                                   "  'positionX' FLOAT NOT NULL," +
                                  "  'positionY' FLOAT NOT NULL," +
                                  "  'positionZ' FLOAT NOT NULL" +                                
                                  ");";

                var result = cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// this saves a new player to the database
    /// </summary>
    public void SavePlayer()
    {
        Debug.Log("Player Saved");
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                //the command inserts all player data into the database
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Player (player, positionX, positionY, PositionZ, colorR, colorG, colorB, energy) VALUES (@Player, @PositionX, @PositionY, @PositionZ, @ColorR, @ColorG, @ColorB, @Energy);";

                //this defines an sql parameter that can be more easily assigned to
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Player",
                    Value = PersistentData.current.playerName              
                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionX",
                    Value = PlayerBehaviour.current.gameObject.transform.position.x

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionY",
                    Value = PlayerBehaviour.current.gameObject.transform.position.y

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionZ",
                    Value = PlayerBehaviour.current.gameObject.transform.position.z

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ColorR",
                    Value = PlayerBehaviour.current.gameObject.GetComponent<Renderer>().material.color.r

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ColorG",
                    Value = PlayerBehaviour.current.gameObject.GetComponent<Renderer>().material.color.g

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ColorB",
                    Value = PlayerBehaviour.current.gameObject.GetComponent<Renderer>().material.color.b

                });

                cmd.Parameters.Add(new SqliteParameter {

                    ParameterName = "Energy",
                    Value = GameLogic.score

                });
                var result = cmd.ExecuteNonQuery();                
            }
        }
    }
    /// <summary>
    /// This method updates the player based on the information held in the persistent data object
    /// </summary>
    public void UpdatePlayer()
    {
        Debug.Log("Player Updated");
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Player SET player=@Player, positionX=@PositionX, positionY=@PositionY, positionZ=@PositionZ, colorR=@ColorR, colorG=@ColorG, colorB=@ColorB, energy=@Energy WHERE player=@Player;";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Player",
                    Value = PersistentData.current.playerName                   
                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionX",
                    Value = PlayerBehaviour.current.gameObject.transform.position.x

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionY",
                    Value = PlayerBehaviour.current.gameObject.transform.position.y

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionZ",
                    Value = PlayerBehaviour.current.gameObject.transform.position.z

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ColorR",
                    Value = PlayerBehaviour.current.gameObject.GetComponent<Renderer>().material.color.r

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ColorG",
                    Value = PlayerBehaviour.current.gameObject.GetComponent<Renderer>().material.color.g

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "ColorB",
                    Value = PlayerBehaviour.current.gameObject.GetComponent<Renderer>().material.color.b

                });

                cmd.Parameters.Add(new SqliteParameter
                {

                    ParameterName = "Energy",
                    Value = GameLogic.score

                });

                var result = cmd.ExecuteNonQuery();                
            }
        }
    }

    /// <summary>
    /// This method saves all the enemy data to the database
    /// </summary>
    public void SaveEnemy()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO EnemyTable (player, positionX, positionY, PositionZ, energy) VALUES (@Player, @PositionX, @PositionY, @PositionZ, @Energy);";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Player",
                    Value = PersistentData.current.playerName                
            });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionX",
                    Value = Enemy.current.gameObject.transform.position.x

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionY",
                    Value = Enemy.current.gameObject.transform.position.y

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionZ",
                    Value = Enemy.current.gameObject.transform.position.z

                });

                cmd.Parameters.Add(new SqliteParameter
                {

                    ParameterName = "Energy",
                    Value = Enemy.current.StolenEnergy

                });

                var result = cmd.ExecuteNonQuery();                
            }
        }
    }

    /// <summary>
    /// this function updates all the enemy data based on what the persistent data object is holding
    /// </summary>
    public void UpdateEnemy()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE EnemyTable SET player=@Player, positionX=@PositionX, positionY=@PositionY, PositionZ=@PositionZ, energy=@Energy WHERE player=@Player;";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Player",
                    Value = PersistentData.current.playerName                    
                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionX",
                    Value = Enemy.current.gameObject.transform.position.x

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionY",
                    Value = Enemy.current.gameObject.transform.position.y

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionZ",
                    Value = Enemy.current.gameObject.transform.position.z

                });

                cmd.Parameters.Add(new SqliteParameter
                {

                    ParameterName = "Energy",
                    Value = Enemy.current.StolenEnergy

                });

                var result = cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// This method saves a single mushroom to the database
    /// </summary>
    /// <param name="_name">This is the name of the mushroom</param>
    /// <param name="_position">This is the mushrooms position</param>
    /// <param name="_age">The mushrooms age</param>
    public void SaveMushrooms(string _name ,Vector3 _position, float _age)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO MushroomTable (player, positionX, positionY, PositionZ, age, mushroomname) VALUES (@Player, @PositionX, @PositionY, @PositionZ, @Age, @Mushroomname);";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Player",
                    Value = PersistentData.current.playerName                    
                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionX",
                    Value = _position.x

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionY",
                    Value = _position.y

                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "PositionZ",
                    Value = _position.z

                });

                cmd.Parameters.Add(new SqliteParameter
                {

                    ParameterName = "Age",
                    Value = _age

                });

                cmd.Parameters.Add(new SqliteParameter
                {

                    ParameterName = "Mushroomname",
                    Value = _name

                });

                var result = cmd.ExecuteNonQuery();                
            }
        }
    }

    /// <summary>
    /// This method removes a players information from a specific table.
    /// </summary>
    /// <param name="tableName">This is the desired table that the player is to be removed from as a string</param>
    /// <param name="playername">This the player to be removed from the table</param>
    public void DeletePlayer(string tableName, string playername)
    {        
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM " + tableName + " WHERE player=@Player;";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Player",
                    Value = playername                    
                });
                var result = cmd.ExecuteNonQuery();

            }
        }
    }
    /// <summary>
    /// This Function returns a string consisting of the highscores based on the limit set
    /// </summary>
    /// <param name="limit">The amount of high scores to be retrieved</param>
    /// <returns>A string comprised of all the data for the high scores table</returns>
    public string GetHighScores(int limit)
    {        
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Player ORDER BY energy DESC LIMIT @Count;";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Count",
                    Value = limit
                });
               
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var highScoreName = reader.GetString(1);
                    var score = reader.GetFloat(8);                    
                    var text = string.Format("Name: {0}           Score: {1}", highScoreName, score);
                    HighScoreTable += text + "\n";                    
                }
               
            }
        }
        return HighScoreTable;
    }
    /// <summary>
    /// This methid drops all the tables from the database
    /// </summary>
    public void clearTable()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DROP TABLE Player";

                var result = cmd.ExecuteNonQuery();
                Debug.Log("TableDropped");
            }
        }

        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DROP TABLE EnemyTable";

                var result = cmd.ExecuteNonQuery();
                Debug.Log("TableDropped");
            }
        }

        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DROP TABLE MushroomTable";

                var result = cmd.ExecuteNonQuery();
                Debug.Log("TableDropped");
            }
        }

    }
   
}

