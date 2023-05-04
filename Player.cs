using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DodgeGame.Shared
{
    public class Player : GameCharacter
    {
        private Image playerImage;
        private int speed = 15;
        public bool gamePaused = false;

        public override int X
        {
            get { return (int)Canvas.GetLeft(playerImage); }
            set { Canvas.SetLeft(playerImage, value); }
        }

        public override int Y
        {
            get { return (int)Canvas.GetTop(playerImage); }
            set { Canvas.SetTop(playerImage, value); }

        }

        public Image Image
        {
            get { return playerImage; }
        }


        public Player() : base()
        {
        }

        public Player(Canvas canvas) : base(canvas)
        {
            Draw();

            // Listening for arrow keys (player's movement)
            canvas.KeyDown += canvas_KeyDown;
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gamePaused)
                Move(e);
        }

        protected override void Draw()
        {
            /*
            player = new Rectangle() { Height = 50, Width = 50, Fill = Brushes.Green };
            Canvas.SetLeft(player, 350);
            Canvas.SetTop(player, 150);
            */

            playerImage = new Image
            {
                Height = 50,
                Width = 50,
                Source = new BitmapImage(new Uri(path + "\\player.png", UriKind.RelativeOrAbsolute))
            };
            Canvas.SetLeft(playerImage, 350);
            Canvas.SetTop(playerImage, 150);
            canvas.Children.Add(playerImage);
        }

        public void Move(KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Up) && Keyboard.IsKeyDown(Key.Left))
            {
                Canvas.SetTop(playerImage, Canvas.GetTop(playerImage) - speed);
                Canvas.SetLeft(playerImage, Canvas.GetLeft(playerImage) - speed);
            }
            else if (Keyboard.IsKeyDown(Key.Up) && Keyboard.IsKeyDown(Key.Right))
            {
                Canvas.SetTop(playerImage, Canvas.GetTop(playerImage) - speed);
                Canvas.SetLeft(playerImage, Canvas.GetLeft(playerImage) + speed);
            }
            else if (Keyboard.IsKeyDown(Key.Down) && Keyboard.IsKeyDown(Key.Left))
            {
                Canvas.SetTop(playerImage, Canvas.GetTop(playerImage) + speed);
                Canvas.SetLeft(playerImage, Canvas.GetLeft(playerImage) - speed);
            }
            else if (Keyboard.IsKeyDown(Key.Down) && Keyboard.IsKeyDown(Key.Right))
            {
                Canvas.SetTop(playerImage, Canvas.GetTop(playerImage) + speed);
                Canvas.SetLeft(playerImage, Canvas.GetLeft(playerImage) + speed);
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Up:
                        Canvas.SetTop(playerImage, Canvas.GetTop(playerImage) - speed);
                        break;
                    case Key.Down:
                        Canvas.SetTop(playerImage, Canvas.GetTop(playerImage) + speed);
                        break;
                    case Key.Left:
                        Canvas.SetLeft(playerImage, Canvas.GetLeft(playerImage) - speed);
                        break;
                    case Key.Right:
                        Canvas.SetLeft(playerImage, Canvas.GetLeft(playerImage) + speed);
                        break;
                }
            }

            if (!IsWithinBounds(playerImage))
            {
                double left = Canvas.GetLeft(playerImage);
                double top = Canvas.GetTop(playerImage);
                double right = left + playerImage.ActualWidth;
                double bottom = top + playerImage.ActualHeight;
                double windowWidth = Application.Current.MainWindow.ActualWidth;
                double windowHeight = Application.Current.MainWindow.ActualHeight;

                if (left < 0)
                {
                    Canvas.SetLeft(playerImage, 0);
                }
                else if (right > windowWidth)
                {
                    Canvas.SetLeft(playerImage, windowWidth - playerImage.ActualWidth);
                }

                if (top < 0)
                {
                    Canvas.SetTop(playerImage, 0);
                }
                else if (bottom > windowHeight)
                {
                    Canvas.SetTop(playerImage, windowHeight - playerImage.ActualHeight);
                }
            }
        }

        private bool IsWithinBounds(Image player)
        {
            double left = Canvas.GetLeft(player);
            double top = Canvas.GetTop(player);
            double right = left + player.ActualWidth;
            double bottom = top + player.ActualHeight;
            double windowWidth = Application.Current.MainWindow.ActualWidth;
            double windowHeight = Application.Current.MainWindow.ActualHeight;

            return left >= 0 && top >= 0 && right <= windowWidth && bottom <= windowHeight;
        }

    }
}