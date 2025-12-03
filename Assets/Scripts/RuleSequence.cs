using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * RuleSequence
 * 
 * Holds an ordered list of GameRule objects used for sequential stepping.
 * 
 */
[CreateAssetMenu(menuName = "GameOfLife/RuleSequence")]
public class RuleSequence : ScriptableObject
{
    public List<GameRule> rules = new List<GameRule>();
}
