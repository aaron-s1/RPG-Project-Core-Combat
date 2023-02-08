using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        [SerializeField] AnimatorOverrideController animatorOverride = null;


        public void Spawn(Transform handTransform, Animator animator)
        {
            if (equippedPrefab != null)
                Instantiate(equippedPrefab, handTransform);

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
        }

        public float GetDamage() =>
            weaponDamage;

        public float GetRange() =>
            weaponRange;
    }
}