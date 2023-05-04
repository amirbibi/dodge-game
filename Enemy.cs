using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DodgeGame.Shared
{
    public class Enemy : GameCharacter
    {
        private Image enemyImage;
        private static int counter = 0;
        public int id = 0;
        public bool isAlive;

        public override int X
        {
            get { return (int)Canvas.GetLeft(enemyImage); }
            set { Canvas.SetLeft(enemyImage, value); }
        }

        public override int Y
        {
            get { return (int)Canvas.GetTop(enemyImage); }
            set { Canvas.SetTop(enemyImage, value); }
        }

        public Image Image
        {
            get { return enemyImage; }
        }
        public Enemy() : base()
        {
        }

        public Enemy(Canvas canvas) : base(canvas)
        {
            counter++;
            id = counter;
            isAlive = true;
            if (counter == 10) counter = 1;
            Draw();
        }

        protected override void Draw()
        {
            enemyImage = new Image
            {
                Height = 50,
                Width = 50,
                Source = new BitmapImage(new Uri(path + "\\enemy" + counter + ".png", UriKind.RelativeOrAbsolute))
            };

            Canvas.SetLeft(enemyImage, rand.NextInt64(800));
            Canvas.SetTop(enemyImage, rand.NextInt64(600));
            canvas.Children.Add(enemyImage);

        }
    }
}