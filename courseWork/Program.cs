using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GeographyQuizGame
{
    class Program
    {
        static void Main(string[] args)
        {
            QuizGame game = new QuizGame();
            game.Start();
        }
    }

    class QuizGame
    {
        private List<Question> questions = new List<Question>();
        private Random random = new Random();

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
                        if (questions.Count > 0)
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
                questions.Add(new MultipleChoiceQuestion(questionText, options, correctOption - 1));
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

            questions.Add(new OpenEndedQuestion(questionText, acceptableAnswers));
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

            questions.Add(new TrueFalseQuestion(statement, isTrue));
            Console.WriteLine("Question added successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void ViewAllQuestions()
        {
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
            ViewAllQuestions();
            if (questions.Count == 0) return;

            Console.WriteLine("Enter the number of the question to edit (or 0 to cancel):");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= questions.Count)
            {
                Question question = questions[index - 1];
                question.Edit();
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
            ViewAllQuestions();
            if (questions.Count == 0) return;

            Console.WriteLine("Enter the number of the question to delete (or 0 to cancel):");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= questions.Count)
            {
                questions.RemoveAt(index - 1);
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
                Console.Clear();
                Console.WriteLine("=== Correct Answers ===");
                for (int i = 0; i < questions.Count; i++)
                {
                    Console.WriteLine($"\nQuestion {i + 1}:");
                    questions[i].DisplayCorrectAnswer();
                }
            }

            Console.WriteLine("\nWould you like to play again? (Y/N)");
            if (!Console.ReadLine().ToUpper().StartsWith("Y"))
            {
                return;
            }
        }
    }

    abstract class Question
    {
        protected string QuestionText;

        public Question(string questionText)
        {
            QuestionText = questionText;
        }

        public abstract bool AskQuestion();
        public abstract void Display();
        public abstract void DisplayCorrectAnswer();
        public abstract void Edit();
    }

    class MultipleChoiceQuestion : Question
    {
        private List<string> options;
        private int correctOption;

        public MultipleChoiceQuestion(string questionText, List<string> options, int correctOption)
            : base(questionText)
        {
            this.options = options;
            this.correctOption = correctOption;
        }

        public override bool AskQuestion()
        {
            Console.WriteLine(QuestionText);
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
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
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"Option {i + 1}: {options[i]}");
            }
            Console.WriteLine($"Correct Option: {correctOption + 1}");
        }

        public override void DisplayCorrectAnswer()
        {
            Display();
        }

        public override void Edit()
        {
            Console.WriteLine("Enter new question text (or press Enter to keep current):");
            string newText = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newText))
            {
                QuestionText = newText;
            }

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"Enter new option {i + 1} (or press Enter to keep current):");
                string newOption = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newOption))
                {
                    options[i] = newOption;
                }
            }

            Console.WriteLine("Enter new correct option number (1-4, or press Enter to keep current):");
            string newCorrectOption = Console.ReadLine();
            if (int.TryParse(newCorrectOption, out int newCorrect) && newCorrect >= 1 && newCorrect <= 4)
            {
                correctOption = newCorrect - 1;
            }
        }
    }

    class OpenEndedQuestion : Question
    {
        private List<string> acceptableAnswers;

        public OpenEndedQuestion(string questionText, List<string> acceptableAnswers)
            : base(questionText)
        {
            this.acceptableAnswers = acceptableAnswers;
        }

        public override bool AskQuestion()
        {
            Console.WriteLine(QuestionText);
            Console.Write("Your answer: ");
            string answer = Console.ReadLine().Trim().ToLower();
            return acceptableAnswers.Contains(answer);
        }

        public override void Display()
        {
            Console.WriteLine($"Type: Open-Ended");
            Console.WriteLine($"Question: {QuestionText}");
            Console.WriteLine($"Acceptable Answers: {string.Join(", ", acceptableAnswers)}");
        }

        public override void DisplayCorrectAnswer()
        {
            Display();
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
                acceptableAnswers = newAnswers.Split(',').Select(a => a.Trim().ToLower()).ToList();
            }
        }
    }

    class TrueFalseQuestion : Question
    {
        private bool isTrue;

        public TrueFalseQuestion(string questionText, bool isTrue)
            : base(questionText)
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
            Display();
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
    }
}