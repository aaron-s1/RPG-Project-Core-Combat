using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        [SerializeField] bool isRightHanded = true;

        [SerializeField] AnimatorOverrideController animatorOverride = null;


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null) {
                Transform handTransform;

                if (isRightHanded)
                    handTransform = rightHand;
                else handTransform = leftHand;
                
                Instantiate(equippedPrefab, handTransform);
            }

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
        }

        public float GetDamage() =>
            weaponDamage;

        public float GetRange() =>
            weaponRange;
    }
}