using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// This classs deals with all the games main logic such as loading in the mushrooms and dealing with the player score.
/// </summary>
public class GameLogic : MonoBehaviour {

    //declare singleton variable and statics
    public static GameLogic currrent;
    public static float score;

    //decalred public GameObjects
    public GameObject ground;
    public GameObject gameOver;

    //varibaled for world size
    [HideInInspector]
    public float groundX;
    [HideInInspector]
    public float groundZ;
    
    //UI variables
    public Text scoreText;    
    
    //declare int variables for  
    public int obstacleAmount;
    public int collectableAmount;
    
    public LayerMask layer;
    
    List<GameObject> items;

    // Use this for initialization
    void Awake () {
        //assign singleton
        currrent = this;
       
        //get terrain dimensions
        groundX = ground.GetComponent<Renderer>().bounds.size.x / 2;
        groundZ = ground.GetComponent<Renderer>().bounds.size.z / 2;
        
        //if the game has not been loaded in then spawn the deault amount of mushrooms
        if (!PersistentData.current.loaded)
        {            
            items = worldObjects(collectableAmount);
        }
    }
	
	// Update is called once per frame
	void Update () {

        //spawn more Mushrooms when there is less than 5 on the field       
        if(GameObject.FindGameObjectsWithTag("Mushroom").Length < 5)
        {
            items = worldObjects(15);
        }       
    }

    private void LateUpdate()
    {
        //update the players score
        scoreText.text = "Score : "+ score.ToString();
    }

    /// <summary>
    /// This resets the players position, the mushrooms and shows the gameover screen when the reset button is pressed.
    /// </summary>
    public void gameReset()
    {        
        PlayerBehaviour.current.gameObject.GetComponent<MeshRenderer>().enabled = true;
        PlayerBehaviour.dead = false;
        gameOver.SetActive(false);
        PlayerBehaviour.current.gameObject.transform.position = new Vector3(-10, 0.5f, -10);
        
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();
        items = worldObjects(collectableAmount);
        score = 0;       
    }

    /// <summary>
    /// This creates the list of objects in the game in this case just the mushrooms
    /// </summary>
    /// <param name="_colectableAmount"> The amount of collectables to be spawned</param>
    /// <returns></returns>
    private List<GameObject> worldObjects(float _colectableAmount)
    {
        //Create a new local array list. 
        List<GameObject> itemsCreated = new List<GameObject>();// = new ArrayList();

        for (int j = 0; j < _colectableAmount; j++)
        {
            itemsCreated.Add(Mushrooms("Mushroom" + j, PrimitiveType.Cube, Color.yellow, "Mushroom",new Vector3(Random.Range(-groundX, groundX), 0, Random.Range(-groundZ, groundZ))));
        }
        // Return the local array lists
        return itemsCreated;
    }

    /// <summary>
    /// This function creates a default mushroom
    /// </summary>
    /// <param name="name"> The mushroom name</param>
    /// <param name="primitive">The type of primitive ie cube, sphere</param>
    /// <param name="color">The default colour of the object</param>
    /// <param name="tag">The objects tag to be used for acessing the array of objects</param>
    /// <param name="position">The position of the mushroom in the game</param>
    /// <returns></returns>
    private GameObject Mushrooms(string name, PrimitiveType primitive, Color color, string tag , Vector3 position)
    {       
        GameObject go = GameObject.CreatePrimitive(primitive);       
        go.transform.position = position; 
        go.GetComponent<Renderer>().material.color = color;      
        go.name = name;
        go.tag = tag;
        go.layer = 10;
        //Add the mushroom script to the created mushroom objects
        go.AddComponent<Mushroom>();
        // Return the item created
        return go;
    }

    /// <summary>
    /// This function overloads the mushroom function and creates the mushrooms to be loaded from the database
    /// </summary>
    /// <param name="name"> The mushroom name</param>
    /// <param name="primitive">The type of primitive ie cube, sphere</param>
    /// <param name="color">The default colour of the object</param>
    /// <param name="tag">The objects tag to be used for acessing the array of objects</param>
    /// <param name="position">The position of the mushroom in the game</param>
    /// <param name="age">The age is taken from the database and applied to the mushroom when loaded</param>
    /// <param name="loaded">This defines wether or not the mushroom has been loaded in or is a default one</param>
    /// <returns></returns>
    public static GameObject Mushrooms(string name, PrimitiveType primitive, Color color, string tag, Vector3 position, float age, bool loaded)
    {        
        GameObject go = GameObject.CreatePrimitive(primitive);       
        go.transform.position = position;        
        go.GetComponent<Renderer>().material.color = color;       
        go.name = name;
        go.tag = tag;
        go.layer = 10;
        //Add the mushroom script to the created mushroom objects
        go.AddComponent<Mushroom>();
        go.GetComponent<Mushroom>().thisAge = age;
        go.GetComponent<Mushroom>().loadedMushroom = loaded;      
        return go;
    }   

   

}
