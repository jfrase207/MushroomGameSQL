using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// This class deals with all the player behaviours including loading saving player data.
/// </summary>
public class PlayerBehaviour : SteeringBehaviours
{
    //singleton variable
    public static PlayerBehaviour current;
    
    //player varibales
    public float speed;
    float turnSpeed = 40;
    public Vector4 playerColor = new Vector4(1,1,1,1);
    public static bool dead = false;
    
    // Use this for initialization
    void Start()
    {
        //sigleton
        current = this;    
        //if the game is loaded players details are updated from the persisten data object
        if(PersistentData.current.loaded == true)
        {
            playerColor = PersistentData.current.playerColor;
            transform.position = PersistentData.current.playerPos;
            GameLogic.score = PersistentData.current.pScore;            
        }
    }

    void FixedUpdate()
    {
        //the players movement behaviour
        if (!dead)
        {          
            GetComponent<Renderer>().material.color = playerColor;
            WorldWrap();
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);           
        }
    }

    /// <summary>
    /// When th application quites the enemy data is saved if the game was not loaded or updated to the database if it was
    /// </summary>
    private void OnApplicationQuit()
    {
        if (!PersistentData.current.loaded)
            SQLFunctions.current.SavePlayer();
        else
            SQLFunctions.current.UpdatePlayer();
    }
    // the players collision
    void OnCollisionEnter(Collision col)
    {
        //check if the collided object has the tag of blackhole if so reset player variables
        if (col.gameObject.tag == "Blackhole")
        {
            playerColor = Vector4.one;
            GameLogic.currrent.gameOver.SetActive(true);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            dead = true;
        }
        //if player collides with a object tagged mushroom the mushroom is destroyed and the player colour changes through each
        //of the rgb as the game progress's
        if (col.gameObject.tag == "Mushroom")
        {            
            Destroy(col.gameObject);
            GameLogic.score++;           
            if (GetComponent<Renderer>().material.color.g >= 0)
                playerColor.y -= 0.2f;
            else if (GetComponent<Renderer>().material.color.r >= 0)
                playerColor.x -= 0.2f;
            else if (GetComponent<Renderer>().material.color.b >= 0)
                playerColor.z -= 0.2f;

        }
        //if the collided object is tagged poisonous then the object is destroyed and score reduced also the players red colour is affected 
        if (col.gameObject.tag == "Poisonous")
        {
            Destroy(col.gameObject);
            GameLogic.score--;           
            playerColor.x += 0.2f;
        }
    }
   

}
