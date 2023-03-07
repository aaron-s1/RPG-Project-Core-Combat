using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Text text;


        void Awake() {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>(); 
            text = GetComponent<Text>();
        }


        void Update()
        {
            if (fighter.GetTarget() == null)
            {
                text.text = "N/Aaaa";
                return;
            }

            Health health = fighter.GetTarget();
            
            text.text = String.Format("{0:0}%", health.GetPercentage());

            // GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage());
            // text.text = "1";

            // text.text = health.GetPercentage().ToString("{0:0%");
            // text.text = "1";
            // text.text = health.GetHealthPercentageAsText(); //.ToString() + "%";

            // text.text = health.GetHealthPercentage();
        }
    }
}