using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadScore : SingletonClass<SaveAndLoadScore>
{
    [SerializeField] int id = 1;
    [SerializeField] string filename;
    public List<SaveHighScoreData> saveScore = new List<SaveHighScoreData>();

    private void Start() 
    {
        saveScore = FileHandler.ReadListFromJSON<SaveHighScoreData>(filename);
        idCheck();
    }

    public void addToList(int score)
    {
        saveScore.Add(new SaveHighScoreData(id,score));
        FileHandler.SaveToJSON<SaveHighScoreData>(saveScore,filename);
    }

    private void idCheck()
    {
        foreach(var score in saveScore)
        {
            if(score.id == id)
            {
                id++;
            }
        }
    }
}
