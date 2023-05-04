using System.Windows;

namespace DodgeGame.Shared
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameManager gameManager;

        public MainWindow()
        {
            // Init window
            InitializeComponent();

            // Init game manager
            gameManager = new GameManager(GameCanvas, PauseNotation, GameLostNotation, GameWonNotation);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Pause();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Play();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (!gameManager.IsFirstGame)
                gameManager.ClearBoard();
            gameManager.NewGame();
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            gameManager.SaveGame();
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            gameManager.LoadGame();
        }
    }
}
