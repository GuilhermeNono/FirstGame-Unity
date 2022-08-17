using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;
    [SerializeField] private Text cherriesText;
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject gameObject = col.gameObject;
        if (gameObject.CompareTag("Cherry"))
        {
            Destroy(gameObject);
            cherries++;
            cherriesText.text = $"Cherries: {cherries}";
        }
    }
    
    
}
