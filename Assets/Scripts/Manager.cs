using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    private bool getRing = false;
    private GameObject targetRing;
    private int _progress = 2;
    public int Progress 
    {
        get { return _progress; }
        set { _progress = value; }
    }
    private bool _isGameOver;
    public bool IsGameOver 
    {
        get { return _isGameOver; }
        set { _isGameOver = value; }
    }

    public bool isLowLevelCompleted = false;
    public bool isMidLevelCompleted = false;
    public bool isHighLevelCompleted = false;
    [SerializeField] private UIManager _uiManager;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GameObject[] allPanels = GameObject.FindObjectsOfType<GameObject>(true);
        GameObject ChangeLvl = System.Array.Find(allPanels, obj => obj.name == "ChangeLvl");
        ChangeLvl.SetActive(true);
        _isGameOver = true;
        LoadLevelCompletionStates();
    }
    private void Update()
    {
        print("low " + isLowLevelCompleted);
        print("mid " + isMidLevelCompleted);
        print("high " + isHighLevelCompleted);
        if (_uiManager == null) 
        {
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }
        
        if (getRing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("tower"))
                    {
                        _progress--;
                        _uiManager.UpdateText();
                        GameObject tower = hit.collider.gameObject;
                        StartCoroutine(MoveRingOnTower(targetRing, tower));
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !_isGameOver) 
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) 
                {
                    if (hit.collider.CompareTag("ring")) 
                    {
                        GameObject ring = hit.collider.gameObject.transform.parent.gameObject;
                        targetRing = ring;
                        StartCoroutine(MoveRing(targetRing));
                        getRing = true;
                    }
                }
            }
        }
    }

    private IEnumerator MoveRing(GameObject ring) 
    {
        while (ring.transform.localPosition.y < 2.4f) 
        {
            ring.transform.Translate(new Vector3(3,0,0) * Time.deltaTime * 5f);
            yield return null;
        }
        yield break;
    }
    private IEnumerator MoveRingOnTower(GameObject ring, GameObject tower)
    {
        targetRing = null;
        getRing = false;
        ring.transform.SetParent(GameObject.Find("PlatformYou").transform);

        bool isTowerRight = tower.transform.localPosition.x < ring.transform.localPosition.x;
        ring.transform.SetParent(tower.transform);
        if (isTowerRight)
        {
            while (ring.transform.localPosition.x > -2.3)
            {
                ring.transform.Translate(new Vector3(0, 3, 0) * Time.deltaTime * 5f);
                yield return null;
            }
            
        }
        else
        {
            while (ring.transform.localPosition.x < -2.3)
            {
                ring.transform.Translate(new Vector3(0, -3, 0) * Time.deltaTime * 5f);
                yield return null;
            }
        }
        tower.GetComponent<CylinderScript>().UpdateRingCount();
        int ringsCount = tower.GetComponent<CylinderScript>().RingCount;
        float posRing = 0.4f;
        switch (ringsCount) 
        {
            case 1:
                posRing = 0.9f;
                break;
            case 2:
                posRing = 0.65f;
                break;
            case 3:
                posRing = 0.4f;
                break;
        }


        while (ring.transform.localPosition.y > -posRing)
        {
            ring.transform.Translate(new Vector3(-3, 0, 0) * Time.deltaTime * 5f); // Перемещение по оси Y
            yield return null; 
        }
        GameObject[] towers = GameObject.FindGameObjectsWithTag("tower");
        foreach (GameObject tower0 in towers) 
        {
            tower0.GetComponent<CylinderScript>().CheckRingUp();
        }
        if (_progress == 0)
            GameOverCheck();
        yield break;
    }


    private void GameOverCheck()
    {
        _isGameOver = true;
        GameObject[] towers = GameObject.FindGameObjectsWithTag("tower");
        bool allTrue = true;
        foreach (GameObject tower in towers)
        {
            CylinderScript cylinder = tower.GetComponent<CylinderScript>();
            if (cylinder != null)
            {
                if (!cylinder.isTrue)
                {
                    allTrue = false;
                    break;
                }
            }
        }

        if (allTrue)
        {
            
            _uiManager.UpdateText(1);
            CheckLevelCompletion(SceneManager.GetActiveScene().name);
            _uiManager.UpdateLvlCompliteText();
            GameObject[] allPanels = GameObject.FindObjectsOfType<GameObject>(true);
            GameObject GameOverPanel = System.Array.Find(allPanels, obj => obj.name == "GameOverPanel");
            GameOverPanel.SetActive(true);
        }
        else
        {
           
            _uiManager.UpdateText(2);
            GameObject[] allPanels = GameObject.FindObjectsOfType<GameObject>(true);
            _uiManager.UpdateLvlCompliteText();
            GameObject GameOverPanel = System.Array.Find(allPanels, obj => obj.name == "GameOverPanel");
            GameOverPanel.SetActive(true);
        }
    }

    private void CheckLevelCompletion(string sceneName) 
    {
        if (sceneName == "low")
        {
            isLowLevelCompleted = true;
            SaveLevelCompletionStates();
        } else if (sceneName == "mid")
        {
            isMidLevelCompleted = true;
            SaveLevelCompletionStates();
        } else if (sceneName == "high") 
        {
            isHighLevelCompleted = true;
            SaveLevelCompletionStates();
        }
    }
    private void SaveLevelCompletionStates()
    {
        PlayerPrefs.SetInt("LowLevelCompleted", isLowLevelCompleted ? 1 : 0);
        PlayerPrefs.SetInt("MidLevelCompleted", isMidLevelCompleted ? 1 : 0);
        PlayerPrefs.SetInt("HighLevelCompleted", isHighLevelCompleted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadLevelCompletionStates()
    {
        isLowLevelCompleted = PlayerPrefs.GetInt("LowLevelCompleted", 0) == 1;
        isMidLevelCompleted = PlayerPrefs.GetInt("MidLevelCompleted", 0) == 1;
        isHighLevelCompleted = PlayerPrefs.GetInt("HighLevelCompleted", 0) == 1;
    }
}
