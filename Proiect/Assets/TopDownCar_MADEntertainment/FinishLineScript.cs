using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FInishLineScript : MonoBehaviour
{
    public int lapsDone;
    public TMP_Text laps;
    public GameObject gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        lapsDone = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(lapsDone > 4)
        {
            gameOverScreen.SetActive(true);
        }
        laps.text = "" + lapsDone.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            lapsDone++;
        }
    }
}
