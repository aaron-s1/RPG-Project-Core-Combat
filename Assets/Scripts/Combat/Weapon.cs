using UnityEngine;
using RPG.Attributes;


namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;        
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }

            HandleAnimatorOverrides(animator);
        }

        void HandleAnimatorOverrides(Animator animator)
        {
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;

            else if (overrideController)
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);

            if (oldWeapon == null)
                oldWeapon = leftHand.Find(weaponName);
                
            if (oldWeapon == null)
                return;
            
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
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

        // public bool HasProjectile() {
        //     if (projectile != null)
        //         return true;
        //     else return false;
        // }
            

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile,
                                            GetHandTransform(rightHand, leftHand).position, 
                                            Quaternion.identity);
            
            // Debug.Log(target);
            projectileInstance.SetTarget(target, weaponDamage);
        }


        public float GetDamage() => weaponDamage;
        public float GetRange() => weaponRange;
    }
}