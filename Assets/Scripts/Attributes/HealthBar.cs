using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        
        void Update() {
            if (EnemyIsUndamaged() || EnemyIsDead())
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(-healthComponent.GetFraction(), 1, 1);
        }

        bool EnemyIsUndamaged()
        {
            if (Mathf.Approximately(-healthComponent.GetFraction(), -1))
                return true;
            return false;
        }

        bool EnemyIsDead()
        {
            if (Mathf.Approximately(-healthComponent.GetFraction(), 0))
                return true;
            return false;
        }
    }
}