using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wordCountConsole
{
    class Program
    {
 
        static void Main(string[] args)
        {
            void CheckFrequency(string filename, Dictionary<string, int> dictionary, string outputPath = "")
            {
                var fileContent = File.ReadAllText(filename);
                var wordPattern = new Regex(@"\w+");
                List<string> keysToRemove = new List<string> { "a", "an", "or", "the",
                 "and", "I", "you", "he", "she", "it", "we" , "they"};
                foreach (Match match in wordPattern.Matches(fileContent))
                {
                    int counter = 0;
                    dictionary.TryGetValue(match.Value, out counter);
                    counter++;
                    dictionary[match.Value] = counter;
                }
                foreach (var key in keysToRemove)
                    dictionary.Remove(key);

                if (outputPath != "")
                {
                    using (StreamWriter file = new StreamWriter(outputPath))
                        foreach (var entry in dictionary)
                            file.WriteLine("[{0} {1}]", entry.Key, entry.Value);
                }

            }

            Console.WriteLine("Write wordcount [options] <input file path> , and then press Enter for frequency report");
            string line = Console.ReadLine();
            if (line.StartsWith("wordcount"))
            {
                string command = line.Substring(10);
                List<string> inputs = command.Split(" ").ToList();
                //saving to output file command
                if (inputs.Contains("-o") || inputs.Contains("--output"))
                {
                    inputs.Remove("-o");
                    inputs.Remove("--output");
                    string outputPath = inputs.ElementAt(1);
                    inputs.Remove(inputs.ElementAt(1));

                    foreach (string i in inputs)
                    {
                        var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                        CheckFrequency(inputs.ElementAt(2),wordsIncluded, outputPath);
                        foreach (KeyValuePair<string, int> kvp in wordsIncluded)
                        {
                            Console.WriteLine(kvp.Key + " " + kvp.Value.ToString());
                        }

                    }
                }
                //merge input file word frequencies
                if (inputs.Contains("-m") || inputs.Contains("--merge"))
                {
                    inputs.Remove("-m");
                    inputs.Remove("--merge");
                    var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                    foreach (string i in inputs)
                    {
                        CheckFrequency(i, wordsIncluded);
                    }
                    foreach (KeyValuePair<string, int> kvp in wordsIncluded)
                    {
                        Console.WriteLine(kvp.Key + " " + kvp.Value.ToString());
                    }

                }
                //running without options
                else
                {
                    foreach (string i in inputs)
                    {
                        var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                        CheckFrequency(i, wordsIncluded);
                        foreach (KeyValuePair<string, int> kvp in wordsIncluded)
                        {
                            Console.WriteLine(kvp.Key + " " + kvp.Value.ToString());
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid command!");
                
            }
            //switch (Console.ReadLine())
            //{
            //    case
            //        Console.WriteLine("asd");
            //        break;
            //}
        }
    }
}
