using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    class CircleListener : IActionListener
    {
        Robot robot;
        public CircleListener(Robot robot)
        {
            this.robot = robot;
        }

        public virtual void ActionPerformed(ActionEvent evt)
        {
            int originx = (int)GhostMouse.size.GetWidth() / 2;
            int originy = (int)GhostMouse.size.GetHeight() / 2;
            double pi = 3.1457;
            for (double theta = 0; theta < 4 * pi; theta = theta + 0.1)
            {
                double radius = theta * 20;
                double x = Math.Cos(theta) * radius + originx;
                double y = Math.Sin(theta) * radius + originy;
                robot.MouseMove((int)x, (int)y);
                try
                {
                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}