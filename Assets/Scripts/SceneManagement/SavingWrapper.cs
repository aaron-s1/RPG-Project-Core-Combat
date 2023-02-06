using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 0.2f;

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            // yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);   // Broken.
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                Load();

            if (Input.GetKeyDown(KeyCode.S))
                Save();
        }

        public void Save() =>
            GetComponent<SavingSystem>().Save(defaultSaveFile);

        // call to loading systems
        public void Load() =>
            GetComponent<SavingSystem>().Load(defaultSaveFile);
    }
}

