using UnityEngine;
using RPG.Attributes;


namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;        
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float flatBonusDamageModifier = 5f;
        [SerializeField] float totalBonusDamageModifier = 0f;
        [SerializeField] float weaponRange = 2f;

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


        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile,
                                            GetHandTransform(rightHand, leftHand).position, 
                                            Quaternion.identity);
            
            // Debug.Log(target);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }


        public float GetDamage() => flatBonusDamageModifier;
        public float GetPercentageBonus() => totalBonusDamageModifier;
        public float GetRange() => weaponRange;
    }
}