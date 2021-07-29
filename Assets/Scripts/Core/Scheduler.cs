using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Scheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction Action)
        {
            if (currentAction == Action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            
            currentAction = Action;

        }
        public void CancelAction()
        {
            StartAction(null);
        }
    }
}
