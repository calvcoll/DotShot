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
        protected int PosX;
        protected int PosY;
        protected int Width {get; set;}
        protected int Height {get; set;}
        protected Canvas canvas;
        protected Boolean IsDead = false;
        public bool HasCollided = false;

        protected MathHelper maths = new MathHelper();

        protected Ellipse Sprite;

        private bool move;
        public bool Move { get { return this.move; } set { this.move = value; } }

        public bool collidesWith(Entity entity)
        {
            int x1 = this.PosX;
            int y1 = this.PosY;
            int w1 = this.Width;
            int h1 = this.Height;
            Rect box1 = new Rect(x1, y1, w1, h1);

            int x2 = entity.PosX;
            int y2 = entity.PosY;
            int w2 = entity.Width;
            int h2 = entity.Height;
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
               if (_posX > (this.canvas.Width - this.Width) || _posY > (this.canvas.Height - this.Height) || _posX < 0 || _posY < 0)
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
               _posX += this.PosX;
               _posY += this.PosY;
               if (_posX > (this.canvas.Width - this.Width) || _posY > (this.canvas.Height - this.Height) || _posX < 0 || _posY < 0)
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
               this.PosX += _posX;
               this.PosY += _posY;
               this.onSpriteMove();
               this.Move = true;
           }
       }

       public void moveSpriteTo(int _posX, int _posY)
       {
           if (!OutOfBounds(_posX, _posY))
           {
               this.PosX = _posX;
               this.PosY = _posY;
               this.onSpriteMove();
               this.Move = true;
           }
       }

       protected void onSpriteMove()
       {
           this.canvas.Children.Remove(Sprite);
           this.Sprite.SetValue(Canvas.LeftProperty, (double)PosX);
           this.Sprite.SetValue(Canvas.TopProperty, (double)PosY);
           this.canvas.Children.Add(Sprite);
           //this.renderSprite();
       }

       protected void renderSprite()
       {
           this.Sprite = new Ellipse();

           this.Sprite.Height = this.Height;
           this.Sprite.Width = this.Width;

           this.Sprite.Fill = new SolidColorBrush(Colors.Magenta);
           this.Sprite.Fill.Opacity = 1;

           this.Sprite.SetValue(Canvas.LeftProperty, (double)PosX);
           this.Sprite.SetValue(Canvas.TopProperty, (double)PosY);
           this.canvas.Children.Add(Sprite);
       }

       public Point getPosition()
       {
           return new Point(this.PosX, this.PosY);
       }

       public void setDead()
       {
           this.IsDead = true;
           this.onDeath();
       }

       protected void onDeath()
       {
            this.canvas.Children.Remove(Sprite);
       }
    }
}
