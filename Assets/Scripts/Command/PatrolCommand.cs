using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class PatrolCommand : ICommand
    {
        public void Execute(Robot robot)
        {
            if (robot.IsAtPatrolPoint())
            {
                robot.SetNextPatrolPoint();
            }
            else
            {
                // Mover el robot hacia el punto de patrullaje actual
                Vector3 direction = (robot.patrolPoints[robot.currentPatrolIndex].position - robot.transform.position).normalized;
                robot.transform.position += direction * robot.patrolSpeed * Time.deltaTime;
            }
        }
    }
}
