using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadyTriggered;

        void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                if (!alreadyTriggered) {
                    alreadyTriggered = true;
                    GetComponent<PlayableDirector>().Play();
                }
            }
        }
    }
}
