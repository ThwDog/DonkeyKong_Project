using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item")]
public class ItemScriptable : ScriptableObject
{
    public enum Type
    {
        collectScore , weapon
    } 

    public Type type;
    public string _name;
    public GameObject obj;
}
