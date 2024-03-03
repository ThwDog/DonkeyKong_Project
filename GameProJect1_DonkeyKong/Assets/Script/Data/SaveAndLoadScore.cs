using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadScore : SingletonClass<SaveAndLoadScore>
{
    [SerializeField] int id = 1;
    [SerializeField] string filename;
    internal List<SaveHighScoreData> saveScore = new List<SaveHighScoreData>();
   [SerializeField] internal List<int> TopScoreSortList = new List<int>();

    public override void Awake() 
    {
        base.Awake();
        //if folder doesn't exit create path
        if (!System.IO.Directory.Exists(Application.dataPath + "/SaveScoreFolder" ))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/SaveScoreFolder");
            System.IO.File.Create(Application.dataPath + "/SaveScoreFolder/" +filename);
        }
    }

    private void Start() 
    {
        saveScore = FileHandler.ReadListFromJSON<SaveHighScoreData>(filename);
        if(saveScore != null)
            sortScore();
        idCheck();
    }

    public void addToList(int score)
    {
        saveScore.Add(new SaveHighScoreData(id,score));
        FileHandler.SaveToJSON<SaveHighScoreData>(saveScore,filename);
        TopScoreSortList.Add(score);
        TopScoreSortList.Sort();
        TopScoreSortList.Reverse();
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

    public void sortScore()
    {
        Dictionary<int,int> keys = new Dictionary<int, int>();
        foreach(var dic in saveScore)
        {
            if(keys.ContainsKey(dic.id))
            {
                if(keys[dic.id] < dic.score)
                    keys[dic.id] = dic.score;                
                TopScoreSortList.Add(dic.score);
            }
            else
            { 
                keys.Add(dic.id,dic.score);
                TopScoreSortList.Add(dic.score);
            }
        }   

        TopScoreSortList.Sort();
        TopScoreSortList.Reverse();
    }
}
