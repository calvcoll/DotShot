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
    public class Entity
    {
        protected int posX;
        protected int posY;
        protected int width {get; set;}
        protected int height {get; set;}
        protected Canvas canvas;
        protected Boolean isDead = false;
        public bool hasCollided = false;

        protected MathHelper maths = new MathHelper();

        protected Ellipse sprite;

        private bool move;
        public bool Move { get { return this.move; } set { this.move = value; } }

        public bool collidesWith(Entity entity)
        {
            int x1 = this.posX;
            int y1 = this.posY;
            int w1 = this.width;
            int h1 = this.height;
            Rect box1 = new Rect(x1, y1, w1, h1);

            int x2 = entity.posX;
            int y2 = entity.posY;
            int w2 = entity.width;
            int h2 = entity.height;
            Rect box2 = new Rect(x2, y2, w2, h2);

            box1.Intersect(box2);
            if (box1 == Rect.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       public Boolean OutOfBounds(int _posX, int _posY, Boolean isCoords = false)
       {
           if (isCoords)
           {
               if (_posX > (this.canvas.Width - this.width) || _posY > (this.canvas.Height - this.height) || _posX < 0 || _posY < 0)
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           else
           {
               _posX += this.posX;
               _posY += this.posY;
               if (_posX > (this.canvas.Width - this.width) || _posY > (this.canvas.Height - this.height) || _posX < 0 || _posY < 0)
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }

       }

       public void moveSprite(int _posX, int _posY)
       {
           if (!OutOfBounds(_posX, _posY) && this.Move)
           {
               this.posX += _posX;
               this.posY += _posY;
               this.onSpriteMove();
               this.Move = true;
           }
       }

       public void moveSpriteTo(int _posX, int _posY)
       {
           if (!OutOfBounds(_posX, _posY))
           {
               this.posX = _posX;
               this.posY = _posY;
               this.onSpriteMove();
               this.Move = true;
           }
       }

       protected void onSpriteMove()
       {
           this.canvas.Children.Remove(sprite);
           this.sprite.SetValue(Canvas.LeftProperty, (double)posX);
           this.sprite.SetValue(Canvas.TopProperty, (double)posY);
           this.canvas.Children.Add(sprite);
           //this.renderSprite();
       }

       protected void renderSprite()
       {
           this.sprite = new Ellipse();

           this.sprite.Height = this.height;
           this.sprite.Width = this.width;

           this.sprite.Fill = new SolidColorBrush(Colors.Magenta);
           this.sprite.Fill.Opacity = 1;

           this.sprite.SetValue(Canvas.LeftProperty, (double)posX);
           this.sprite.SetValue(Canvas.TopProperty, (double)posY);
           this.canvas.Children.Add(sprite);
       }

       public Point getPosition()
       {
           return new Point(this.posX, this.posY);
       }

       public void setDead()
       {
           this.isDead = true;
           this.onDeath();
       }

       protected void onDeath()
       {
            this.canvas.Children.Remove(sprite);
       }
    }
}
