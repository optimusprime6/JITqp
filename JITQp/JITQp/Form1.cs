using JITQp.Models;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace JITQp
{
    public partial class Form1 : Form
    {
        Config configuration;

        public Form1()
        {
            InitializeComponent();
            try
            {
                configuration = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));

                DateTime now = DateTime.Now;

                foreach (Questionpaper questionPaper in configuration.questionPaper)
                {
                    if (now.Month <= 7 && questionPaper.sem % 2 == 0)
                        comboBox1.Items.Add(questionPaper.sem);

                    else if (now.Month > 7 && questionPaper.sem % 2 == 1)
                        comboBox1.Items.Add(questionPaper.sem);

                }
            }

            catch(Exception)
            {
                MessageBox.Show("No Config file found in the working directory");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int sem = Int16.Parse(comboBox1.Text);

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = "json";
                dialog.Filter = "JSON (.json) | *.json";
                dialog.Title = "Pick Questions";
                dialog.CheckFileExists = true;
                dialog.Multiselect = false;

                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    List<string> questions = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(dialog.FileName));

                    List<ChosenQuestion> ChosenQuestions = new List<ChosenQuestion>();

                    Questionpaper SemPaper = null;

                    foreach (Questionpaper p in configuration.questionPaper)
                    {
                        if (p.sem == sem)
                        {
                            SemPaper = p;
                            break;
                        }
                    }

                    Random random = new Random();

                    foreach(Pattern section in SemPaper.pattern)
                    {
                        for(int i = 0; i < section.numberOfQuestions; ++i)
                        {
                            int index = random.Next(0, questions.Count);

                            ChosenQuestion chosenQuestion = new ChosenQuestion();
                            chosenQuestion.Question = questions[index];
                            chosenQuestion.Marks = section.marksPerQuestion;

                            ChosenQuestions.Add(chosenQuestion);

                            questions.RemoveAt(index);
                        }
                    }

                    string result = JsonConvert.SerializeObject(ChosenQuestions, Formatting.Indented);

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "Select a file to store the chosen questions";
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.Filter = "Text | *.txt";

                    if(saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string resultFileName = saveFileDialog.FileName;
                        // ITerate through chosenQuestions, build a string, replace it in writeALlText function below
                        result = result.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "");
                        File.WriteAllText(resultFileName, result);
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Invalid Sem");
            }
        }
    }
}
