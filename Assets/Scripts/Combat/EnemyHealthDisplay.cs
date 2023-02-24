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
                text.text = "N/A";
                return;
            }

            Health health = fighter.GetTarget();
            
            text.text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}