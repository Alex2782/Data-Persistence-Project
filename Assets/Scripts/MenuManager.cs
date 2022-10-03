using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance = null;
    public TMP_InputField nameInput;
    public TMP_Text bestScoreText;
    public string playerName;
    public string bestPlayerName;
    public int bestScore;


    [System.Serializable]
    class SaveData
    {
        public string bestPlayerName;
        public string playerName;
        public int bestScore;
    }

    public void SaveScoreData()
    {
        string path = GetDataPath();
        SaveData data = new SaveData();
        data.bestPlayerName = bestPlayerName;
        data.bestScore = bestScore;
        data.playerName = playerName;
        string json = JsonUtility.ToJson(data);

        Debug.Log(json);

        File.WriteAllText(path, json);
    }

    public void LoadScoreData()
    {
        string path = GetDataPath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayerName = data.bestPlayerName;
            bestScore = data.bestScore;
            playerName = data.playerName;

            Debug.Log("bestPlayerName: " + bestPlayerName);
            Debug.Log("bestScore: " + bestScore);
            Debug.Log("playerName: " + playerName);
        }
    }

    private string GetDataPath()
    {
        return Application.persistentDataPath + "/data.json";
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadScoreData();
        //nameInput.textComponent.SetText(playerName);
        nameInput.text = playerName;

        if (bestScore > 0)
        {
            bestScoreText.text = $"Best Score : {bestPlayerName} : {bestScore}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickedStart()
    {
        playerName = nameInput.text;
        SceneManager.LoadScene(1);
    }

    public void onClickedQuit()
    {
        playerName = nameInput.text;
        SaveScoreData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
