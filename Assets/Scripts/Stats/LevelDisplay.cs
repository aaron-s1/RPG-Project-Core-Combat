using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        Text text;

        void Awake() {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            text = GetComponent<Text>();
        }

        void Update() =>
            text.text = String.Format("{0:0}", baseStats.GetLevel());
    }
}
