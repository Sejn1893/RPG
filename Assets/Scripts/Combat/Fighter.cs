using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        Health  target;
        
        public float TimeBetweenAttacks = 1f;

        public Weapon DeafaultWeapon;
        public Transform RightHand = null;
        public Transform LeftHand = null;
        private GameObject _spawnedWeapon = null;

        private Weapon _currentWeapon = null;
        

        private float _timeSinceLastAttack;

        private void Awake()
        {
            _currentWeapon = DeafaultWeapon;
            if (_currentWeapon != null)
            {
                EquipWeapon(_currentWeapon);
            }
        }
        void Start()
        {
            
        }

        private void Update()
        {
             _timeSinceLastAttack += Time.deltaTime;
             
            
            if (target != null)
            {
                bool isInRange = Vector3.Distance(transform.position, target.transform.position) < _currentWeapon.GetRange();
                if (target.IsDead()) return;
                if (!isInRange)
                {
                    GetComponent<Move>().MoveTo(target.transform.position);
                }
                else
                {
                    
                    GetComponent<Move>().Cancel();
                    AttackBehaviour(); 
                }
         
            }
            
        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (_timeSinceLastAttack > TimeBetweenAttacks)
            {
                // This will trigger Hit() event
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
                _timeSinceLastAttack = 0;
                
            }
            
        }

        // Animation Event
        void Hit()
        {
            if (!target) { return; }
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(RightHand, LeftHand, target);
            }
            else
            {
                target.TakeDmg(_currentWeapon.GetDmg());
            }    
            
        }
        void Shoot()
        {
            Hit();
        }

        public void Attack(GameObject combatTarget)
        {
            
            GetComponent<Scheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health tmpTarget = GetComponent<Health>();
            return tmpTarget != null && !tmpTarget.IsDead();
        }  
        public void EquipWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
           
            _currentWeapon = weapon;
            
            if (_spawnedWeapon)
            {
                Destroy(_spawnedWeapon);
            }
            _spawnedWeapon = weapon.Spawn(RightHand, LeftHand, animator);
            


        }

        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
    
    
}
