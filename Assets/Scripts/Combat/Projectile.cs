using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target = null;
        float _dmg = 0f;
        public float Speed = 2f;

        public GameObject FireballImpactEffect;
        public bool IsHoming = true;
        private Vector3 _hitLocation;

        void Start()
        {
            _hitLocation = target.transform.position;
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (target == null) return;
            if (IsHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * Speed * Time.deltaTime);

            if (target.IsDead())
            {
                Destroy(gameObject, 2f);
            }
        }

        public void SetTarget(Health target, float dmg)
        {
            this.target = target;
            this._dmg = dmg;
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();

            if (targetCollider == null)
            {

                return target.transform.position;
            }
            _hitLocation = target.transform.position + Vector3.up * targetCollider.height / 2;
            return _hitLocation;


        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDmg(_dmg);

            if (FireballImpactEffect != null)
            {
                Instantiate(FireballImpactEffect, GetAimLocation(), transform.rotation);
            }
            Destroy(gameObject);



        }
    }
}
