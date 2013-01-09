using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DotShot
{
    public class ImageHelper
    {
        public void rotate(Image img, int degressRotation)
        {
            RotateTransform rotate = new RotateTransform();
            rotate.Angle = 45;
            rotate.CenterX = img.Width / 2;
            rotate.CenterY = img.Height / 2;
            img.RenderTransform = rotate;
        }
    }
}
