using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wordCountConsole
{
    class Program
    {
        public void CheckFrequency (string filename, Dictionary<string, int> wordsIncluded)
        {
            var fileContent = File.ReadAllText(filename);
            var wordPattern = new Regex(@"\w+");
            List<string> keysToRemove = new List<string> { "a", "an", "or", "the",
            "and", "I", "you", "he", "she", "it", "we" , "they"};
            foreach (Match match in wordPattern.Matches(fileContent))
            {
                int counter = 0;
                wordsIncluded.TryGetValue(match.Value, out counter);
                counter++;
                wordsIncluded[match.Value] = counter;
            }
            foreach (var key in keysToRemove)
                wordsIncluded.Remove(key);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Write wordcount [options] <input file path> , and then press Enter for frequency report");
        }
    }
}
