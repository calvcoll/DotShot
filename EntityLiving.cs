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
    public class EntityLiving : Entity
    {
        protected int health = 0;

        protected void dealDamage(int damage)
        {
            health -= damage;
        }

        public int getHealth()
        {
            return this.health;
        }
    }
}
