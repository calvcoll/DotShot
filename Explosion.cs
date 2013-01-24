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
            this.PosX = maths.GetRandInt(0, (int) _canvas.Width);
            this.PosY = maths.GetRandInt(0, (int) _canvas.Height);
            this.Width = 10;
            this.Height = 10;
            this.canvas = _canvas;
            renderExplosion();
        }

        private void renderExplosion()
        {
            this.Sprite = new Ellipse();

            this.Sprite.Height = this.Height;
            this.Sprite.Width = this.Width;

            this.Sprite.Fill = new SolidColorBrush(Colors.Yellow);
            this.Sprite.Fill.Opacity = 1;

            this.Sprite.SetValue(Canvas.LeftProperty, (double)PosX);
            this.Sprite.SetValue(Canvas.TopProperty, (double)PosY);
            canvas.Children.Add(this.Sprite);
        }

        public void explode()
        {
            canvas.Children.Remove(this.Sprite);
            if (this.Sprite.Height > 40)
            {
                this.expand = false;
                this.setDead();
            }
            if (this.expand)
            {
                this.Sprite.Height += 5;
                this.Sprite.Width += 5;
            }
            renderExplosion();
        }
    }
}
