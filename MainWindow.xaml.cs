using System;
using System.Windows;
using System.IO;

namespace SelbsttestPraktikum
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            bool parseYears = int.TryParse(YearsBox.Text, out int years);
            bool parseMonths = int.TryParse(MonthsBox.Text, out int months);
            bool parseHours = int.TryParse(HoursPerWeekBox.Text, out int hoursPerWeek);

            if (!parseYears || !parseMonths || !parseHours || string.IsNullOrWhiteSpace(name))
            {
                ResultText.Text = "❗ Bitte fülle alle Felder korrekt aus.";
                ResultText.Visibility = Visibility.Visible;
                return;
            }

            int totalWeeks = years * 52 + months * 4;
            int totalHours = totalWeeks * hoursPerWeek;
            bool usesTools = ToolsYes.IsChecked == true;
            bool stipendLimit = StipendiumYes.IsChecked == true;

            string result;

            if (totalHours >= 500 && usesTools && stipendLimit)
            {
                result = $"{name}, du hast {totalHours} Stunden gesammelt (über 500), verwendest relevante Tools und liegst an der Zuverdienstgrenze. → Eine Anrechnung des Pflichtpraktikums ist sinnvoll.";
            }
            else if (totalHours >= 500 && usesTools && !stipendLimit)
            {
                result = $"{name}, du hast {totalHours} Stunden gesammelt (über 500) und verwendest bereits relevante Tools. → Eine Anrechnung des Pflichtpraktikums ist sinnvoll.";
            }
            else if (totalHours >= 500 && !usesTools)
            {
                result = $"{name}, du hast {totalHours} Stunden gesammelt (über 500) aber du verwendest keine relevanten Tools. → Eine Absolvierung des Pflichtpraktikums ist sinnvoll.";
            }
            else if (totalHours < 500)
            {
                result = $"{name}, du hast nur {totalHours} Stunden gesammelt – für eine Anrechnung brauchst du mindestens 500.";
            }
            else
            {
                result = $"{name}, bitte prüfe noch einmal deine Angabe und starte den Test erneut.";
            }

            ResultText.Text = result;
            ResultText.Visibility = Visibility.Visible;

            // Ergebnis optional speichern (lokal als CSV)
            SaveResultAsCsv(name, totalHours, usesTools, stipendLimit, result);

            EvaluateButton.Visibility = Visibility.Collapsed;
            ResetButton.Visibility = Visibility.Visible;


        }

        private void SaveResultAsCsv(string name, int totalHours, bool usesTools, bool stipendLimit, string result)
        {
            string filePath = "test_results.csv";
            string line = $"\"{DateTime.Now}\",\"{name}\",{totalHours},{usesTools},{stipendLimit},\"{result}\"";

            try
            {
                bool newFile = !File.Exists(filePath);
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    if (newFile)
                    {
                        writer.WriteLine("Timestamp,Name,Gesamtstunden,Tools,Stipendium,Ergebnis");
                    }
                    writer.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}");
            }
     
        }


        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            NameBox.Text = "";
            YearsBox.Text = "";
            MonthsBox.Text = "";
            HoursPerWeekBox.Text = "";
            ToolsYes.IsChecked = false;
            ToolsNo.IsChecked = false;
            StipendiumYes.IsChecked = false;
            StipendiumNo.IsChecked = false;

            ResultText.Text = "";
            ResultText.Visibility = Visibility.Collapsed;
            ResetButton.Visibility = Visibility.Collapsed;

            EvaluateButton.Visibility = Visibility.Visible;

        }

    }
}
