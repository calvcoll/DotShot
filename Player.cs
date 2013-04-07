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

        private new void renderSprite()
        {
            base.renderSprite(Colors.Magenta);
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

            if (this.collidesWith(entity) && entity is Wall)
            {
                this.IsDead = true;
            }
        }
    }
}
