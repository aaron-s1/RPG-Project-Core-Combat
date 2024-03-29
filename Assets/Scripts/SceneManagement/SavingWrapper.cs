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


        // void Awake() =>
            // StartCoroutine(LoadLastScene());

        // \/ Disabled so screen doesn't flash every time game is started.
        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);   // Broken.
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
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

