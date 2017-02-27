using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLetterGame
{
    class Program
    {
        private const ConsoleColor InfoColor = ConsoleColor.Blue;
        private const ConsoleColor InputColor = ConsoleColor.White;
        private const ConsoleColor ErrorColor = ConsoleColor.Red;
        private const ConsoleColor ResultColor = ConsoleColor.Green;

        /// <summary>
        /// A dictionary to hold the word-to-usage scenarios
        /// </summary>
        private static List<string> _words;

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.ForegroundColor = InfoColor;

            // Load words from a local copy of https://github.com/dwyl/english-words
            Console.WriteLine("Loading dictionary file.");

            // Create and start stopwatch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Load the file
            if (!LoadWords("words.tsv"))
            {
                Console.WriteLine("Word load failure. Terminating.");
                return;
            }

            // End stopwatch
            stopwatch.Stop();

            Console.WriteLine($"Loaded {_words.Count} words in {stopwatch.Elapsed}.");
            Console.WriteLine($"Write '.' to quit.");

            // Keep a record of whether or not we're pruning words
            var pruneStartWithRealWord = true;

            while (true)
            {
                Console.ForegroundColor = InputColor;
                Console.Write("> ");

                // Attempt to wait for and read the user's input
                var readLine = Console.ReadLine();

                // Catostrophic failure
                if (readLine == null)
                {
                    Console.ForegroundColor = ErrorColor;
                    Console.WriteLine("Input null. Terminating.");
                    break;
                }

                var input = readLine.Trim();

                // Check for termination
                if (input == ".")
                {
                    Console.ForegroundColor = ErrorColor;
                    Console.WriteLine("Bye.");
                    break;
                }
                
                // Check for reserved words
                switch (input)
                {
                    case "~p":
                        // Prune enable command
                        pruneStartWithRealWord = true;
                        Console.ForegroundColor = InfoColor;
                        Console.WriteLine("Enabled pruning of words that start with other english words.");
                        continue;
                    case "~np":
                        // Prune disable command
                        pruneStartWithRealWord = false;
                        Console.ForegroundColor = InfoColor;
                        Console.WriteLine("Disabled pruning of words that start with other english words.");
                        continue;
                    default:
                        // Skip if the user's input is less than 3 characters because of the search times
                        if (input.Length < 3)
                        {
                            Console.ForegroundColor = ErrorColor;
                            Console.WriteLine("Enter 3 or more characters.");
                            continue;
                        }
                        // This is where the magic happens
                        PrintBestMatches(readLine, pruneStartWithRealWord);
                        break;
                }
            }
        }

        /// <summary>
        /// Finds and prints the best matches for the given word
        /// </summary>
        /// <param name="word">The word fragment to compare against</param>
        /// <param name="prune">True to prune words that start with other similar english words</param>
        private static void PrintBestMatches(string word, bool prune)
        {
            // Sort out which words begin with the given fragment and are over 3 letters long (because pruning is greedy)
            var foundWords = new List<string>(_words.Where(pair => pair.StartsWith(word) && pair.Length > 3));

            // Order by length -- the longer the word, the better for the game, after all!
            foundWords = new List<string>(foundWords.OrderByDescending(pair => pair.Length));

            // Remove words that start with other similar english words only if told to
            if (prune)
            {
                // remove words that start with other real words
                var remove = new List<string>();

                foreach (var foundWord in foundWords)
                {
                    // Find all words that are not the needle that start with the needle
                    var any = foundWords.Where(s => s.StartsWith(foundWord) && s != foundWord);

                    // add all the found words to the remove-queue
                    remove.AddRange(any);
                }

                // Remove all in the remove-queue
                foreach (var remover in remove)
                    foundWords.Remove(remover);

                Console.ForegroundColor = InfoColor;
                Console.WriteLine($"~ Pruned {remove.Count} words that start with words similar to {word}.");
            }

            Console.ForegroundColor = ResultColor;

            // Print them out for the user
            foreach (var foundWord in foundWords)
                Console.WriteLine(foundWord);
        }

        /// <summary>
        /// Parse the tab-seperated value file of words
        /// </summary>
        /// <param name="filename">The file name to load</param>
        /// <returns>True if the word dict loaded correctly, false otherwise</returns>
        private static bool LoadWords(string filename)
        {
            // Validate file
            if (!File.Exists(filename))
            {
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine("Unable to find words file.");
                return false;
            }

            // Init the words list
            _words = new List<string>();

            // Load the file into memory
            using (var sr = new StreamReader(filename))
            {
                // Read until the end fo the file
                while (!sr.EndOfStream)
                {
                    // Grab the current line
                    var currentLine = sr.ReadLine();

                    // End of stream or catostrophic failure
                    if (currentLine == null) return false;

                    // Insert into dict
                    _words.Add(currentLine);
                }
            }
            return true;
        }
    }
}
