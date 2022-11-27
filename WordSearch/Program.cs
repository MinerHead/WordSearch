using System;
using System.Collections.Generic;
using System.Data;

namespace WordSearch
{
    class Program
    {
        static bool ShowNotFound = false; // whether or not words that cannot be found will be displayed in the result message
        static char[,] Grid = new char[,] {
            {'C', 'P', 'K', 'X', 'O', 'I', 'G', 'H', 'S', 'F', 'C', 'H'},
            {'Y', 'G', 'W', 'R', 'I', 'A', 'H', 'C', 'Q', 'R', 'X', 'K'},
            {'M', 'A', 'X', 'I', 'M', 'I', 'Z', 'A', 'T', 'I', 'O', 'N'},
            {'E', 'T', 'W', 'Z', 'N', 'L', 'W', 'G', 'E', 'D', 'Y', 'W'},
            {'M', 'C', 'L', 'E', 'L', 'D', 'N', 'V', 'L', 'G', 'P', 'T'},
            {'O', 'J', 'A', 'A', 'V', 'I', 'O', 'T', 'E', 'E', 'P', 'X'},
            {'C', 'D', 'B', 'P', 'H', 'I', 'A', 'W', 'V', 'X', 'U', 'I'},
            {'L', 'G', 'O', 'S', 'S', 'B', 'R', 'Q', 'I', 'A', 'P', 'K'},
            {'E', 'O', 'I', 'G', 'L', 'P', 'S', 'D', 'S', 'F', 'W', 'P'},
            {'W', 'F', 'K', 'E', 'G', 'O', 'L', 'F', 'I', 'F', 'R', 'S'},
            {'O', 'T', 'R', 'U', 'O', 'C', 'D', 'O', 'O', 'F', 'T', 'P'},
            {'C', 'A', 'R', 'P', 'E', 'T', 'R', 'W', 'N', 'G', 'V', 'Z'}
        };

        static string[] Words = new string[] 
        {
            "",
            "d",
            "CA",
            "CAR",
            "CARR",
            "'",
            "CARPET",
            "CHAIR",
            "DOG",
            "BALL",
            "DRIVEWAY",
            "FISHING",
            "FOODCOURT",
            "FRIDGE",
            "GOLF",
            "MAXIMIZATION",
            "PUPPY",
            "SPACE",
            "TABLE",
            "TELEVISION",
            "WELCOME",
            "WINDOW",
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Word Search");

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    Console.Write(Grid[y, x]);
                    Console.Write(' ');
                }
                Console.WriteLine("");

            }

            Console.WriteLine("");
            Console.WriteLine("Found Words");
            Console.WriteLine("------------------------------");

            FindWords();

            Console.WriteLine("------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }

        private static void FindWords()
        {
            //Find each of the words in the grid, outputting the start and end location of
            //each word, e.g.
            //PUPPY found at (10,7) to (10, 3) 
            var dict = GenerateCharDictionary();
            foreach (var originalWord in Words)
            {
                if (string.IsNullOrWhiteSpace(originalWord))
                {// empty or null word string
                    DisplayMsgResult(null);
                    continue;
                }
                else if (originalWord.Length == 1)
                {// single char word string
                    if (!dict.ContainsKey(Char.ToUpper(originalWord[0])))
                    {// char is not in the grid
                        DisplayMsgResult(originalWord.ToUpper());
                        continue;
                    }
                    foreach (var coordOne in dict[Char.ToUpper(originalWord[0])])
                    {
                        DisplayMsgResult(originalWord.ToUpper(), coordOne, coordOne);
                    }
                    continue;
                }

                // word string with more than 1 char
                bool found = false;
                var word = originalWord.ToUpper();
                if (!dict.ContainsKey(word[0]) || !dict.ContainsKey(word[1]))
                {// first or second char is not in the grid
                    DisplayMsgResult(word);
                    continue;
                }
                foreach (var coordOne in dict[word[0]])
                {// first point
                    foreach (var coordTwo in dict[word[1]])
                    {// second point
                        Coordinate direction;
                        if (Coordinate.IsValidDistance(coordOne, coordTwo, out direction))
                        {// valid starting point and directiion
                            if (word.Length == 2)
                            {// word string with 2 char
                                DisplayMsgResult(word, coordOne, coordTwo);
                                found = true;
                            }
                            else
                            {// more than 2 char
                                var coordCurrent = coordTwo;
                                for (int i = 2; i < word.Length; i++)
                                {
                                    if (!dict.ContainsKey(word[i]))
                                    {// char is not in the grid
                                        DisplayMsgResult(word);
                                        break;
                                    }
                                    var coordNext = coordCurrent.GetNext(direction);
                                    if (dict[word[i]].Contains(coordNext))
                                    {
                                        if (i == word.Length - 1)
                                        {// reached last char in the word
                                            DisplayMsgResult(word, coordOne, coordNext);
                                            found = true;
                                        }
                                        else
                                        {// not last char, move to next
                                            coordCurrent = coordNext;
                                        }
                                    }
                                    else
                                    {// cannot find valid char for this starting point
                                        break;
                                    }
                                }
                            }
                            
                        }
                    }
                }
                if (!found)
                {// no matching word can be found in the grid
                    DisplayMsgResult(word);
                }
            }
        }

        private static Dictionary<char, List<Coordinate>> GenerateCharDictionary()
        {
            // Generate a dictionary containing coordinates for each unique Char in the grid
            var dict = new Dictionary<char, List<Coordinate>>();
            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    dict.TryAdd(Char.ToUpper(Grid[y, x]), new List<Coordinate>());
                    dict[Char.ToUpper(Grid[y, x])].Add(new Coordinate(x, y));
                }
            }

            return dict;
        }

        private static void DisplayMsgResult(string word, Coordinate? start = null, Coordinate? end = null)
        {
            if (string.IsNullOrWhiteSpace(word))
            {// invalid word input
                if (ShowNotFound)
                {
                    Console.WriteLine($"Invalid word as it cannot be empty");
                }
            }
            else if (start.HasValue && end.HasValue)
            {
                Console.WriteLine($"{word} found at ({start.Value.X}, {start.Value.Y}) to ({end.Value.X}, {end.Value.Y})");
            }
            else if (ShowNotFound)
            {
                Console.WriteLine($"{word} cannot be found");
            }
            
        }
    }
}
