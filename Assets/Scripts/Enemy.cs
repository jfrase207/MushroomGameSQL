using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the enemy actions and data
/// </summary>
public class Enemy : SteeringBehaviours
{
    //create singleton
    public static Enemy current;
    //This is the type of states the enemy has
    public enum EnemyState
    {
        IDLE,
        ATTACK,
        RETREAT
    }
    //this holds the enemy's current state
    public EnemyState state;
    //energy and colour variables
    public  float StolenEnergy = 1;
    float currentAmountStolen;
    float stolenColor;
    
    private void Awake()
    {
        //assign singleton
        current = this;
        //if the game has been loaded in the enemy data is retrieved from the persistendata object then assinged to the enemy
        if (PersistentData.current.loaded == true)
        {
            transform.position = PersistentData.current.enemyPos;
            StolenEnergy = PersistentData.current.enemyEnergy;
        }
    }
    /// <summary>
    /// When th application quites the enemy data is saved if the game was not loaded or updated to the database if it was
    /// </summary>
    private void OnApplicationQuit()
    {
        if (!PersistentData.current.loaded)
            SQLFunctions.current.SaveEnemy();
        else
            SQLFunctions.current.UpdateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        //world wraps the enemy movement
        WorldWrap();    
        //the switch statement deals with the enemy state
        switch(state)
        {
            //when the enemy is idle the enemy wanders around the game field until the player is within 15 distance then the state changes to attack
            case EnemyState.IDLE:
                {
                    MoveUnit(Wander() + collisionAvoidance(), 3f);

                    if (Vector3.Distance(PlayerBehaviour.current.gameObject.transform.position, transform.position) < 15)
                        state = EnemyState.ATTACK;

                    break;
                }
             //when the enemy is in the attack state the enemy seeks the player out 
             //once close enought the enemy steals a fixed amount of energy
             //once it stolen that amount it updates the player and score variables
             //then the state changes to retreat.
            case EnemyState.ATTACK:
                {
                    MoveUnit(Seek(PlayerBehaviour.current.gameObject.transform.position) + collisionAvoidance(), 10f);
                    if (Vector3.Distance(PlayerBehaviour.current.gameObject.transform.position, transform.position) < 2)
                        currentAmountStolen -= 0.1f;

                    if (currentAmountStolen <= -0.2f)
                    {
                        GameLogic.score--;
                        GameLogic.currrent.scoreText.text = "Score: " + GameLogic.score;
                        PlayerBehaviour.current.playerColor.z -= 0.2f;
                        StolenEnergy += currentAmountStolen;
                        currentAmountStolen = 0;
                        state = EnemyState.RETREAT;
                    }


                    break;
                }
            //When in the retreat state the enemy flees away from the player unitl far enough away then state changes to idle again
            case EnemyState.RETREAT:
                {
                    MoveUnit(Flee(PlayerBehaviour.current.transform.position), 15f);

                    if (Vector3.Distance(PlayerBehaviour.current.gameObject.transform.position, transform.position) > 20)
                    {
                        state = EnemyState.IDLE;
                    }

                    break;
                }                
        }        
        //the enemy colour is updated
        GetComponent<Renderer>().material.color = new Vector4(StolenEnergy, StolenEnergy, 1,1);
    }

}
