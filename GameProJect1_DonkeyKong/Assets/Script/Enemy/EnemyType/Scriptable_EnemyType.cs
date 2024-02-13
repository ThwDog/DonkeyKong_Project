using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy")]
public class Scriptable_EnemyType : ScriptableObject
{
    public enum Type
    {
        typeOne , typeTwo
    }

    public Type type;

    public GameObject obj;
}
