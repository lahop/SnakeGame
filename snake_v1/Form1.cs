using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private Timer gameTimer;
        private List<Point> snake;
        private Point food;
        private string direction;
        private int score;
        private Random random;

        public Form1()
        {
            InitializeComponent();

            gameTimer = new Timer();
            gameTimer.Interval = 100; // Snake speed
            gameTimer.Tick += GameLoop;

            snake = new List<Point> { new Point(5, 5) };
            direction = "RIGHT";
            score = 0;
            random = new Random();

            GenerateFood();
            gameTimer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
            gameCanvas.Invalidate();
        }

        private void MoveSnake()
        {
            Point head = snake[0];

            switch (direction)
            {
                case "UP":
                    head.Offset(0, -1);
                    break;
                case "DOWN":
                    head.Offset(0, 1);
                    break;
                case "LEFT":
                    head.Offset(-1, 0);
                    break;
                case "RIGHT":
                    head.Offset(1, 0);
                    break;
            }

            snake.Insert(0, head);
            if (head == food)
            {
                score += 10;
                lblScore.Text = "Score: " + score;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        private void CheckCollision()
        {
            Point head = snake[0];

            if (head.X < 0 || head.Y < 0 || head.X >= gameCanvas.Width / 10 || head.Y >= gameCanvas.Height / 10)
            {
                GameOver();
            }

            for (int i = 1; i < snake.Count; i++)
            {
                if (head == snake[i])
                {
                    GameOver();
                }
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over! Your score is: " + score);
            Application.Exit();
        }

        private void GenerateFood()
        {
            int x = random.Next(0, gameCanvas.Width / 10);
            int y = random.Next(0, gameCanvas.Height / 10);
            food = new Point(x, y);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (direction != "DOWN") direction = "UP";
                    break;
                case Keys.Down:
                    if (direction != "UP") direction = "DOWN";
                    break;
                case Keys.Left:
                    if (direction != "RIGHT") direction = "LEFT";
                    break;
                case Keys.Right:
                    if (direction != "LEFT") direction = "RIGHT";
                    break;
            }
        }

        private void gameCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            // Draw snake
            foreach (Point part in snake)
            {
                graphics.FillRectangle(Brushes.Green, part.X * 10, part.Y * 10, 10, 10);
            }

            // Draw food
            graphics.FillRectangle(Brushes.Red, food.X * 10, food.Y * 10, 10, 10);
        }
    }
}