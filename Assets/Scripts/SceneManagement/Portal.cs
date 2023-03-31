using System;
using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B
        }

        [SerializeField] int sceneToLoadIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                StartCoroutine(Transition());
        }

        IEnumerator Transition()
        {
            if (sceneToLoadIndex < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController =  GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex);
            PlayerController newPlayerController =  GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerPosition(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            playerController.enabled = true;
            Destroy(gameObject);
        }


        Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal != this)
                    if (portal.destination == destination)
                        return portal;
            }

            return null;
        }


        
        void UpdatePlayerPosition(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            var navMeshAgent = player.GetComponent<NavMeshAgent>();

            navMeshAgent.enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            navMeshAgent.enabled = true;

            // navMeshAgent.Warp(otherPortal.spawnPoint.position);
        }
    }
}