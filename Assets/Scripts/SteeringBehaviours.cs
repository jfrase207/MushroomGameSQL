using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains all the logic for the steering behaviour used for the enemy and the world wrap feature used by player and enemy
/// </summary>
public class SteeringBehaviours : MonoBehaviour {

    Vector3 CurrentVelocity;
    Vector3 desiredVelocity;
    Vector3 steeringVelocity;

    public static float MaxSpeed = 50f;
    public static float MaxSteering = 1f;

    float wanderRadius = 1;
    float wanderDistance = 2;
    float randPoint = 2;
    float aheadDistance = 20f;   
    float sperationDistance = 30;
    float slowingradius = 15f;
    float vAheadDist = 4f;
    float vAheadDist1 = 1f;

    [Header("Layer Mask For Raycast to ground")]
    public LayerMask WalkableGround;
    public LayerMask Obstacles;   

    Vector3 wanderTarget;
    Vector3 targetPosition;  

    private void Start()
    {
        wanderTarget = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(wanderTarget.x, 0, wanderTarget.y);        
    }

    public void MoveUnit(Vector3 steering, float speed)
    {
       
        CurrentVelocity += Truncate(steering, MaxSteering);
        CurrentVelocity = Truncate(CurrentVelocity, speed);
        transform.forward = Vector3.Normalize(new Vector3(CurrentVelocity.x, 0.0f, CurrentVelocity.z));
        transform.position += CurrentVelocity * Time.deltaTime;
    }

    public Vector3 Seek(Vector3 Pos)
    {

        desiredVelocity = Vector3.Normalize(Pos - transform.position) * MaxSpeed;
        steeringVelocity = desiredVelocity - CurrentVelocity;
        return steeringVelocity;
    }

    public Vector3 Flee(Vector3 Pos)
    {
        desiredVelocity = Vector3.Normalize(transform.position - Pos) * MaxSpeed;
        steeringVelocity = desiredVelocity - CurrentVelocity;
        return steeringVelocity;
    }

    public Vector3 Wander()
    {       
        float rand = randPoint * Time.deltaTime;
        wanderTarget += new Vector3(Random.Range(-10f, 10f) * rand, 0f, Random.Range(-10f, 10f) * rand);        
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        targetPosition = transform.position + (transform.forward * wanderDistance) + wanderTarget;

        return Seek(targetPosition);
    }

  
    public Vector3 vAhead(float length)
    {
        Vector3 ahead;
        float dLength = CurrentVelocity.magnitude / MaxSpeed;
        ahead = transform.position + (transform.forward * aheadDistance * dLength) * length;
        return ahead;
    }

    Vector3 avoidance;

    public Vector3 collisionAvoidance()
    {
        float foo = 0;

        if (Physics.CheckSphere(vAhead(vAheadDist),0.5f,GameLogic.currrent.layer))
        {
            Vector3 fObs = findObstacles(vAhead(vAheadDist)).transform.position;
           
            avoidance.x = vAhead(vAheadDist).x - fObs.x;
            avoidance.z = vAhead(vAheadDist).z - fObs.z;

            avoidance = avoidance.normalized;
            avoidance *= aheadDistance;

        }
        if (Physics.CheckSphere(vAhead(vAheadDist1), 0.5f, GameLogic.currrent.layer))
        {
            Vector3 fObs = findObstacles(vAhead(vAheadDist1)).transform.position;
            
            avoidance.x = vAhead(vAheadDist1).x - fObs.x;
            avoidance.z = vAhead(vAheadDist1).z - fObs.z;

            avoidance = avoidance.normalized;
            avoidance *= aheadDistance;
        }

        return avoidance*1.5f;

    }

    public Collider findObstacles(Vector3 colCheck)
    {

        Collider[] hitColliders = Physics.OverlapSphere(colCheck, 1, GameLogic.currrent.layer);
        

        foreach (var hit in hitColliders)
        {

            return hit;

        }
        return null;


    }

    static public Vector3 Truncate(Vector3 velocity, float speed)
    {
        if (velocity.sqrMagnitude > speed * speed)
        {
            velocity.Normalize();
            velocity *= speed;
        }
        return velocity;
    }

    /// <summary>
    /// This checks the objects position and if the player moves out of bounds the player position is negated flipping them to the other side of the game field
    /// </summary>
    public void WorldWrap()
    {
        if (transform.position.x > GameLogic.currrent.groundX || transform.position.x < -GameLogic.currrent.groundX)
        {
            transform.position = new Vector3(updatePosX(transform.position.x, GameLogic.currrent.groundX), transform.position.y, transform.position.z);
        }
        if (transform.position.z > GameLogic.currrent.groundZ || transform.position.z < -GameLogic.currrent.groundZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, updatePosX(transform.position.z, GameLogic.currrent.groundZ));
        }
    }

    /// <summary>
    /// This clamps the objects position to within the game field
    /// </summary>
    /// <param name="_pos">This takes in the position of the object</param>
    /// <param name="ground">This is the game field bounds</param>
    /// <returns></returns>
    public float updatePosX(float _pos, float ground)
    {
        _pos = -Mathf.Clamp(_pos, -ground, ground);
        return _pos;
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.Normalize(desiredVelocity) * (wanderDistance + wanderRadius));
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.Normalize(CurrentVelocity) * wanderDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * wanderDistance) + CurrentVelocity, wanderRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + (transform.forward * wanderDistance) + CurrentVelocity, wanderTarget);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(vAhead(vAheadDist), 0.5f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(vAhead(vAheadDist1), 0.5f);

    }

}
