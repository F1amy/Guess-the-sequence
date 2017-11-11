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
        #region Lists and Variables declaration / + initialization

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

        #endregion

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

        private void Button_ChangeColor(int ButtonNumber, bool EnableMode)
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

        private void Sequence_CreateNew()
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

        private async void Sequence_Play()
        {
            foreach (Button CurrentItem in ButtonList)
            {
                CurrentItem.IsEnabled = false;
            }

            await Task.Delay(500);

            foreach (int CurrentItem in CurrentSequenceList)
            {
                Button_ChangeColor(CurrentItem - 1, true);
                await Task.Delay(1000);

                Button_ChangeColor(CurrentItem - 1, false);
                await Task.Delay(500);
            }

            for (int i = 0; i < ButtonList.Count; i++)
            {
                Button_ChangeColor(i, true);
            }

            foreach (Button CurrentItem in ButtonList)
            {
                CurrentItem.IsEnabled = true;
            }

            InformationTextBlock.Text = "Guess";
        }

        private void Game_Won(bool isWon)
        {
            for (int i = 0; i < ButtonList.Count; i++)
            {
                Button_ChangeColor(i, false);
            }

            foreach (Button CurrentItem in ButtonList)
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
            Sequence_CreateNew();

            InformationTextBlock.Text = "Watch";
            StartButton.IsEnabled = false;
            
            Sequence_Play();
        }

        private void AnyButton_Click(object sender, RoutedEventArgs e)
        {
            Button CurrentButton = (Button)sender;

            int Row = Grid.GetRow(CurrentButton);
            int Column = Grid.GetColumn(CurrentButton);
            int ButtonNumber = 0;

            switch (Column.ToString() + Row.ToString())
            {
                case "11":
                    ButtonNumber = 1;
                    break;
                case "31":
                    ButtonNumber = 2;
                    break;
                case "51":
                    ButtonNumber = 3;
                    break;
                case "13":
                    ButtonNumber = 4;
                    break;
                case "33":
                    ButtonNumber = 5;
                    break;
                case "53":
                    ButtonNumber = 6;
                    break;
                default:
                    DebugStringTextBlock.Text = "Error: No such case in Row Column Switch.";
                    break;
            }

            if (CurrentSequenceList[CurrentGuessBlock - 1] == ButtonNumber && CurrentGuessBlock == CurrentSequenceList.Count)
            {
                Game_Won(true);
            }
            else if (CurrentSequenceList[CurrentGuessBlock - 1] != ButtonNumber)
            {
                Game_Won(false);
            }
            else
            {
                CurrentGuessBlock++;
            }
        }
    }
}
