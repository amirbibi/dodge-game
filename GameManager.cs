using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DodgeGame.Shared
{
    internal class GameManager
    {
        private Canvas canvas;
        private DispatcherTimer timer;
        private GameCharacter[] gameCharacters;
        private GameCharacter[] deserializedGameCharacters;
        private Enemy[] enemies;
        private Player player;
        private TextBlock pauseNotation;
        private TextBlock gameLostNotation;
        private TextBlock gameWonNotation;
        private int numberOfEnemies;
        private bool gameOn;
        private bool firstGame = true;
        private int speed = 5;


        public bool IsGameOn
        {
            get { return gameOn; }
        }
        public bool IsFirstGame
        {
            get { return firstGame; }
        }

        public GameManager(Canvas canvas, TextBlock pauseNotation, TextBlock gameLostNotation, TextBlock gameWonNotation)
        {
            this.canvas = canvas;
            this.pauseNotation = pauseNotation;
            this.gameLostNotation = gameLostNotation;
            this.gameWonNotation = gameWonNotation;
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Update();
        }

        public void Pause()
        {
            if (!player.gamePaused && timer != null)
            {
                Debug.WriteLine("Pause");
                timer.Stop();
                player.gamePaused = true;
                pauseNotation.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void Play()
        {
            if (timer != null)
            {
                Debug.WriteLine(enemies.Length);
                timer.Start();
                player.gamePaused = false;
                pauseNotation.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public void NewGame()
        {
            gameLostNotation.Visibility = System.Windows.Visibility.Collapsed;
            gameWonNotation.Visibility = System.Windows.Visibility.Collapsed;

            numberOfEnemies = 10;

            // Init player and enemies
            player = new Player(canvas);
            enemies = new Enemy[numberOfEnemies];

            gameCharacters = new GameCharacter[numberOfEnemies + 1];
            gameCharacters[numberOfEnemies] = player;

            // Init game manager
            for (int i = 0; i < numberOfEnemies; i++)
            {
                enemies[i] = new Enemy(canvas);
                gameCharacters[i] = enemies[i];
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                for (int j = i + 1; j < enemies.Length; j++)
                {
                    if (enemies[i].isAlive)
                        CheckCollision(enemies[i], enemies[j], true);
                }
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].isAlive)
                    CheckCollision(player, enemies[i], true);
            }

            gameOn = true;
            player.gamePaused = false;

            if (firstGame)
            {
                InitializeTimer();
                firstGame = false;
            }
        }

        public void ClearBoard()
        {
            /*
            canvas.Children.Remove(player.Image);
            */
            canvas.Children.Remove(player.Image);

            foreach (Enemy enemy in enemies)
                canvas.Children.Remove(enemy.Image);
        }

        private void FinishGame()
        {
            gameOn = false;
            ClearBoard();
        }

        private void GameWon()
        {
            Debug.WriteLine("Winner!");
            gameWonNotation.Visibility = System.Windows.Visibility.Visible;
            FinishGame();
        }

        private void GameLost()
        {
            Debug.WriteLine("Loser!");
            gameLostNotation.Visibility = System.Windows.Visibility.Visible;
            FinishGame();
        }

        private void Update()
        {
            // Move Enemeies
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].isAlive)
                    EnemeyMove(enemies[i]);
            }

            // Check of collisions
            // Between enemies
            for (int i = 0; i < enemies.Length; i++)
            {
                for (int j = i + 1; j < enemies.Length; j++)
                {
                    if (enemies[i].isAlive)
                        CheckCollision(enemies[i], enemies[j], false);
                }
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].isAlive)
                    CheckCollision(player, enemies[i], false);
            }
        }

        private void EnemeyMove(Enemy enemy)
        {
            CheckCollision(player, enemy, false);
            // Calculate the distance between the enemy and the player
            double xDist = player.X - enemy.X;
            double yDist = player.Y - enemy.Y;

            // Move the enemy towards the player by 'speed' pixels
            double angle = System.Math.Atan2(yDist, xDist);
            Canvas.SetLeft(enemy.Image, enemy.X + speed * System.Math.Cos(angle));
            Canvas.SetTop(enemy.Image, enemy.Y + speed * System.Math.Sin(angle));
        }

        private void CheckCollision(GameCharacter character1, GameCharacter character2, bool reset)
        {
            double xDist = character1.X - character2.X;
            double yDist = character1.Y - character2.Y;
            double distance = System.Math.Sqrt(xDist * xDist + yDist * yDist);

            if (character1 is Enemy && character2 is Enemy)
            {
                if (distance < 50) // Collision
                {
                    if (reset)
                    {
                        ClearBoard();
                        NewGame();
                        return;
                    }
                    canvas.Children.Remove((character1 as Enemy).Image);
                    (character1 as Enemy).isAlive = false;
                    numberOfEnemies--;
                    if (numberOfEnemies == 1)
                        GameWon();
                }
            }

            else if (character1 is Player && character2 is Enemy)
            {
                if (distance < 50) // Collision
                {
                    if (reset)
                    {
                        ClearBoard();
                        NewGame();
                        return;
                    }
                    GameLost();
                }
            }
        }

        public void SaveGame()
        {
            Debug.WriteLine("Saving Game...");
            string writeSavedText = "";
            for (int i = 0; i < gameCharacters.Length; i++)
            {
                writeSavedText += gameCharacters[i].X;
                writeSavedText += ", ";
                writeSavedText += gameCharacters[i].Y;
                writeSavedText += ", ";
            }
            File.WriteAllText("MyFile.txt", writeSavedText);
        }

        public void LoadGame()
        {
            Debug.WriteLine("Loading Game...");
            string readSavedText = File.ReadAllText("MyFile.txt");
            string[] savedTextArray = readSavedText.Split(", ");

            for (int i = 0; i < savedTextArray.Length - 1; i += 2)
            {
                gameCharacters[i / 2].X = int.Parse(savedTextArray[i]);
                gameCharacters[i / 2].Y = int.Parse(savedTextArray[i + 1]);
            }
        }
    }
}
