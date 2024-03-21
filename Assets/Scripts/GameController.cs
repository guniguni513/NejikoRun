using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public NejikoController nejiko;
    public TextMeshProUGUI TMP;
    public LifePanel lifePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //スコアを更新
        int score = CalcScore();
        TMP.text = $"Score : {score} m";

        //ライフパネルを更新
        lifePanel.UpdateLife(nejiko.Life());
    }

    int CalcScore()
    {
        //ネジコの走行距離をスコアとする
        return (int)nejiko.transform.position.z;
    }
}
