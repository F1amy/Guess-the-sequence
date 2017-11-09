using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Guess_the_sequence
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Button> ButtonList = new List<Button>();
        private List<int> CurrentSequenceList = new List<int>();
        private List<SolidColorBrush> BrushesList = new List<SolidColorBrush>()
        {
            Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Purple
        };
        private List<SolidColorBrush> DarkBrushesList = new List<SolidColorBrush>()
        {
            new BrushConverter().ConvertFromString("#FF7F0000") as SolidColorBrush, //darkRed
            new BrushConverter().ConvertFromString("#FF805700") as SolidColorBrush, //darkOrange
            new BrushConverter().ConvertFromString("#FF7A8000") as SolidColorBrush, //darkYellow
            new BrushConverter().ConvertFromString("#FF004700") as SolidColorBrush, //darkGreen
            new BrushConverter().ConvertFromString("#FF000055") as SolidColorBrush, //darkBlue
            new BrushConverter().ConvertFromString("#FF3C003C") as SolidColorBrush  //darkPurple
        };
        private int CurrentGuessBlock = 1, CurrentScore = 0, SequenceMaximum = 6;

        public MainWindow()
        {
            InitializeComponent();

            ButtonList.Add(RedButton1);
            ButtonList.Add(OrangeButton2);
            ButtonList.Add(YellowButton3);
            ButtonList.Add(GreenButton4);
            ButtonList.Add(BlueButton5);
            ButtonList.Add(PurpleButton6);
        }

        private void ButtonChangeColor(int ButtonNumber, bool EnableMode)
        {
            if (EnableMode == true)
            {
                ButtonList[ButtonNumber].Background = BrushesList[ButtonNumber];
            }
            else if (EnableMode == false)
            {
                ButtonList[ButtonNumber].Background = DarkBrushesList[ButtonNumber];
            }
        }

        private void CreateNewSequence()
        {
            Random Random = new Random();

            CurrentSequenceList.Clear();
            SequenceMaximum = Random.Next(6, 10 + 1);

            for (int i = 0; i < SequenceMaximum; i++)
            {
                CurrentSequenceList.Add(Random.Next(1, 7));
            }

            #if DEBUG

            DebugStringTextBlock.Text = "Sequence: " + string.Join(", ", CurrentSequenceList) + ".";

            #endif
        }

        private async void PlaySequence()
        {
            foreach (Button CurrentItem in ButtonList) //turn off the buttons
            {
                CurrentItem.IsEnabled = false;
            }

            await Task.Delay(500); //wait 500 milliseconds

            foreach (int CurrentItem in CurrentSequenceList)
            {
                ButtonChangeColor(CurrentItem - 1, true);
                await Task.Delay(1000); //wait 1000 milliseconds

                ButtonChangeColor(CurrentItem - 1, false);
                await Task.Delay(500);
            }

            for (int i = 0; i < ButtonList.Count; i++)
            {
                ButtonChangeColor(i, true);
            }

            foreach (Button CurrentItem in ButtonList) //turn on the buttons
            {
                CurrentItem.IsEnabled = true;
            }

            InformationTextBlock.Text = "Guess";
        }

        private void GameWon(bool isWon)
        {
            for (int i = 0; i < ButtonList.Count; i++)
            {
                ButtonChangeColor(i, false);
            }

            foreach (Button CurrentItem in ButtonList) //turn off the buttons
            {
                CurrentItem.IsEnabled = false;
            }

            CurrentGuessBlock = 1;
            StartButton.IsEnabled = true;

            if (isWon == true)
            {
                CurrentScore += 100 * SequenceMaximum;
            }
            else if (isWon == false)
            {
                if (CurrentScore > 50 * SequenceMaximum)
                {
                    CurrentScore -= 50 * SequenceMaximum;
                }
                else
                {
                    CurrentScore = 0;
                }
            }

            InformationTextBlock.Text = isWon == true ? "Correct" : "Wrong";
            ScoreTextBlock.Text = "Score: " + CurrentScore.ToString();

            #if DEBUG

            DebugStringTextBlock.Text = string.Empty;

            #endif
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewSequence();

            InformationTextBlock.Text = "Watch";
            StartButton.IsEnabled = false;
            
            PlaySequence();
        }

        private void AnyButton_Click(int ButtonNumber)
        {
            if (CurrentSequenceList[CurrentGuessBlock - 1] == ButtonNumber && CurrentGuessBlock == CurrentSequenceList.Count)
            {
                GameWon(true);
            }
            else if (CurrentSequenceList[CurrentGuessBlock - 1] != ButtonNumber)
            {
                GameWon(false);
            }
            else
            {
                CurrentGuessBlock++;
            }
        }

        private void RedButton1_Click(object sender, RoutedEventArgs e) => AnyButton_Click(1);

        private void OrangeButton2_Click(object sender, RoutedEventArgs e) => AnyButton_Click(2);

        private void YellowButton3_Click(object sender, RoutedEventArgs e) => AnyButton_Click(3);

        private void GreenButton4_Click(object sender, RoutedEventArgs e) => AnyButton_Click(4);

        private void BlueButton5_Click(object sender, RoutedEventArgs e) => AnyButton_Click(5);

        private void PurpleButton6_Click(object sender, RoutedEventArgs e) => AnyButton_Click(6);
    }
}
