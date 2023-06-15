using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public GameObject gameOverText;
    public float carMaxHealth = 100f;
    public float carCurrentHealth = 100f;

    private Slider mySlider;
    private float timer;
    private bool gazedAt;
    private Coroutine fillBarRoutine;

    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponent<Slider>();
        if (mySlider == null)
        {
            Debug.Log("Please add a Slider Component to this GameObject!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FillBar());
    }

    private IEnumerator FillBar()
    {
        while(carCurrentHealth > 0)
        {
            mySlider.value = carCurrentHealth / carMaxHealth;

            yield return null;
        }
        OnBarFilled();
    }

    private void OnBarFilled()
    {
        gameOverText.SetActive(true);
    }
}
