using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Interop;

namespace DotShot
{
    public partial class MainPage : UserControl
    {
        private List<Ellipse> backgroundSpriteList = new List<Ellipse>();
        private List<Entity> EntityList = new List<Entity>();

        private Explosion explosion;

        private bool upPressed = false;
        private bool downPressed = false;
        private bool leftPressed = false;
        private bool rightPressed = false;
        private bool restartPressed = false;
        private bool firePressed = false;

        DispatcherTimer dt = new DispatcherTimer();

        private MathHelper maths = new MathHelper();
        private Player player;

        TextBox DeathMessageBox = new TextBox();
        TextBox ScoreBox = new TextBox();
        TextBox HealthBox = new TextBox();

        private int score = 0;

        private SilverlightHost Host;

        public MainPage()
        {
            InitializeComponent();

            Host = Application.Current.Host;
            if (Application.Current.IsRunningOutOfBrowser)
            {
                Application.Current.MainWindow.Title = "DotShot - Wild Framerate Heaven.";
            }
            Host.Settings.EnableFrameRateCounter = true;
            Host.Settings.MaxFrameRate = 60;

            this.KeyDown += new KeyEventHandler(Page_KeyDown);
            this.KeyUp += new KeyEventHandler(Page_KeyUp);

            dt.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            dt.Tick += new EventHandler(GameLoop);

            GenerateBackground(200);

            startGame();
        }

        private void startGame()
        {
            gameMain.Background = new SolidColorBrush(Colors.Black);
            gameMain.Background.Opacity = 1;
            GenerateEnemies(10);
            GenerateWalls(10);

            player = new Player(200, 100, gameMain);
            EntityList.Add(player);

            generateTextBoxes();
            dt.Start();
        }

        private void generateTextBoxes()
        {
            ScoreBox.IsEnabled = false;
            ScoreBox.Height = 25;
            ScoreBox.Width = 100;
            ScoreBox.SetValue(Canvas.LeftProperty, (double) 0);
            ScoreBox.SetValue(Canvas.TopProperty, (double) 0);
            ScoreBox.Text = score.ToString();

            HealthBox.IsEnabled = false;
            HealthBox.Height = 25;
            HealthBox.Width = 100;
            HealthBox.SetValue(Canvas.LeftProperty, gameMain.Width - 100);
            HealthBox.SetValue(Canvas.TopProperty, (double) 0);
            HealthBox.Text = player.getHealth().ToString();

            DeathMessageBox.IsEnabled = false;
            DeathMessageBox.Height = 25;
            DeathMessageBox.Width = 110;
            DeathMessageBox.SetValue(Canvas.LeftProperty, gameMain.Width / 2 - (DeathMessageBox.Width / 2));
            DeathMessageBox.SetValue(Canvas.TopProperty, gameMain.Height / 2 - (DeathMessageBox.Height /2));
            DeathMessageBox.Text = "You died. How sad.";
        }

        private void GenerateEnemies(int enemies)
        {
            for (int x = 0; x < enemies; x++)
            {
                Enemy enemy = new Enemy(gameMain);
                EntityList.Add(enemy);
            }
        }

        private void GenerateWalls(int walls)
        {
            for (int x = 0; x < walls; x++)
            {
                Wall wall = new Wall(gameMain);
                EntityList.Add(wall);
            }
        }

