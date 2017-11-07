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
        private List<Button> button_array = new List<Button>();
        private List<int> current_sequence_array = new List<int>();
        private List<SolidColorBrush> brushes_array = new List<SolidColorBrush>()
        {
            Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Purple
        };
        private List<SolidColorBrush> dark_brushes_array = new List<SolidColorBrush>()
        {
            new BrushConverter().ConvertFromString("#FF7F0000") as SolidColorBrush, //darkRed
            new BrushConverter().ConvertFromString("#FF805700") as SolidColorBrush, //darkOrange
            new BrushConverter().ConvertFromString("#FF7A8000") as SolidColorBrush, //darkYellow
            new BrushConverter().ConvertFromString("#FF004700") as SolidColorBrush, //darkGreen
            new BrushConverter().ConvertFromString("#FF000055") as SolidColorBrush, //darkBlue
            new BrushConverter().ConvertFromString("#FF3C003C") as SolidColorBrush  //darkPurple
        };
        private int current_guess_block = 1, current_score = 0, seq_max = 6;

        public MainWindow()
        {
            InitializeComponent();

            button_array.Add(button1_red);
            button_array.Add(button2_orange);
            button_array.Add(button3_yellow);
            button_array.Add(button4_green);
            button_array.Add(button5_blue);
            button_array.Add(button6_purple);
        }

        private void button_change_color(int button_number, bool enable)
        {
            if (enable == true)
            {
                button_array[button_number].Background = brushes_array[button_number];
            }
            else if (enable == false)
            {
                button_array[button_number].Background = dark_brushes_array[button_number];
            }
        }

        private void create_new_sequence()
        {
            Random rnd = new Random();

            current_sequence_array.Clear();
            seq_max = rnd.Next(6, 10 + 1);

            for (int i = 0; i < seq_max; i++)
            {
                current_sequence_array.Add(rnd.Next(1, 7));
            }
        }

        private async void play_sequence()
        {
            foreach (Button current_element in button_array) //turn off the buttons
            {
                current_element.IsEnabled = false;
            }

            await Task.Delay(500); //wait 500 milliseconds

            foreach (int current_element in current_sequence_array)
            {
                button_change_color(current_element - 1, true);
                await Task.Delay(1000); //wait 1000 milliseconds

                button_change_color(current_element - 1, false);
                await Task.Delay(500);
            }

            for (int i = 0; i < button_array.Count; i++)
            {
                button_change_color(i, true);
            }

            foreach (Button current_element in button_array) //turn on the buttons
            {
                current_element.IsEnabled = true;
            }

            textBlock_information.Text = "Guess";
        }

        private void game_win()
        {
            for (int i = 0; i < button_array.Count; i++)
            {
                button_change_color(i, false);
            }

            foreach (Button current_element in button_array) //turn off the buttons
            {
                current_element.IsEnabled = false;
            }

            button_start.IsEnabled = true;
            current_guess_block = 1;
            current_score += 100 * seq_max;
            textBlock_information.Text = "Correct";
            textBlock_score.Text = "Score: " + current_score.ToString();
        }

        private void game_fail()
        {
            for (int i = 0; i < button_array.Count; i++)
            {
                button_change_color(i, false);
            }

            foreach (Button current_element in button_array) //turn off the buttons
            {
                current_element.IsEnabled = false;
            }

            current_guess_block = 1;
            button_start.IsEnabled = true;

            if (current_score > 50 * seq_max)
            {
                current_score -= 50 * seq_max;
            }
            else
            {
                current_score = 0;
            }

            textBlock_information.Text = "Wrong";
            textBlock_score.Text = "Score: " + current_score.ToString();
        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            create_new_sequence();

            textBlock_information.Text = "Watch";
            button_start.IsEnabled = false;
            
            play_sequence();
        }

        private void button_all_Click(int button_index)
        {
            if (current_sequence_array[current_guess_block - 1] == button_index && current_guess_block == current_sequence_array.Count)
            {
                game_win();
            }
            else if (current_sequence_array[current_guess_block - 1] != button_index)
            {
                game_fail();
            }
            else
            {
                current_guess_block++;
            }
        }

        private void button1_red_Click(object sender, RoutedEventArgs e) => button_all_Click(1);

        private void button2_orange_Click(object sender, RoutedEventArgs e) => button_all_Click(2);

        private void button3_yellow_Click(object sender, RoutedEventArgs e) => button_all_Click(3);

        private void button4_green_Click(object sender, RoutedEventArgs e) => button_all_Click(4);

        private void button5_blue_Click(object sender, RoutedEventArgs e) => button_all_Click(5);

        private void button6_purple_Click(object sender, RoutedEventArgs e) => button_all_Click(6);
    }
}
