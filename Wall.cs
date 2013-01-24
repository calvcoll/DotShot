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
    public class Wall : EntityLiving
    {

        public Wall(Canvas _canvas)
        {
            this.PosX = maths.GetRandInt(0, (int)_canvas.Width - 5);
            this.PosY = maths.GetRandInt(0, (int)_canvas.Height - 5);
            this.Width = 12;
            this.Height = 12;
            this.health = 20;
            this.canvas = _canvas;
            this.renderSprite();
        }

        protected new void renderSprite()
        {
            this.Sprite = new Ellipse();

            this.Sprite.Height = this.Height;
            this.Sprite.Width = this.Width;

            this.Sprite.Fill = new SolidColorBrush(Colors.Brown);
            this.Sprite.Fill.Opacity = 1;

            this.Sprite.SetValue(Canvas.LeftProperty, (double)PosX);
            this.Sprite.SetValue(Canvas.TopProperty, (double)PosY);
            canvas.Children.Add(this.Sprite);
        }

        public void checkCollisions(Entity entity)
        {
        }
    }
}
