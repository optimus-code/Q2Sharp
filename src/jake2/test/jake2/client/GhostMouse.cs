using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public class GhostMouse
    {
        public static Size size;
        public static void Main(String[] args)
        {
            Robot robot = new Robot();
            size = Toolkit.GetDefaultToolkit().GetScreenSize();
            JFrame frame = new JFrame("Ghost Mouse (tm)!");
            JButton button = new JButton("Gho Ghost");
            frame.GetContentPane().Add(button);
            button.AddActionListener(new CircleListener(robot));
            frame.Pack();
            frame.SetLocation((int)(size.Width - frame.GetWidth()) / 2, (int)(size.Height - frame.GetHeight()) / 2);
            frame.SetVisible(true);
        }
    }
}