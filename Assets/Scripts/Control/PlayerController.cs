using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {

        void Update() {
            if (InteractWithCombat())
                return;

            if (InteractWithMovement())
                return;            
        }


        bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();


                if (target != null) {
                    if (Input.GetMouseButtonDown(0)) 
                        GetComponent<Fighter>().AttackTarget(target);
                        
                    return true;
                }
            }

            return false;
        }


        bool InteractWithMovement() {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                    // GetComponent<Mover>().MoveTo(hit.point);
                }

                return true;
            }

            return false;
        }



        static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}