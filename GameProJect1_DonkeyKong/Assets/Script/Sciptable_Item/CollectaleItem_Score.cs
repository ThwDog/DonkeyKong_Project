using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CollectableItem")]
public class CollectaleItem_Score : ItemScriptable
{
    public int score;

    public virtual void setScore(GameManager gm)
    {
        var _diff = gm.difficulty;

        switch(_diff)
        {
            case GameManager.diff.one: 
                score = 300;
                break;
            case GameManager.diff.two:
                score = 500;
                break;
            case GameManager.diff.three:
                score = 800;
                break;
            case GameManager.diff.four:
                score = 800;
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }
}
