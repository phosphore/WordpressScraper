using System;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Web;

namespace WPScraping
{
    class Program
    {
        public struct article
        {
            public string name;
            public string address;
        }

        public static article tmp;

        static void Main(string[] args)
        {
            Console.WriteLine(" WordPress scraper v0.2");
            Console.WriteLine("       Beep beep!");
            Console.WriteLine("phosphore - DWTFYW license\n");
            Console.WriteLine("What should I do?");
            Console.WriteLine("1 - New scan");
            Console.WriteLine("2 - Continue previous scan");
            int a = Convert.ToInt32(Console.ReadLine());

            if (a == 1) ////////////////////////////////////////// 1st Option ////////////////////////////////////
            {
                Console.WriteLine("\nYou'll find your dump in C:\\cineblog01_titles.txt\nPress a key to start...");
                Console.ReadKey();
                //Create txt file
                string[] initiallines = { "Title dump from cineblog01.tv", "[1510 pages]" };
                try
                {
                    System.IO.File.WriteAllLines(@"C:\cineblog01_titles.txt", initiallines);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an error creating the file. Check it. Exception: \n" + ex.Message);
                }

                Console.WriteLine("\nFile created\n");

                spider(1);
            }
            else if (a == 2) ////////////////////////////////////////// 2nd Option ////////////////////////////////////
            {
                
                Console.WriteLine("\n page number?");
                int pgnum = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\nYou'll find your dump in C:\\cineblog01_titles.txt\nPress a key to start...");
                Console.ReadKey();
                spider(pgnum);
            }
            else
            {
                Console.WriteLine("Type just 1 or 2");
                Console.ReadKey();
            }

        }




        private static void drawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " di " + total.ToString() + "    - Extracting..."); //blanks at the end remove any excess
        }

       
        private static void spider(int pgnum)
        {
            //Get film titles and the url of the article
            for (int i = pgnum; i <= 1510; i++)
            {
                Console.Clear();
                drawTextProgressBar(i, 1510);
                Console.WriteLine();
                WebClient c = new WebClient();
                c.Encoding = System.Text.Encoding.UTF8;
                string html = "";
                try
                {
                    html = c.DownloadString("http://www.cineblog01.tv/page/" + i + "/");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errore di connessione.");
                    Console.WriteLine("Stacktrace: " + ex.Message);
                    Console.ReadKey();
                }
                var doc = new HtmlDocument();
                doc.LoadHtml(html);



                //HtmlNode specificNode = doc.GetElementbyId("post-title");
                HtmlNodeCollection nodesMatchingXPath = doc.DocumentNode.SelectNodes("//*[@id=\"post-title\"]");

                Console.Write("... ");
                foreach (var item in nodesMatchingXPath)
                {
                    tmp.name = WebUtility.HtmlDecode(item.FirstChild.InnerText);
                    tmp.address = item.FirstChild.Attributes["href"].Value;
                    //Writes it in a txt file, appending text

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\cineblog01_titles.txt", true))
                    {
                        file.WriteLine(tmp.name + "\t" + tmp.address);
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" --> ");
                Console.ResetColor();
                Console.WriteLine("Extracted!");
            }

        }

    }
}
