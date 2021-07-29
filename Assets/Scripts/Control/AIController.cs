using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine.AI;



namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        public float ChaseDistance = 5f;
        public float SuspicionTime = 3f;
        public PatrolPath patrolPath;
        public float WaypointTolerance = 1f;
        public float WaypointDwellTime;
        public float ChaseVelocity;

        NavMeshAgent _navMeshAgent;
        Fighter fighter;
        GameObject player;
        Health health;
        Move move;

        float _timeSinceLastSawPlayer = Mathf.Infinity;
        float _timeSinceArrivedAtWaypoint = Mathf.Infinity;

        Vector3 _guardPosition;
        int _currentWaypointIndex = 0;
        private float _originalSpeed;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _originalSpeed = _navMeshAgent.speed;

            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            health = GetComponent<Health>();
            move = GetComponent<Move>();
           // patrolPath = GetComponent<PatrolPath>();

            _guardPosition = transform.position;
        }

        private void Update()
        {
            


            if (health.IsDead()) return;
            if (InAttackRange() && fighter.CanAttack(player))
            {
                _timeSinceLastSawPlayer = 0;
                ChaseSpeed();
                fighter.Attack(player);
            }
            else if (_timeSinceLastSawPlayer < SuspicionTime)
            {
                GetComponent<Scheduler>().CancelAction();
            }
            else
            {
                PatrolBehaviour();
            }
            

            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            _navMeshAgent.speed = _originalSpeed;

            Vector3 nextPosition = _guardPosition;
           
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    WaypointDwellTime = Random.Range(0f, 4f);
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > WaypointDwellTime)
            {
                move.StartMoveAction(nextPosition);
            }
            
        }

        private bool InAttackRange()
        {
            float distanceToPayer =  Vector3.Distance(transform.position, player.transform.position);
            return distanceToPayer < ChaseDistance;
       
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }
        public bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < WaypointTolerance;
        }
        public Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWaypointIndex);
        }
        public void CycleWaypoint()
        {
            _currentWaypointIndex = patrolPath.GetNextWaypoint(_currentWaypointIndex);
        }
        public void ChaseSpeed()
        {
            if (InAttackRange())
            {
                
                _navMeshAgent.speed = ChaseVelocity;
            }
            
            
        }
    }
    
}

