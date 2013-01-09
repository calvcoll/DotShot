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
            this.posX = maths.GetRandInt(0, (int)_canvas.Width - 5);
            this.posY = maths.GetRandInt(0, (int)_canvas.Height - 5);
            this.width = 12;
            this.height = 12;
            this.health = 20;
            this.canvas = _canvas;
            this.renderSprite();
        }

        protected new void renderSprite()
        {
            this.sprite = new Ellipse();

            this.sprite.Height = this.height;
            this.sprite.Width = this.width;

            this.sprite.Fill = new SolidColorBrush(Colors.Brown);
            this.sprite.Fill.Opacity = 1;

            this.sprite.SetValue(Canvas.LeftProperty, (double)posX);
            this.sprite.SetValue(Canvas.TopProperty, (double)posY);
            canvas.Children.Add(this.sprite);
        }

        public void checkCollisions(Entity entity)
        {
        }
    }
}
