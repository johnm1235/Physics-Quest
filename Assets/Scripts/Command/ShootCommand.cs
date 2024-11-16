using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class ShootCommand : ICommand
    {
        public void Execute(Robot robot)
        {
            if (robot.CanShoot())
            {
                robot.ShootProjectile();
            }
        }
    }
}
