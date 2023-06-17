using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndingManager : MonoBehaviour
{
    [SerializeField] private GameObject EndingUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (_enemies.Length == 0)
        {
            EndingUI.SetActive(true);
            Invoke(nameof(ExitGame), 5);
        }
        
    }

    private void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
