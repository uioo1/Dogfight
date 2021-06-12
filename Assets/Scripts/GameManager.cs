using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1_Prefab;
    public GameObject player2_Prefab;
    [SerializeField]
    public GameObject[] players;
    public int[] win_Rounds;
    public int currentPlayerNum;
    bool isPlaying;

    void Awake()
    {
        win_Rounds[0] = 0;
        win_Rounds[1] = 0;
        isPlaying = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayers()
    {
        if(players[0] != null)
            players[0].transform.position = new Vector3(-9f, Random.Range(-2.2f, 4.8f), 0);
        else
            players[0] = Instantiate(player1_Prefab, new Vector3(-9f, Random.Range(-2.2f, 4.8f), 0), player1_Prefab.transform.rotation);
        if(players[1] != null)
            players[1].transform.position = new Vector3(9f, Random.Range(-2.2f, 4.8f), 0);
        else
            players[1] = Instantiate(player2_Prefab, new Vector3(9f, Random.Range(-2.2f, 4.8f), 0), player2_Prefab.transform.rotation);
    }

    public void RoundOver(GameObject lose_plane)
    {
        if(lose_plane == players[0])
        {
            win_Rounds[1]++;
        }
        else if(lose_plane == players[1])
        {
            win_Rounds[0]++;
        }
        Gameover_check();
        lose_plane.SetActive(true);
        SpawnPlayers();
    }

    public void Gameover_check()
    {
        if(win_Rounds[0] >= 5)
        {
            GameOver(players[0]);
        }
        else if(win_Rounds[1] >= 5)
        {
            GameOver(players[1]);
        }
    }

    public void GameOver(GameObject win_plane)
    {
        if(win_plane == players[0])
        {
            // player1 WIN!
        }
        else if(win_plane == players[1])
        {
            // player2 WIN!
        }
    }
}
