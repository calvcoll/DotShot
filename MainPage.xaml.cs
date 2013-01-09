﻿using System;
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
        private List<Entity> entityList = new List<Entity>();

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

        TextBox deathMessageBox = new TextBox();
        TextBox scoreBox = new TextBox();
        TextBox healthBox = new TextBox();

        private int score = 0;

        private SilverlightHost browser;

        public MainPage()
        {
            InitializeComponent();

            browser = Application.Current.Host;
            try
            {
                Application.Current.MainWindow.Title = "DotShot - Wild Framerate Heaven.";
            }
            catch (NotSupportedException nse)
            {
                System.Diagnostics.Debug.WriteLine("Not supported (this is a non-browser option). \n" + nse.StackTrace);
            }
            browser.Settings.EnableFrameRateCounter = true;
            browser.Settings.MaxFrameRate = 60;

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
            entityList.Add(player);

            generateTextBoxes();
            dt.Start();
        }

        private void generateTextBoxes()
        {
            scoreBox.IsEnabled = false;
            scoreBox.Height = 25;
            scoreBox.Width = 100;
            scoreBox.SetValue(Canvas.LeftProperty, (double) 0);
            scoreBox.SetValue(Canvas.TopProperty, (double) 0);
            scoreBox.Text = score.ToString();

            healthBox.IsEnabled = false;
            healthBox.Height = 25;
            healthBox.Width = 100;
            healthBox.SetValue(Canvas.LeftProperty, gameMain.Width - 100);
            healthBox.SetValue(Canvas.TopProperty, (double) 0);
            healthBox.Text = player.getHealth().ToString();

            deathMessageBox.IsEnabled = false;
            deathMessageBox.Height = 25;
            deathMessageBox.Width = 110;
            deathMessageBox.SetValue(Canvas.LeftProperty, gameMain.Width / 2 - (deathMessageBox.Width / 2));
            deathMessageBox.SetValue(Canvas.TopProperty, gameMain.Height / 2 - (deathMessageBox.Height /2));
            deathMessageBox.Text = "You died. How sad.";
        }

        private void GenerateEnemies(int enemies)
        {
            for (int x = 0; x < enemies; x++)
            {
                Enemy enemy = new Enemy(gameMain);
                entityList.Add(enemy);
            }
        }

        private void GenerateWalls(int walls)
        {
            for (int x = 0; x < walls; x++)
            {
                Wall wall = new Wall(gameMain);
                entityList.Add(wall);
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
                    gameMain.Children.Remove(deathMessageBox);
                    score = 0;
                    dt.Stop();
                    startGame();
                }
            }
        }

        private void updateScore()
        {
            gameMain.Children.Remove(scoreBox);
            score++;
            scoreBox.Text = score.ToString();
            gameMain.Children.Add(scoreBox);
        }

        private void updateHealth()
        {
            if (player != null)
            {
                gameMain.Children.Remove(healthBox);
                healthBox.Text = player.getHealth().ToString();
                gameMain.Children.Add(healthBox);
            }
        }

        private void displayDeathMessage()
        {
            //This prevents the game from trying to add the object twice, after the first iteration.
            gameMain.Children.Remove(healthBox);
            healthBox.Text = "0";
            gameMain.Children.Add(healthBox);
            gameMain.Children.Remove(deathMessageBox);
            gameMain.Children.Add(deathMessageBox);
        }

        private void cleanupEntities()
        {
            for (int x = 0; x<entityList.Count;x++)
            {
                Entity entity = entityList[x];
                entity.setDead();
                entityList.Remove(entity);
            }
        }

        private void checkCollisions()
        {
            //Enemy collision code
            foreach (Entity entity in entityList)
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
            foreach (Entity entity in entityList)
            {
                if (entity is Enemy)
                {
                    Enemy enemy = (Enemy)entity;
                    //add a foreach loop identical to above checking for collisions then move away from that enemy
                    int x = (int)enemy.getPosition().X;
                    int y = (int)enemy.getPosition().Y;
                    enemy.hasCollided = false;
                    foreach (Entity entity2 in entityList)
                    {
                        if (entity2 is Enemy)
                        {
                            Enemy enemy2 = (Enemy)entity2;
                            enemy2.hasCollided = false;
                            if (enemy != enemy2)
                            {
                                if (enemy.collidesWith(enemy2))
                                {
                                    enemy.moveSprite(3, 3);
                                    enemy2.moveSprite(-3, -3);

                                    enemy.hasCollided = true;
                                }
                            }
                        }
                    }
                    if (!enemy.hasCollided)
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