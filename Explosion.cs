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
    class Explosion : Entity
    {
        private bool expand = false;

        public Explosion(Canvas _canvas)
        {
            this.posX = maths.GetRandInt(0, (int) _canvas.Width);
            this.posY = maths.GetRandInt(0, (int) _canvas.Height);
            this.width = 10;
            this.height = 10;
            this.canvas = _canvas;
            renderExplosion();
        }

        private void renderExplosion()
        {
            this.sprite = new Ellipse();

            this.sprite.Height = this.height;
            this.sprite.Width = this.width;

            this.sprite.Fill = new SolidColorBrush(Colors.Yellow);
            this.sprite.Fill.Opacity = 1;

            this.sprite.SetValue(Canvas.LeftProperty, (double)posX);
            this.sprite.SetValue(Canvas.TopProperty, (double)posY);
            canvas.Children.Add(this.sprite);
        }

        public void explode()
        {
            canvas.Children.Remove(this.sprite);
            if (this.sprite.Height > 40)
            {
                this.expand = false;
                this.setDead();
            }
            if (this.expand)
            {
                this.sprite.Height += 5;
                this.sprite.Width += 5;
            }
            renderExplosion();
        }
    }
}
