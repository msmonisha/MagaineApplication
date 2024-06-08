using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineStore
{
    public class Magazines
    {
        public List<MagazineData> data { get; set; }
        public bool success { get; set; }

        public string token { get; set; }

        public string message { get; set; }
    }

    public class Category
    {

        public List<string> data { get; set; }
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
    }

    public class Subscribers
    {
        public List<SubscriberData> data { get; set; }
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
    }

    public class MagazineData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
    }

    public class SubscriberData
    {
        public string id { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public int[] magazineIds { get; set; }

    }

    public class AnswerData
    {
        public string totalTime { get; set; }

        public bool answerCorrect { get; set; }

        public List<string> shouldBe { get; set; }

    }

    public class Answer
    {
        public AnswerData data { get; set; }
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
    }
}
