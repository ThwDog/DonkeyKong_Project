using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BerralType", menuName = "ScriptableObject/BerralType")]
public class BarrelTypeScriptable : ScriptableObject 
{
    public enum type
    {
        OpeningType , NormalType , CrossType , CanUseLadderType 
    }

    public type _type;

    public float speed;

    
}

