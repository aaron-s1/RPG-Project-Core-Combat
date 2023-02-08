using RPG.Core;
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
        [SerializeField] Projectile projectile = null;

        [SerializeField] AnimatorOverrideController animatorOverride = null;


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);

                Instantiate(equippedPrefab, handTransform);
            }

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
        }

        Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            if (isRightHanded)
                return rightHand;
            else
                return leftHand;
        }

        public bool HasProjectile() =>
            projectile != null;

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile,
                                            GetHandTransform(rightHand, leftHand).position, 
                                            Quaternion.identity);
            
            projectileInstance.SetTarget(target);
        }

        public float GetDamage() =>
            weaponDamage;

        public float GetRange() =>
            weaponRange;
    }
}