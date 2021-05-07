using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExamPracticing
{
    class Program
    {

        public struct IceSkate
        {
            public string Name;
            public string CountryCode;
            public double Points;
            public double ComponentPoints;
            public int ErrorPoint;

            public double OverallPoints;
        }

        static List<IceSkate> ShortSoft = new List<IceSkate>();
        static List<IceSkate> Finals = new List<IceSkate>();

        static void Main(string[] args)
        {
            StreamReader ReadShort = new StreamReader("rovidprogram.txt",Encoding.UTF8);
            string Row = ReadShort.ReadLine();
            while (!ReadShort.EndOfStream)
            {
                Row = ReadShort.ReadLine();
                string[] Split = Row.Split(';');
                IceSkate Temp = new IceSkate();
                Temp.Name = Split[0];
                Temp.CountryCode = Split[1];
                Temp.Points = Convert.ToDouble(Split[2].Replace('.', ','));
                Temp.ComponentPoints = Convert.ToDouble(Split[3].Replace('.', ','));
                Temp.ErrorPoint = Convert.ToInt32(Split[4]);
                ShortSoft.Add(Temp);
            }
            ReadShort.Close();

            StreamReader ReadFinals = new StreamReader("donto.txt", Encoding.UTF8);
            Row = ReadFinals.ReadLine();
            while (!ReadFinals.EndOfStream)
            {
                Row = ReadFinals.ReadLine();
                string[] Split = Row.Split(';');
                IceSkate Temp = new IceSkate();
                Temp.Name = Split[0];
                Temp.CountryCode = Split[1];
                Temp.Points = Convert.ToDouble(Split[2].Replace('.', ','));
                Temp.ComponentPoints = Convert.ToDouble(Split[3].Replace('.', ','));
                Temp.ErrorPoint = Convert.ToInt32(Split[4]);
                Finals.Add(Temp);
            }
            ReadFinals.Close();

            Console.WriteLine("A rövidprogramban indult versenyzők száma:"+ ShortSoft.Count);
            bool InIt = false;
            int Index = 0;
            while (!InIt && Index < Finals.Count)
            {
                if (Finals[Index].CountryCode == "HUN")
                {
                    InIt = true;
                }
                else
                {
                    Index++;
                }
            }
            if (InIt)
            {
                Console.WriteLine("Van Magyar versenyző a döntőben");
            }
            else
            {
                Console.WriteLine("Nincs Magyar versenyző a döntőben");
            }

            Console.WriteLine("Egy nevet kérek:");
            string names = Console.ReadLine();

            double OvrPoints = OverallPoints(names);
            if (OvrPoints == 0)
            {
                Console.WriteLine("Nem volt ilyen versenyző");
            }
            else
            {
                Console.WriteLine("Az összepontszáma a versenyzőnek: "+OvrPoints);
            }
            List<string> GotInCountry = new List<string>();

            foreach (IceSkate contestant in Finals)
            {
                if (!GotInCountry.Contains(contestant.CountryCode))
                {
                    GotInCountry.Add(contestant.CountryCode);
                }
            }
            foreach(string Country in GotInCountry)
            {
                int count = 0;
                foreach  (IceSkate contestant in Finals)
                {
                    if (contestant.CountryCode == Country)
                    {
                        count++;
                    }
                }
                if (count > 1)
                {
                    Console.WriteLine(Country + ":" + count);
                }
            }
            FileStream fs3 = new FileStream("vegeredmeny.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs3);

            for (int i = 0; i < Finals.Count; i++)
            {
                IceSkate newSkate = Finals[i];
                newSkate.OverallPoints = OverallPoints(Finals[i].Name);
                Finals[i] = newSkate;
            }
            Finals = Finals.OrderBy(item => item.OverallPoints).ToList();
            Finals.Reverse();
            int reached = 1;
            foreach (IceSkate contestant in Finals)
            {
                sw.WriteLine(reached + ". " + contestant.Name + ";" + contestant.CountryCode + ";" + contestant.OverallPoints);
                reached++;
            }
            sw.Close();
            fs3.Close();
        }

        static double OverallPoints(string Name)
        {
            double OvrPoints = 0;
            foreach (IceSkate item in ShortSoft)
            {
                if (item.Name == Name)
                { 
                    OvrPoints += item.Points + item.ComponentPoints - item.ErrorPoint;
                }
            }
            foreach (IceSkate item in Finals)
            {
                if (item.Name == Name)
                {
                    OvrPoints += item.Points + item.ComponentPoints - item.ErrorPoint;
                }
            }
            return OvrPoints;
        }
    }
}
