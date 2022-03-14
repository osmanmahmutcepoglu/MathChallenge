using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MathChallenge
{
    internal class Program
    {
        class Polynom
        {
            public int coefficient { get; set; }
            public int power { get; set; }
            public Polynom(int coefficient, int power)
            {
                this.coefficient = coefficient;
                this.power = power;
            }

        }
        static void Main(string[] args)
        {
            Console.WriteLine(MathChallenge("(-1x^3)(3x^3+2)"));
        }
        public static string MathChallenge(string str)
        {

            string letter = Regex.Match(str, @"[A-Za-z]").Value[0].ToString();

            string modifiedStr = Regex.Replace(str, @"[a-zA-Z]", "x");
            modifiedStr = Regex.Replace(modifiedStr, @"-", "+-");
            modifiedStr = Regex.Replace(modifiedStr, @"\^\+\-", "^-");

            List<string> termArray = modifiedStr.Split(')').ToList();

            termArray.RemoveAt(termArray.Count() - 1);


            string newTerm = "";
            string[] newTermArray;
            List<List<Polynom>> polynomList = new List<List<Polynom>>();

            foreach (string term in termArray)
            {
                newTerm = Regex.Replace(term, @"\(", "");
                newTerm = Regex.Replace(newTerm, @"\d+x(?!\^)", "$&^1");
                newTerm = Regex.Replace(newTerm, @"\+\-?\d+(?!x)", "$&x^0");
                newTerm = Regex.Replace(newTerm, @"^\d+$", "$&x^0");
                newTermArray = newTerm.Split('+');
                List<Polynom> result = new List<Polynom>();
                foreach (var item in newTermArray)
                {
                    if (item != "")
                    {
                        Match parts = Regex.Match(item, @"^(\-?\d+)x\^(\-?\d+)$");
                        result.Add(new Polynom(Convert.ToInt32(parts.Groups[1].Value), Convert.ToInt32(parts.Groups[2].Value)));
                    }
                }
                polynomList.Add(result);

            }

            List<Polynom> solutionPoliynom = new List<Polynom>();

            for (int i = 1; i < polynomList.Count(); i++)
            {
                solutionPoliynom = polyMultiply(polynomList[i - 1], polynomList[i]);
            }

            solutionPoliynom = solutionPoliynom.OrderByDescending(o => o.power).ToList();



            List<Polynom> newSolutionPoliynom = new List<Polynom>();

            for (int i = 0; i < solutionPoliynom.Count() - 1; i++)
            {
                if (solutionPoliynom[i].power != solutionPoliynom[i + 1].power)
                {
                    newSolutionPoliynom.Add(solutionPoliynom[i]);
                }
                else
                {
                    solutionPoliynom[i + 1].coefficient = solutionPoliynom[i].coefficient + solutionPoliynom[i + 1].coefficient;
                }
            }
            newSolutionPoliynom.Add(solutionPoliynom[solutionPoliynom.Count() - 1]);
            solutionPoliynom.RemoveAt(solutionPoliynom.Count() - 1);

            string newString = "";

            foreach (var poly in newSolutionPoliynom)
            {
                if (poly.power != 1 && poly.power != 0)
                {
                    newString += "+" + poly.coefficient.ToString() + letter + "^" + poly.power.ToString();
                }
                else if (poly.power == 1)
                {
                    newString += "+" + poly.coefficient.ToString() + letter;
                }
                else
                {
                    newString += "+" + poly.coefficient.ToString();
                }
            }


            string formattedString = Regex.Replace(newString, @"\+\-", "-");
            formattedString = Regex.Replace(formattedString, @"^\+", "");
            formattedString = Regex.Replace(formattedString, @"([-\+])1([a-zA-Z])", "$1$2");
            formattedString = Regex.Replace(formattedString, @"^1([a-zA-Z])", "$1");

            return formattedString;
        }
        static List<Polynom> polyMultiply(List<Polynom> polynom1, List<Polynom> polynom2)
        {
            List<Polynom> result = new List<Polynom>();
            foreach (Polynom pol1 in polynom1)
            {
                foreach (Polynom pol2 in polynom2)
                {
                    result.Add(TermMultiply(pol1, pol2));
                }
            }
            return result;
        }
        static Polynom TermMultiply(Polynom polynom1, Polynom polynom2)
        {
            return new Polynom(polynom1.coefficient * polynom2.coefficient, polynom1.power + polynom2.power);
        }
    }
}
