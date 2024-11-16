using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class ChaseCommand : ICommand
    {
        public void Execute(Robot robot)
        {
            // Lógica para perseguir al jugador
            robot.agent.SetDestination(robot.player.position);
        }
    }
}
