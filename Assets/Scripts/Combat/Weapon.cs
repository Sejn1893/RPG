using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create New Weapon")]
    public class Weapon : ScriptableObject
    {
        public float WeaponRange = 2f;
        public float WeaponDamage = 5f;
        public Projectile projectile = null;
        public bool IsRightHanded = true;
        private GameObject _spawnedWeapon = null;


        public AnimatorOverrideController AnimatorOverride = null;
        public GameObject WeaponPrefab = null;

        const string weaponName = "Weapon";
        public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {

            

            if (WeaponPrefab != null)
            {
                

                Transform handTransform = GetHandTransform(rightHand, leftHand);
                _spawnedWeapon =  Instantiate(WeaponPrefab, handTransform);
                
            }
            if (AnimatorOverride != null)
            {
                animator.runtimeAnimatorController = AnimatorOverride;
            }
            return _spawnedWeapon;
            
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (IsRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            if (target.IsDead()) return;
            Projectile tmpProjectile = Instantiate(projectile, GetHandTransform(rightHand,leftHand).position, Quaternion.identity);
            tmpProjectile.SetTarget(target, WeaponDamage);
        }
        public float GetRange()
        {
            return WeaponRange;
        }
        public float GetDmg()
        {
            return WeaponDamage;
        }
        
    }
}