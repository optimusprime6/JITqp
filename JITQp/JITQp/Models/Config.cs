using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JITQp.Models
{

    public class Config
    {
        public Questionpaper[] questionPaper { get; set; }
    }

    public class Questionpaper
    {
        public int sem { get; set; }
        public Pattern[] pattern { get; set; }
    }

    public class Pattern
    {
        public int numberOfQuestions { get; set; }
        public int marksPerQuestion { get; set; }
    }

}
