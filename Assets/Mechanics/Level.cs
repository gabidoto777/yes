using UnityEngine;

public class Level : MonoBehaviour
{


    public Transform spawnPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => GameManager.Instance.InstantiatePlayer(spawnPos.position);

}