        void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) upPressed = true;
            if (e.Key == Key.Down) downPressed = true;
            if (e.Key == Key.Left) leftPressed = true;
            if (e.Key == Key.Right) rightPressed = true;
            if (e.Key == Key.R) restartPressed = true;
            if (e.Key == Key.Space) firePressed = true;
        }

        void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) upPressed = false;
            if (e.Key == Key.Down) downPressed = false;
            if (e.Key == Key.Left) leftPressed = false;
            if (e.Key == Key.Right) rightPressed = false;
            if (e.Key == Key.R) restartPressed = false;
            if (e.Key == Key.Space) firePressed = false;
        }

        public void GenerateBackground(int number, int x = -1, int y = -1)
        {
            Boolean posSpecified;
            if (x < 0 && y < 0)
            {
                posSpecified = false;
            }
            else
            {
                posSpecified = true;
            }
            for (int i = 0; i < number; i++)
            {
                Ellipse ellipse = new Ellipse();

                ellipse.Height = maths.GetRandInt(10, 15);
                ellipse.Width = ellipse.Height;

                ellipse.Fill = new SolidColorBrush(Colors.White);
                ellipse.Fill.Opacity = maths.GetRandInt(2, 8) * 0.1;

                if (!posSpecified)
                {
                    x = maths.GetRandInt(0, (int)Math.Round(gameMain.Width, 0));
                    y = maths.GetRandInt(0, (int)Math.Round(gameMain.Height, 0));
                }
                else
                {
                    x += maths.GetRandInt(-20, 20);
                    y += maths.GetRandInt(-20, 20);
                }

                ellipse.SetValue(Canvas.LeftProperty, (double)x);
                ellipse.SetValue(Canvas.TopProperty, (double)y);

                gameMain.Children.Add(ellipse);

                backgroundSpriteList.Add(ellipse);
            }
        }

        private void GameLoop(object sender, EventArgs args)
        {
            updateGameLogic();
        }

        private void updateGameLogic()
        {
            if (player != null)
            {
                keyCheck();
                enemyLogic();
                checkCollisions();
                updateScore();
                updateHealth();
            }
            else
            {
                cleanupEntities();
                displayDeathMessage();
                if (this.restartPressed)
                {
                    gameMain.Children.Remove(DeathMessageBox);
                    score = 0;
                    dt.Stop();
                    startGame();
                }
            }
        }

        private void updateScore()
        {
            gameMain.Children.Remove(ScoreBox);
            score++;
            ScoreBox.Text = score.ToString();
            gameMain.Children.Add(ScoreBox);
        }

        private void updateHealth()
        {
            if (player != null)
            {
                gameMain.Children.Remove(HealthBox);
                HealthBox.Text = player.getHealth().ToString();
                gameMain.Children.Add(HealthBox);
            }
        }

        private void displayDeathMessage()
        {
            //This prevents the game from trying to add the object twice, after the first iteration.
            gameMain.Children.Remove(HealthBox);
            HealthBox.Text = "0";
            gameMain.Children.Add(HealthBox);
            gameMain.Children.Remove(DeathMessageBox);
            gameMain.Children.Add(DeathMessageBox);
        }

        private void cleanupEntities()
        {
            for (int x = 0; x<EntityList.Count;x++)
            {
                Entity entity = EntityList[x];
                entity.setDead();
                EntityList.Remove(entity);
            }
        }

        private void checkCollisions()
        {
            //Enemy collision code
            foreach (Entity entity in EntityList)
            {
                if (player != null)
                {
                    //Enemy Collision
                    if (entity is Enemy)
                    {
                        Enemy enemy = (Enemy)entity;
                        player.checkCollisions(enemy);
                        if (player.getHealth() == 0)
                        {
                            player.setDead();
                            if (player != null)
                            {
                                player = null;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void enemyLogic()
        {
            foreach (Entity entity in EntityList)
            {
                if (entity is Enemy)
                {
                    Enemy enemy = (Enemy)entity;
                    //add a foreach loop identical to above checking for collisions then move away from that enemy
                    int x = (int)enemy.getPosition().X;
                    int y = (int)enemy.getPosition().Y;
                    enemy.HasCollided = false;
                    foreach (Entity entity2 in EntityList)
                    {
                        if (entity2 is Enemy)
                        {
                            Enemy enemy2 = (Enemy)entity2;
                            enemy2.HasCollided = false;
                            if (enemy != enemy2)
                            {
                                if (enemy.collidesWith(enemy2))
                                {
                                    enemy.moveSprite(3, 3);
                                    enemy2.moveSprite(-3, -3);

                                    enemy.HasCollided = true;
                                }
                            }
                        }
                    }
                    if (!enemy.HasCollided)
                    {
                        if (x - player.getPosition().X > 0)
                        {
                            enemy.moveSprite(-3, 0);
                        }
                        if (x - player.getPosition().X < 0)
                        {
                            enemy.moveSprite(3, 0);
                        }
                        if (y - player.getPosition().Y > 0)
                        {
                            enemy.moveSprite(0, -3);
                        }
                        if (y - player.getPosition().Y < 0)
                        {
                            enemy.moveSprite(0, 3);
                        }
                    }
                }
            }
        }

        private void keyCheck()
        {

            if (upPressed)
            {
                player.moveSprite(0, -5);
            }
            if (leftPressed)
            {
                player.moveSprite(-5, 0);
            }
            if (rightPressed)
            {
                player.moveSprite(5, 0);
            }
            if (downPressed)
            {
                player.moveSprite(0, 5);
            }
            if (firePressed && explosion != null)
            {
                explosion = new Explosion(gameMain);
            }
        }

        private void drawSpritesToCanvas()
        {
            Ellipse sprite = new Ellipse();

            sprite.Height = maths.GetRandInt(10, 15);
            sprite.Width = sprite.Height;

            sprite.Fill = new SolidColorBrush(Colors.Red);
            sprite.Fill.Opacity = maths.GetRandInt(2, 8) * 0.1;

            int x = maths.GetRandInt((int)(0 + sprite.Width), (int)(gameMain.Width - sprite.Width));
            int y = maths.GetRandInt(((int)(0 + sprite.Height)), (int)(gameMain.Height - sprite.Height));

            sprite.SetValue(Canvas.LeftProperty, (double)x);
            sprite.SetValue(Canvas.TopProperty, (double)y);

            gameMain.Children.Add(sprite);
        }

        private void CatchMouseEnter(object sender, MouseEventArgs e)
        {
            gameMain.CaptureMouse();
        }
    }
}
