using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetGameHistory : MonoBehaviour
{
    private DBManager _dbManager;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform content;
    
    void Start()
    {
        _dbManager = FindObjectOfType<DBManager>();
        ShowHistory();
    }

    private void ShowHistory()
    {
        List<string> nicks = new List<string>();
        List<int> scores = new List<int>();
        (nicks, scores) = _dbManager.GetAllStat();
        for (int i = 0; i < nicks.Count; i++)
        {
            GameObject newNick = Instantiate(cellPrefab, content);
            GameObject newScore = Instantiate(cellPrefab, content);
            newNick.transform.GetChild(0).GetComponent<TMP_Text>().text = nicks[i];
            newScore.transform.GetChild(0).GetComponent<TMP_Text>().text = scores[i].ToString();
        }
    }
}
