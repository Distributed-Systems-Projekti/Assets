using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player settings")]
    public GameObject playerPrefab;
    public GameObject camera;

    [Header("Food settings")]
    public GameObject foodPrefab;

    public Vector2 xRange;
    public Vector2 yRange;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnFood();
        }
    }

    void Awake()
    {
        instance = this;
    }

    private void SpawnFood()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(xRange.x, xRange.y),
            Random.Range(yRange.x, yRange.y),
            1
        );
        GameObject _food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(xRange.x, xRange.y),
            Random.Range(yRange.x, yRange.y),
            1
        );
        GameObject _player = PhotonNetwork.Instantiate(
            playerPrefab.name,
            spawnPosition,
            Quaternion.identity
        );
        camera.SetActive(true);
        camera.GetComponent<SmoothCameraFollow>().target = _player.transform;

        _player.GetComponent<ScoreManager>().enabled = true;
        _player.GetComponent<Movement>().enabled = true;
    }
}
