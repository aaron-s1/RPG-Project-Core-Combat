using UnityEngine;
using RPG.Attributes;
using UnityEngine.UI;
using System;


namespace RPG.Attributes 
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text text;

        void Awake() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            text.text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}