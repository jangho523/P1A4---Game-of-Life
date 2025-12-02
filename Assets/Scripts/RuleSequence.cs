using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameOfLife/RuleSequence")]
public class RuleSequence : ScriptableObject
{
    public List<GameRule> rules = new List<GameRule>();
}
