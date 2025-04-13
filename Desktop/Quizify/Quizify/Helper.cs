using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quizify
{
    static class Helper
    {
        public static User user = new User();
        public static QuestionModel model = new QuestionModel();
        public static List<QuestionModel> models = new List<QuestionModel>();
        public static string basePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName.Replace(@"\bin", @"\resource\");
        public static Regex rgEmail = new Regex("^[a-zA-z0-9].+[a-zA-z0-9]+.{5,10}$");

        public static bool CheckIsEmpty(this Control control)
        {
            bool b1 = control.Controls.OfType<TextBox>().Any(a => string.IsNullOrWhiteSpace(a.Text));
            return b1;
        }

        public static string GetImage(this string image)
        {
            FileInfo[] f = new DirectoryInfo(basePath).GetFiles("*" + image + "*.*");
            if (f.Length > 0) 
                return f[0].FullName;
            return null;
        }

        public static int ToInt32(this object o)
        {
            return Convert.ToInt32(o);
        }

        public static void ShowMessage(this string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowMessageError(this string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public class QuestionModel
        {
            public int ID { get; set; }
            public int No { get; set; }
            public string Question { get; set; }
            public string OptionA { get; set; }
            public string OptionB { get; set; }
            public string OptionC { get; set; }
            public string OptionD { get; set; }
            public string Answer { get; set; }
        }
    }
}
