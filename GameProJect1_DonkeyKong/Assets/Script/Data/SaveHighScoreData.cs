using System;

[Serializable]
public class SaveHighScoreData
{
    public int id;
    public int score;

    public SaveHighScoreData(int id,int score)
    {
        this.id = id;
        this.score = score;
    }
}
