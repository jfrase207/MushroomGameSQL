using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class generates the high scores HighScoreTable
/// </summary>
public class HighScores : MonoBehaviour
{
    public int theTopScores;
    public Text hishcoretable;

    // Use this for initialization
    void Start()
    {
        hishcoretable.text = SQLFunctions.current.GetHighScores(theTopScores);
    }

}
