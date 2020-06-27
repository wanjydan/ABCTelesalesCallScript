using System.Collections.Generic;

namespace ABCTelesalesCallScript
{
    public class Core
    {
        public static readonly List<(string, QuestionType)> Questions = new List<(string, QuestionType)>()
        {
            ("Hello, Good Morning/afternoon/evening. My Name is Cynthia Calling you from ABC, could I speak to Mr./Mrs/Ms./Dr./Prof.",
                QuestionType.YesNo),
            ("Can I talk to Mr./Mrs/Ms./Dr./Prof. at some other date and time?", QuestionType.YesNo),
            ("Its about ABC and the benefits your company can acquire from us working together. Which is offering a solution that will significantly reduce your IT infrastructure costs while ensuring reliability and enhanced security through the services we offer. Are you interested?",
                QuestionType.YesNo),
            ("Thank you Charles Mwasambu, Have you heard about ABC? ", QuestionType.YesNo),
            ("What's your take on these ABC Services?", QuestionType.Review),
            ("So I believe securing your business information is extremely critical. How do you back up your business data?",
                QuestionType.System),
            ("With that (Personalize client name), you do agree that your business data is extremely important for your business & needs back up?",
                QuestionType.AgreeDisagree),
            ("Ok. And you do agree that ensuring that, it’s backed up automatically in a secure, off site location gives you a piece of mind that your business info is safe?",
                QuestionType.AgreeDisagree),
            ("You also agree that that our service is very affordable, right?", QuestionType.AgreeDisagree),
            ("Fantastic. If you’d so kind as to giving us an appropriate time in the week one of our business executives will come over to discuss the solution in detail.",
                QuestionType.YesNo),
            ("Kindly assist us with your email address to sent an appointment invite whereby one of our representatives will visit you and take you through the process of registration / set up and get you ready for the services.",
                QuestionType.Void),
            ("Is there any question you would like me to address with regards to our Services / or any clarification on the information I have given you?",
                QuestionType.YesNo),
            ("Is this call an escalation?", QuestionType.YesNo),
            ("Well Charles Mwasambu, thank you for your time, it's been a pleasure talking to you.\n If you have any further clarifications , feel free to contact our website www.abc.co or phone number 020 – 5230028.\n\nHave a good day. Good bye. ",
                QuestionType.Void),
            ("Shall we Continue?", QuestionType.YesNo),
            ("ABC is a backup service provider that offers you affordable & secure backup solutions. It also offers leased server services so customers don’t have to invest in physical servers in their offices which are managed by ABC, 24/ 7 ",
                QuestionType.Void)
        };
    }
}