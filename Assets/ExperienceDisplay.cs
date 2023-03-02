using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Resources
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        Text text;

        void Awake() {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            text = GetComponent<Text>();
        }

        void Update() =>
            text.text = String.Format("{0:0}", experience.GetPoints());
    }
}
