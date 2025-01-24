using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject changeLvl;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text progresText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text lowComlp;
    [SerializeField] private TMP_Text midComlp;
    [SerializeField] private TMP_Text highComlp;

    private void Start()
    {
        UpdateText();
    }
    private void Update()
    {
        UpdateLvlCompliteText();
    }
    public void StartLevel(string levelName) 
    {
        Manager.Instance.IsGameOver = false;
        if (levelName == "low")
        {
            SceneManager.LoadScene("low");
            Manager.Instance.Progress = 2;
            
        } else if (levelName == "mid")
        {
            SceneManager.LoadScene("mid");
            Manager.Instance.Progress = 3;
        } else if (levelName == "high") 
        {
            SceneManager.LoadScene("high");
            Manager.Instance.Progress = 4;
        }
        UpdateText();
    }

    public void ButChangeLvl() 
    {
        GameObject[] allPanels = GameObject.FindObjectsOfType<GameObject>(true);
        GameObject GameOverPanel = System.Array.Find(allPanels, obj => obj.name == "GameOverPanel");
        GameOverPanel.SetActive(false);
        GameObject ChangeLvl = System.Array.Find(allPanels, obj => obj.name == "ChangeLvl");
        ChangeLvl.SetActive(true);
    }

    public void UpdateText() 
    {
        progresText.text = $"Осталось ходов: {Manager.Instance.Progress}"; 
    }
    public void UpdateText(int i) 
    {
        if (i == 1)
            gameOverText.text = "Правильно!";
        else
            gameOverText.text = "Неправильно!";
    }
    public void UpdateLvlCompliteText() 
    {
        if (Manager.Instance.isLowLevelCompleted)
        {
            lowComlp.enabled = true;
        }else
            lowComlp.enabled = false;
        if (Manager.Instance.isMidLevelCompleted)
        {
            midComlp.enabled = true;
        }
        else
            midComlp.enabled = false;
        if (Manager.Instance.isHighLevelCompleted)
        {
            highComlp.enabled = true;
        }
        else
            highComlp.enabled = false;
    }

}
