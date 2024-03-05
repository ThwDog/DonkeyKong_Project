using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy")]
public class Scriptable_EnemyType : ScriptableObject
{
    public enum Type
    {
        typeOne ,  // lvl 1
        typeTwo ,  //Lvl 2? lvl 3? 
        typeThree //lvl 4
    }

    public Type type;

}
