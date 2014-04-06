using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace WindowsFormsApplication1
{
    public partial class le_saviez_vous : Form
    {
        private string extraireArticle(string src)
        {
            //Encodage UTF-8
            byte[] bytes = Encoding.Default.GetBytes(src);
            src = Encoding.UTF8.GetString(bytes);

            //Extraction de l'article
            Regex r = new Regex("(?:<div id=\"mf-lumieresur\" title=\"Lumière sur\">)(((?<!</li>)(.|\n|))*)", RegexOptions.Multiline);
            Match m = r.Match(src);

            return m.ToString();
        }
        private string formatagetexte(string t)
        {
            //Retrait Balises HTML
            t = Regex.Replace(t, "<[^>]*>", String.Empty);

            //Retrait Caractère spécifique au HTML
            t = Regex.Replace(t, "&#160;", " ");

            //Retrait des retours à la ligne ou espaces en plusieurs exemplaires
            t = Regex.Replace(t, "[\f\n\r\t\v]{2,}", String.Empty);

            //Retrait texte : "Lire la suite"
            t = Regex.Replace(t, "Lire la suite", String.Empty);

            return t;
        }

        private string obtenirURL(string src)
        {
            string u = src;//variable renvoyée
            string test = src;
            string s = "/wiki/[^\n]*\\\" title=\\\"[^\n]*\\\">Lire la suite";//Regex Bloc du dernier lien
            string s2 = "/wiki/[^\n]*\\\" t";//Regex extraction du lien pur
            Regex r = new Regex(s);
            Regex r2 = new Regex(s2);
            Match m = r.Match(u);
            Match m2 = r2.Match(m.ToString());
            
            u = m2.ToString();
            u = u.Remove(u.Length - 3);
            u = "http://fr.wikipedia.org" + u;

            return u;
        }

        string url = "";

        public le_saviez_vous()
        {
            InitializeComponent();
            WebClient w = new WebClient();
            String texte = w.DownloadString("http://fr.wikipedia.org/wiki/Wikip%C3%A9dia:Accueil_principal");
            texte = extraireArticle(texte);
            url = obtenirURL(texte);
            richTextBox1.Text = formatagetexte(texte);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = this.url;
            p.Start();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() != DialogResult.Cancel) ;
            {
                richTextBox1.Font = fontDialog1.Font;
            }

        }
    }
}
