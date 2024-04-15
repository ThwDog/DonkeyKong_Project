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

    public int score;

    public Type type;

    public virtual int setScore(GameManager gm)
    {
        var _diff = gm.difficulty;

        switch(_diff)
        {
            case GameManager.diff.one: 
                return score = 300;
            case GameManager.diff.two:
                return score = 500;
            case GameManager.diff.three:
                return score = 800;
            case GameManager.diff.four:
                return score = 800;
            default:
                Debug.Log("Error");
                break;
        }
        return score;
    }

}
