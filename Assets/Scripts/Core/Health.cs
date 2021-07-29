using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float HP = 100;
        bool _isDead = false;
        public bool IsDead()
        {
            return _isDead;
        }
        
        
        public void TakeDmg(float dmg)
        {
            HP = Mathf.Max(HP - dmg, 0);
            if (HP <= 0)
            {
                Death();
            }
        }
        public void Death()
        {
            if (_isDead) return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<Scheduler>().CancelAction();
        }

        public object CaptureState()
        {
            return HP;
        }

        public void RestoreState(object state)
        {
            HP = (float)state;
            if (HP <= 0)
            {
                Death();
            }
        }
    }
}
