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
		private int CurrentGuessBlock = 1,
					CurrentScore = 0,
					SequenceMaximum = 6;

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

		private void ChangeButtonColor(int ButtonNumber, bool VisibilityMode)
		{
			if (VisibilityMode == true)
			{
				ButtonList[ButtonNumber].Background = BrushesList[ButtonNumber];
			}
			else if (VisibilityMode == false)
			{
				ButtonList[ButtonNumber].Background = DarkBrushesList[ButtonNumber];
			}
		}

		private void CreateNewSequence()
		{
			var Random = new Random();

			CurrentSequenceList.Clear();
			SequenceMaximum = Random.Next(6, 10 + 1);

			for (var i = 0; i < SequenceMaximum; i++)
			{
				CurrentSequenceList.Add(Random.Next(1, 7));
			}

			#if DEBUG

			DebugStringTextBlock.Text = "Sequence: " + "{ " + string.Join(", ", CurrentSequenceList) + " }" + ".";

			#endif
		}

		private async void PlaySequence()
		{
			foreach (var CurrentItem in ButtonList)
			{
				CurrentItem.IsEnabled = false;
			}

			await Task.Delay(500);

			foreach (var CurrentItem in CurrentSequenceList)
			{
				ChangeButtonColor(CurrentItem - 1, true);
				await Task.Delay(1000);

				ChangeButtonColor(CurrentItem - 1, false);
				await Task.Delay(500);
			}

			for (var i = 0; i < ButtonList.Count; i++)
			{
				ChangeButtonColor(i, true);
			}

			foreach (var CurrentItem in ButtonList)
			{
				CurrentItem.IsEnabled = true;
			}

			InformationTextBlock.Text = "Guess";
		}

		private void GameWon(bool isWon)
		{
			for (var i = 0; i < ButtonList.Count; i++)
			{
				ChangeButtonColor(i, false);
			}

			foreach (var CurrentItem in ButtonList)
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

		private void AnyButton_Click(object sender, RoutedEventArgs e)
		{
			var CurrentButton = (Button)sender;

			int Row = Grid.GetRow(CurrentButton);
			int Column = Grid.GetColumn(CurrentButton);
			var ButtonNumber = 0;

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
	}
}
