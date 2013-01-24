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
    public class Player : EntityLiving
    {

        public Player(int _posX, int _posY, Canvas _canvas)
        {
            this.PosX = _posX;
            this.PosY = _posY;
            this.Move = true;
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

            this.Sprite.Fill = new SolidColorBrush(Colors.Magenta);
            this.Sprite.Fill.Opacity = 1;

            this.Sprite.SetValue(Canvas.LeftProperty, (double)PosX);
            this.Sprite.SetValue(Canvas.TopProperty, (double)PosY);
            canvas.Children.Add(this.Sprite);
        }

        public Boolean isPlayerDead()
        {
            return this.IsDead;
        }

        public void checkCollisions(Entity entity)
        {
            if (this.collidesWith(entity) && entity is Enemy)
            {
                this.dealDamage(1);
            }
        }
    }
}
