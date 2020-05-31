using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class leaderboard : MonoBehaviour
{
    private Transform scoreContainer;
    private Transform scoreTemplate;
    private List<Transform> scoreListTransform;

    private void Awake()
    {
        scoreContainer = transform.Find("Entry Container");
        scoreTemplate = scoreContainer.Find("Entry Template");

        //Hides template
        scoreTemplate.gameObject.SetActive(false);

        //Grabs list of scores
        highScoreList highScores = getAndSortHighScores();

        //Adds values to UI container
        scoreListTransform = new List<Transform>();
        foreach(scoreEntry scoreEntry in highScores.highScoreEntryList)
        {
            createScoreEntryTransform(scoreEntry, scoreContainer, scoreListTransform);
        }

    }

    //Function for return button
    public void loadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Other Menu");
    }

    //Object to record player and their score
    [System.Serializable] public class scoreEntry
    {
        public long score;
        public string name;
    }

    //List of score objects
    public class highScoreList
    {
        public List<scoreEntry> highScoreEntryList;
    }

    //Used to create a new entry in the high-score table
    public static void createScore(long score, string name)
    {
        scoreEntry newEntry = new scoreEntry { score = score, name = name };

        //Load current list
        string currentScores = PlayerPrefs.GetString("scores");
        highScoreList highScores = JsonUtility.FromJson<highScoreList>(currentScores);

        //Adds new score and saves list
        highScores.highScoreEntryList.Add(newEntry);
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("scores", json);
        PlayerPrefs.Save();
    }

    //Creates a new UI element corresponding to a recorded player and score
    private void createScoreEntryTransform(scoreEntry entry, Transform entryContainer, List<Transform> entryList)
    {
        Transform entryTransform = Instantiate(scoreTemplate, entryContainer);
        RectTransform entryRect = entryTransform.GetComponent<RectTransform>();
        entryRect.anchoredPosition = new Vector2(0, -150f * entryList.Count);
        entryTransform.gameObject.SetActive(true);

        //Possibly add numerical rank to table?

        //int rank = entryList.Count + 1;
        //string rankstring;

        //switch (rank)
        //{
        //    case 1: rankstring = "1st  "; break;
        //    case 2: rankstring = "2nd  "; break;
        //    case 3: rankstring = "3rd  "; break;
        //    default: rankstring = rank + "th  "; break;
        //}

        //entryTransform.Find("Name Entry").GetComponent<TextMeshProUGUI>().text = rankstring + entry.name;

        //Converts score from scoreEntry class to string and passes it to UI
        entryTransform.Find("Score Entry").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
        entryTransform.Find("Name Entry").GetComponent<TextMeshProUGUI>().text = entry.name;
        entryTransform.Find("Background").gameObject.SetActive(entryList.Count % 2 == 0);

        entryList.Add(entryTransform);
    }

    //Returns sorted list of high scores
    public static highScoreList getAndSortHighScores()
    {
        highScoreList highScores;

        //Makes sure scores key is stored
        if (PlayerPrefs.HasKey("scores") == false)
        {
            //If not, creates new list for scores to be added
            List<scoreEntry> entryList = new List<scoreEntry>();
            highScores = new highScoreList { highScoreEntryList = entryList };
            string json = JsonUtility.ToJson(highScores);
            PlayerPrefs.SetString("scores", json);
            PlayerPrefs.Save();
        }
        else
        {
            //Loads current stored scores
            string currentScores = PlayerPrefs.GetString("scores");
            highScores = JsonUtility.FromJson<highScoreList>(currentScores);

            //scoreEntry newEntry = new scoreEntry { name = "KIR", score = 600000000 };
            //highScores.highScoreEntryList.Add(newEntry);

            //Bubble sort to order ranks
            for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
            {
                for (int j = i + 1; j < highScores.highScoreEntryList.Count; j++)
                {
                    if (highScores.highScoreEntryList[j].score > highScores.highScoreEntryList[i].score)
                    {
                        scoreEntry temp = highScores.highScoreEntryList[i];
                        highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                        highScores.highScoreEntryList[j] = temp;
                    }
                }
            }

            //Trims list so only 15 remain
            if (highScores.highScoreEntryList.Count > 15)
            {
                for (int i = highScores.highScoreEntryList.Count; i > 15; i--)
                {
                    highScores.highScoreEntryList.RemoveAt(15);
                    if (highScores.highScoreEntryList.Count < 15)
                    {
                        break;
                    }
                }
            }
        }
        
        return highScores;
    }

    //Resets the leaderboard
    public void clearScoreList()
    {
        string currentScores = PlayerPrefs.GetString("scores");
        highScoreList highScores = JsonUtility.FromJson<highScoreList>(currentScores);

        highScores.highScoreEntryList.Clear();
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("scores", json);
        PlayerPrefs.Save();
    }
}

