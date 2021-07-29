
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Move : MonoBehaviour, IAction, ISaveable
    {
        public Transform Target;
        NavMeshAgent _navMeshAgent;
        Health health;
        private void Start()
        {
            health = GetComponent<Health>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            _navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();

        }
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<Scheduler>().StartAction(this);
            MoveTo(destination);
            
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);

        }

        [System.Serializable]
        struct MoveSavaData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        public object CaptureState()
        {
            MoveSavaData data = new MoveSavaData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            MoveSavaData data  = (MoveSavaData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }

}