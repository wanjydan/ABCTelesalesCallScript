using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ABCTelesalesCallScript
{
    public class StartApp
    {
        public void Call()
        {
            var response = AskQuestion(1);
            if (response.Equals("yes"))
            {
                response = AskQuestion(3);
                if (response.Equals("yes"))
                {
                    AskQuestion(4);
                    AskQuestion(16);

                    response = AskQuestion(5);
                    var reviewOption = (ReviewOption) Int32.Parse(response);
                    if (reviewOption == ReviewOption.Bad || reviewOption == ReviewOption.Poor)
                    {
                        response = AskQuestion(15);
                        if (response.Equals("no"))
                        {
                            var disposition = CaptureDisposition();
                            NotInterested();
                        }
                    }

                    response = AskQuestion(6);
                    var systemOption = (SystemOption) Int32.Parse(response);
                    if (systemOption == SystemOption.Others)
                    {
                        var system = CaptureData("Enter the system you use");
                    }

                    AskQuestion(7);
                    AskQuestion(8);
                    AskQuestion(9);

                    response = AskQuestion(10);
                    if (response.Equals("yes"))
                    {
                        var dateTime = CaptureDateTime();
                    }
                    else
                        NotInterested();

                    AskQuestion(11);
                    var emailId = CaptureEmail();

                    response = AskQuestion(12);
                    if (response.Equals("yes"))
                    {
                        var details = CaptureData("Enter details");
                    }
                    else
                        EndCall();

                    response = AskQuestion(13);
                    if (response.Equals("yes"))
                    {
                        Console.WriteLine("Enter your email address");
                        var email = CaptureEmail();
                        var phone = CapturePhone();
                        SendEmail(email);
                        SendSms(phone);
                    }
                    else
                        EndCall();
                }
                else
                    NotInterested();
            }
            else
            {
                response = AskQuestion(2);
                if (response.Equals("yes"))
                {
                    var dateTime = CaptureDateTime();
                    CallBack(dateTime);
                }
                else
                {
                    var disposition = CaptureDisposition();
                    NotInterested();
                }
            }
        }

        private static string AskQuestion(int questionNumber)
        {
            var question = Core.Questions[questionNumber - 1].Item1;
            var type = Core.Questions[questionNumber - 1].Item2;

            var options = "";
            switch (type)
            {
                case QuestionType.YesNo:
                    options = "yes/no";
                    break;
                case QuestionType.AgreeDisagree:
                    options = "agree/disagree";
                    break;
                case QuestionType.Review:
                case QuestionType.System:
                    options = "Select an option";
                    break;
            }

            Console.WriteLine($"{question} {options}");

            if (type == QuestionType.Review)
            {
                var reviewOptions = Enum.GetValues(typeof(ReviewOption));

                var i = 1;
                foreach (var option in reviewOptions)
                {
                    Console.WriteLine($"{i}. {option}");
                    i++;
                }
            }

            if (type == QuestionType.System)
            {
                var systemOptions = Enum.GetValues(typeof(SystemOption));

                var i = 1;
                foreach (var option in systemOptions)
                {
                    Console.WriteLine($"{i}. {option}");
                    i++;
                }
            }

            if (type == QuestionType.Void)
                return string.Empty;

            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    switch (type)
                    {
                        case QuestionType.YesNo:
                            input = input.Trim().ToLower();
                            if (input.Equals("yes") || input.Equals("no"))
                                return input;
                            Console.WriteLine("Enter a yes/no");
                            break;
                        case QuestionType.AgreeDisagree:
                            input = input.Trim().ToLower();
                            if (input.Equals("agree") || input.Equals("disagree"))
                                return input;
                            Console.WriteLine("Enter a agree/disagree");
                            break;
                        case QuestionType.Review:
                            input = input.Trim().ToLower();
                            if (Int32.TryParse(input, out var reviewOption))
                                if (Enum.IsDefined(typeof(ReviewOption), reviewOption - 1))
                                    return (reviewOption - 1).ToString();
                            Console.WriteLine("Enter a valid option");
                            break;
                        case QuestionType.System:
                            input = input.Trim().ToLower();
                            if (Int32.TryParse(input, out var systemOption))
                                if (Enum.IsDefined(typeof(SystemOption), systemOption - 1))
                                    return (systemOption - 1).ToString();
                            Console.WriteLine("Enter a valid option");
                            break;
                        default:
                            return input;
                    }
                }
            }
        }

        private DateTime CaptureDateTime()
        {
            const string dateFormat = "dd/MM/yyyy";
            const string timeFormat = "HH:mm";
            var cultureInfo = CultureInfo.InvariantCulture;

            Console.WriteLine($"Enter date in this format: {dateFormat}");

            string dateString;

            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    if (DateTime.TryParseExact(input, dateFormat, cultureInfo, DateTimeStyles.None, out _))
                    {
                        dateString = input;
                        break;
                    }

                Console.WriteLine($"Enter a valid date in the format: {dateFormat}");
            }

            Console.WriteLine($"Enter time in this format: {timeFormat}");

            while (true)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    if (DateTime.TryParseExact($"{dateString} {input}", $"{dateFormat} {timeFormat}", cultureInfo,
                        DateTimeStyles.None,
                        out _))
                        break;

                Console.WriteLine($"Enter a valid time in the format: {timeFormat}");
            }

            return DateTime.ParseExact($"{dateString} {input}", $"{dateFormat} {timeFormat}", cultureInfo);
        }

        private static string CaptureEmail()
        {
            // Console.WriteLine("Enter your email address");

            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    if (IsValidEmail(input))
                        return input;

                Console.WriteLine("Enter a valid email address");
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static string CaptureData(string prompt)
        {
            Console.WriteLine(prompt);

            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
            }
        }

        private static string CapturePhone()
        {
            Console.WriteLine("Enter phone number with country dial code");

            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    if (Regex.Match(input, @"^(\+[1-9]{3}[0-9]{9})$").Success)
                        return input;

                Console.WriteLine("Enter a valid phone number");
            }
        }

        private static string CaptureDisposition()
        {
            var options = new List<string>()
            {
                "Number doesnt’t belong to the customer",
                "Not Interested"
            };

            var i = 1;
            foreach (var option in options)
            {
                Console.WriteLine($"{i}. {option}");
                i++;
            }

            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (Int32.TryParse(input, out var option))
                        if (Enumerable.Range(1, options.Count).Contains(option))
                            return options[option - 1];
                }

                Console.WriteLine("Enter a valid option");
            }
        }

        private static void SendEmail(string address)
        {
            // var text = "Hi, there is an Escalation, Kindly look in to it asap";
        }

        private static void SendSms(string phoneNumber)
        {
            const string username = "wanj";
            const string key = "20d64532cb1ec70864141b9f64f02dc9fa1b1a4701edd856b3af791ef4cdb003";
            const string url = "https://api.africastalking.com/version1/messaging";

            const string message = "Hello Test Escalation";

            var formDictionary = new Dictionary<string, string>
            {
                {"username", username}, {"to", phoneNumber}, {"message", message}
            };

            var formContent = new FormUrlEncodedContent(formDictionary);

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Add("apiKey", key);
            request.Headers.Add("Accept", "application/json");

            request.Content = formContent;

            using var client = new HttpClient();

            var response = client.SendAsync(request);
        }

        private void CallBack(DateTime dateTime)
        {
            EndCall();
            Console.WriteLine(
                "\n\n\n<<===================================== NEW CALL =====================================>>\n\n\n");
            Call();
        }

        private static void NotInterested()
        {
            EndCall();
        }

        private static void EndCall()
        {
            AskQuestion(14);
        }
    }
}