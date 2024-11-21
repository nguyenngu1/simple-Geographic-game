using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace GeographyQuizGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=comp1551_quizgame;Uid=root;Pwd=;";
            QuizGame game = new QuizGame(connectionString);
            game.Start();
        }
    }

    class QuizGame
    {
        private string connectionString;
        private Random random = new Random();

        public QuizGame(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Questions (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        QuestionType VARCHAR(20) NOT NULL,
                        QuestionText TEXT NOT NULL,
                        Options TEXT,
                        CorrectAnswer TEXT NOT NULL
                    )";
                using (var command = new MySqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Geography Quiz Game ===");
                Console.WriteLine("1. Create/Edit Game");
                Console.WriteLine("2. Play Game");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        EditMode();
                        break;
                    case "2":
                        if (GetQuestionCount() > 0)
                            PlayMode();
                        else
                        {
                            Console.WriteLine("No questions available. Please add questions first.");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                        }
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void EditMode()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Edit Mode ===");
                Console.WriteLine("1. Add Multiple Choice Question");
                Console.WriteLine("2. Add Open-Ended Question");
                Console.WriteLine("3. Add True/False Question");
                Console.WriteLine("4. View All Questions");
                Console.WriteLine("5. Edit Question");
                Console.WriteLine("6. Delete Question");
                Console.WriteLine("7. Return to Main Menu");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddMultipleChoiceQuestion();
                        break;
                    case "2":
                        AddOpenEndedQuestion();
                        break;
                    case "3":
                        AddTrueFalseQuestion();
                        break;
                    case "4":
                        ViewAllQuestions();
                        break;
                    case "5":
                        EditQuestion();
                        break;
                    case "6":
                        DeleteQuestion();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddMultipleChoiceQuestion()
        {
            Console.WriteLine("\nEnter the question text:");
            string questionText = Console.ReadLine();

            List<string> options = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Enter option {i + 1}:");
                options.Add(Console.ReadLine());
            }

            Console.WriteLine("Enter the number of the correct option (1-4):");
            if (int.TryParse(Console.ReadLine(), out int correctOption) && correctOption >= 1 && correctOption <= 4)
            {
                string optionsString = string.Join("|", options);
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Questions (QuestionType, QuestionText, Options, CorrectAnswer) VALUES (@type, @text, @options, @answer)";
                    using (var command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@type", "MultipleChoice");
                        command.Parameters.AddWithValue("@text", questionText);
                        command.Parameters.AddWithValue("@options", optionsString);
                        command.Parameters.AddWithValue("@answer", correctOption - 1);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Question added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid correct option. Question not added.");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void AddOpenEndedQuestion()
        {
            Console.WriteLine("\nEnter the question text:");
            string questionText = Console.ReadLine();

            Console.WriteLine("Enter acceptable answers (comma-separated):");
            string answersInput = Console.ReadLine();
            List<string> acceptableAnswers = answersInput.Split(',').Select(a => a.Trim().ToLower()).ToList();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Questions (QuestionType, QuestionText, CorrectAnswer) VALUES (@type, @text, @answer)";
                using (var command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@type", "OpenEnded");
                    command.Parameters.AddWithValue("@text", questionText);
                    command.Parameters.AddWithValue("@answer", string.Join("|", acceptableAnswers));
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Question added successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void AddTrueFalseQuestion()
        {
            Console.WriteLine("\nEnter the statement:");
            string statement = Console.ReadLine();

            Console.WriteLine("Is this statement true or false? (T/F):");
            bool isTrue = Console.ReadLine().ToUpper().StartsWith("T");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Questions (QuestionType, QuestionText, CorrectAnswer) VALUES (@type, @text, @answer)";
                using (var command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@type", "TrueFalse");
                    command.Parameters.AddWithValue("@text", statement);
                    command.Parameters.AddWithValue("@answer", isTrue);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Question added successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void ViewAllQuestions()
        {
            List<Question> questions = GetAllQuestions();
            Console.Clear();
            if (questions.Count == 0)
            {
                Console.WriteLine("No questions available.");
            }
            else
            {
                for (int i = 0; i < questions.Count; i++)
                {
                    Console.WriteLine($"\nQuestion {i + 1}:");
                    questions[i].Display();
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        private void EditQuestion()
        {
            List<Question> questions = GetAllQuestions();
            ViewAllQuestions();
            if (questions.Count == 0) return;

            Console.WriteLine("Enter the number of the question to edit (or 0 to cancel):");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= questions.Count)
            {
                Question question = questions[index - 1];
                question.Edit();

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Questions SET QuestionText = @text, Options = @options, CorrectAnswer = @answer WHERE Id = @id";
                    using (var command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", question.Id);
                        command.Parameters.AddWithValue("@text", question.QuestionText);

                        // Handle the Options parameter
                        if (question is MultipleChoiceQuestion mcq)
                        {
                            command.Parameters.AddWithValue("@options", string.Join("|", mcq.Options));
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@options", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@answer", question.GetCorrectAnswerString());
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Question updated successfully!");
            }
            else if (index != 0)
            {
                Console.WriteLine("Invalid question number.");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void DeleteQuestion()
        {
            List<Question> questions = GetAllQuestions();
            ViewAllQuestions();
            if (questions.Count == 0) return;

            Console.WriteLine("Enter the number of the question to delete (or 0 to cancel):");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= questions.Count)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Questions WHERE Id = @id";
                    using (var command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", questions[index - 1].Id);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Question deleted successfully!");
            }
            else if (index != 0)
            {
                Console.WriteLine("Invalid question number.");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void PlayMode()
        {
            Console.Clear();
            Console.WriteLine("=== Play Mode ===");
            Console.WriteLine("Press any key to start the quiz...");
            Console.ReadKey();

            List<Question> questions = GetAllQuestions();
            List<Question> shuffledQuestions = questions.OrderBy(q => random.Next()).ToList();
            int correctAnswers = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < shuffledQuestions.Count; i++)
            {
                Console.Clear();
                Console.WriteLine($"Question {i + 1} of {shuffledQuestions.Count}:");
                if (shuffledQuestions[i].AskQuestion())
                {
                    correctAnswers++;
                }
                Console.WriteLine("\nPress any key for the next question...");
                Console.ReadKey();
            }

            stopwatch.Stop();
            DisplayResults(correctAnswers, shuffledQuestions.Count, stopwatch.Elapsed);
        }

        private void DisplayResults(int correctAnswers, int totalQuestions, TimeSpan timeSpent)
        {
            Console.Clear();
            Console.WriteLine("=== Quiz Results ===");
            Console.WriteLine($"Correct Answers: {correctAnswers} out of {totalQuestions}");
            Console.WriteLine($"Time Spent: {timeSpent.TotalMinutes:F2} minutes");
            Console.WriteLine($"Score: {(double)correctAnswers / totalQuestions * 100:F2}%");

            Console.WriteLine("\nWould you like to see the correct answers? (Y/N)");
            if (Console.ReadLine().ToUpper().StartsWith("Y"))
            {
                List<Question> questions = GetAllQuestions();
                Console.Clear();
                Console.WriteLine("=== Correct Answers ===");
                for (int i = 0; i < questions.Count; i++)
                {
                    Console.WriteLine($"\nQuestion {i + 1}:");
                    questions[i].DisplayCorrectAnswer();
                }
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        private List<Question> GetAllQuestions()
        {
            List<Question> questions = new List<Question>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Questions";
                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["Id"];
                            string questionType = reader["QuestionType"].ToString();
                            string questionText = reader["QuestionText"].ToString();
                            string options = reader["Options"]?.ToString();
                            string correctAnswer = reader["CorrectAnswer"].ToString();

                            Question question = null;
                            switch (questionType)
                            {
                                case "MultipleChoice":
                                    question = new MultipleChoiceQuestion(id, questionText, options.Split('|').ToList(), int.Parse(correctAnswer));
                                    break;
                                case "TrueFalse":
                                    bool isTrue = correctAnswer.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                      correctAnswer.Equals("1");
                                    question = new TrueFalseQuestion(id, questionText, isTrue);
                                    break;
                                case "OpenEnded":
                                    question = new OpenEndedQuestion(id, questionText, correctAnswer.Split('|').ToList());
                                    break;
                            }
                            if (question != null)
                            {
                                questions.Add(question);
                            }
                        }
                    }
                }
            }
            return questions;
        }

        private int GetQuestionCount()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string countQuery = "SELECT COUNT(*) FROM Questions";
                using (var command = new MySqlCommand(countQuery, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

    }

    abstract class Question
    {
        public int Id { get; protected set; }
        public string QuestionText { get; protected set; }

        public Question(int id, string questionText)
        {
            Id = id;
            QuestionText = questionText;
        }

        public abstract bool AskQuestion();
        public abstract void Display();
        public abstract void DisplayCorrectAnswer();
        public abstract void Edit();
        public abstract string GetCorrectAnswerString();
    }

    class MultipleChoiceQuestion : Question
    {
        public List<string> Options { get; private set; }
        private int correctOption;

        public MultipleChoiceQuestion(int id, string questionText, List<string> options, int correctOption)
            : base(id, questionText)
        {
            Options = options;
            this.correctOption = correctOption;
        }

        public override bool AskQuestion()
        {
            Console.WriteLine(QuestionText);
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Options[i]}");
            }

            Console.Write("Your answer (1-4): ");
            if (int.TryParse(Console.ReadLine(), out int answer) && answer >= 1 && answer <= 4)
            {
                return answer - 1 == correctOption;
            }
            return false;
        }

        public override void Display()
        {
            Console.WriteLine($"Type: Multiple Choice");
            Console.WriteLine($"Question: {QuestionText}");
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"Option {i + 1}: {Options[i]}");
            }
            Console.WriteLine($"Correct Option: {correctOption + 1}");
        }

        public override void DisplayCorrectAnswer()
        {
            Console.WriteLine($"Question: {QuestionText}");
            Console.WriteLine($"Correct Answer: {Options[correctOption]}");
        }

        public override void Edit()
        {
            Console.WriteLine("Enter new question text (or press Enter to keep current):");
            string newText = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newText))
            {
                QuestionText = newText;
            }

            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"Enter new option {i + 1} (or press Enter to keep current):");
                string newOption = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newOption))
                {
                    Options[i] = newOption;
                }
            }

            Console.WriteLine("Enter new correct option number (1-4, or press Enter to keep current):");
            string newCorrectOption = Console.ReadLine();
            if (int.TryParse(newCorrectOption, out int newCorrect) && newCorrect >= 1 && newCorrect <= 4)
            {
                correctOption = newCorrect - 1;
            }
        }

        public override string GetCorrectAnswerString()
        {
            return correctOption.ToString();
        }
    }


    class OpenEndedQuestion : Question
    {
        private List<string> AcceptableAnswers { get; set; }

        public OpenEndedQuestion(int id, string questionText, List<string> acceptableAnswers)
            : base(id, questionText)
        {
            AcceptableAnswers = acceptableAnswers;
        }

        public override bool AskQuestion()
        {
            Console.WriteLine(QuestionText);
            string answer = Console.ReadLine().ToLower();
            return AcceptableAnswers.Any(a => a.Equals(answer));
        }

        public override void Display()
        {
            Console.WriteLine($"Type: Open Ended");
            Console.WriteLine($"Question: {QuestionText}");
            Console.WriteLine($"Acceptable Answers: {string.Join(", ", AcceptableAnswers)}");
        }

        public override void DisplayCorrectAnswer()
        {
            Console.WriteLine($"Question: {QuestionText}");
            Console.WriteLine($"Acceptable Answers: {string.Join(", ", AcceptableAnswers)}");
        }

        public override void Edit()
        {
            Console.WriteLine("Enter new question text (or press Enter to keep current):");
            string newText = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newText))
            {
                QuestionText = newText;
            }

            Console.WriteLine("Enter new acceptable answers (comma-separated, or press Enter to keep current):");
            string newAnswers = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAnswers))
            {
                AcceptableAnswers = newAnswers.Split(',').Select(a => a.Trim().ToLower()).ToList();
            }
        }

        public override string GetCorrectAnswerString()
        {
            return string.Join("|", AcceptableAnswers);
        }
    }


    class TrueFalseQuestion : Question
    {
        private bool isTrue;

        public TrueFalseQuestion(int id, string questionText, bool isTrue)
            : base(id, questionText)
        {
            this.isTrue = isTrue;
        }

        public override bool AskQuestion()
        {
            Console.WriteLine(QuestionText);
            Console.Write("True or False? (T/F): ");
            string answer = Console.ReadLine().ToUpper();
            return (answer.StartsWith("T") == isTrue);
        }

        public override void Display()
        {
            Console.WriteLine($"Type: True/False");
            Console.WriteLine($"Statement: {QuestionText}");
            Console.WriteLine($"Correct Answer: {(isTrue ? "True" : "False")}");
        }

        public override void DisplayCorrectAnswer()
        {
            Console.WriteLine($"Statement: {QuestionText}");
            Console.WriteLine($"Correct Answer: {(isTrue ? "True" : "False")}");
        }

        public override void Edit()
        {
            Console.WriteLine("Enter new statement (or press Enter to keep current):");
            string newText = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newText))
            {
                QuestionText = newText;
            }

            Console.WriteLine("Enter new correct answer (T/F, or press Enter to keep current):");
            string newAnswer = Console.ReadLine().ToUpper();
            if (newAnswer.StartsWith("T") || newAnswer.StartsWith("F"))
            {
                isTrue = newAnswer.StartsWith("T");
            }
        }

        public override string GetCorrectAnswerString()
        {
            return isTrue.ToString();
        }
    }
}
