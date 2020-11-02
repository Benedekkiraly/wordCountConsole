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
            void CheckFrequency(string filename, Dictionary<string, int> dictionary, string outputPath = "", List<string> keysToRemove = null)
            {
                var fileContent = File.ReadAllText(filename);
                var wordPattern = new Regex(@"\w+");
                if (keysToRemove == null)
                {
                 keysToRemove = new List<string> { "a", "an", "or", "the",
                 "and", "I", "you", "he", "she", "it", "we" , "they"};
                }


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
                    string result = string.Join(",", inputs);
                    Console.WriteLine($"Inputs: {result}");
                    string outputPath = inputs.ElementAt(1);
                    inputs.Remove(inputs.ElementAt(1));

                    foreach (string i in inputs)
                    {
                        var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                        CheckFrequency(inputs.ElementAt(2),wordsIncluded, outputPath);
                        foreach (KeyValuePair<string, int> kvp in wordsIncluded.OrderByDescending(key => key.Value))
                        {
                            Console.WriteLine(kvp.Key + " " + kvp.Value.ToString());
                        }

                    }
                }
                //lexical order
                if (inputs.Contains("-l") || inputs.Contains("--lex"))
                {
                    inputs.Remove("-l");
                    inputs.Remove("--lex");
                    string result = string.Join(",", inputs);
                    Console.WriteLine($"Inputs: {result}");
                    foreach (string i in inputs)
                    {
                        var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                        CheckFrequency(i, wordsIncluded);
                        foreach (KeyValuePair<string, int> kvp in wordsIncluded.OrderBy(i => i.Key))
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
                    string result = string.Join(",", inputs);
                    Console.WriteLine($"Inputs: {result}");
                    var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                    foreach (string i in inputs)
                    {
                        CheckFrequency(i, wordsIncluded);
                    }
                    foreach (KeyValuePair<string, int> kvp in wordsIncluded.OrderByDescending(key => key.Value))
                    {
                        Console.WriteLine(kvp.Key + " " + kvp.Value.ToString());
                    }

                }
                //frequencies of words with ignored words as input
                if (inputs.Contains("-i") || inputs.Contains("--ignore"))
                {
                    inputs.Remove("-i");
                    inputs.Remove("--ignore");
                    string result = string.Join(",", inputs);
                    Console.WriteLine($"Inputs: {result}");
                    var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                    List<string> ignoredWords = File.ReadAllLines(inputs.ElementAt(0)).ToList();
                    List<string> ignoredList = ignoredWords.ElementAt(0).Split(" ").ToList();
                    inputs.Remove(inputs.ElementAt(0));
                    foreach (string i in inputs)
                    {
                        CheckFrequency(i, wordsIncluded,"",ignoredList);
                    }  
                    foreach (KeyValuePair<string, int> kvp in wordsIncluded.OrderByDescending(key => key.Value))
                    {
                        Console.WriteLine(kvp.Key + " " + kvp.Value.ToString());
                    }

                }
                //running without options
                else
                {
                    string result = string.Join(",", inputs);
                    Console.WriteLine($"Inputs: {result}");
                    foreach (string i in inputs)
                    {
                        var wordsIncluded = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                        CheckFrequency(i, wordsIncluded);
                        foreach (KeyValuePair<string, int> kvp in wordsIncluded.OrderByDescending(key => key.Value))
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
        }
    }
}
//missing: , exceptions, restarting, lexical order gives double output, testing,
