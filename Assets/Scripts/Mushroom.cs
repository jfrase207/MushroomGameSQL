using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class delas with all the mushrooms behaviours
/// </summary>
public class Mushroom : MonoBehaviour {    

    //the offset is the speed the mushrooms rotates at
    float offset;
    //the default max death age
    float deathAge = 60;
    //this mushrooms age
    public float thisAge;
    //this mushrooms calculated life span
    float currentDeathAge;
    //if this mushroom was loaded from database or not
    public bool loadedMushroom = false;
    // Use this for initialization
    void Start () {
        //opffset is a random number between 10 and 60
        offset = Random.Range(10f, 60f);
        if (!loadedMushroom)
        {
            //if the musroom was not loaded create a death age for it
            currentDeathAge = Random.Range(10f, deathAge);
            thisAge = currentDeathAge;
        }
	}

    private void OnApplicationQuit()
    {
        //saves the mushroom to the database on quit
        SQLFunctions.current.SaveMushrooms(gameObject.name, transform.position, thisAge);
    }

    // Update is called once per frame
    void Update () {
        //reduces the lifespan of mushroom
        thisAge -= Time.deltaTime;
        //roates the mushroom by a speed based on the offset
        transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * offset);
        //if the ages reaches 0 then the mushroom shrinks in size and floats up
        if(thisAge < 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
            transform.localScale = new Vector3(transform.localScale.x- Time.deltaTime * 0.1f, transform.localScale.y - Time.deltaTime*0.1f, transform.localScale.z - Time.deltaTime * 0.1f);
        }
        //once the mushroom reaches half its size it is destroyed
        if(transform.localScale.y < 0.5f)
        {
            Destroy(gameObject);
        }
        //once the mushroom reaches its half life its is changed to magenta colour and the tag is changed to poisonous
        if(thisAge < currentDeathAge/2)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
            gameObject.tag = "Poisonous";
        }
    }
}
