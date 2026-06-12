using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;

namespace WordleHelperCS
{
    class Program
    {

        static void Main(string[] args)
        {
            int count = 0;
            string charCounts = "starel";
            string charsToEliminate = "";
            //TODO - todo what? Why lol
            string[] yellow = new string[5] { "     ", "     ", "     ", "     ", "     " };
            string[] greens = new string[1] { "     " };

            Dictionary<char, int[]> charCountResults = new Dictionary<char, int[]>();
            Dictionary<char, int[]> charCountAll = new Dictionary<char, int[]>();
            Dictionary<string, List<string>> containsKey = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> containsKey2 = new Dictionary<string, List<string>>();
            //List<string> possibleWords = AllWords.Where(w => w.EndsWith('s')).ToList();
            List<string> prevWords = GetUsedWords(1);
            List<string> possibleWords = WordLists.Words.Where(w => w.IndexOfAny(charsToEliminate.ToCharArray()) == -1).OrderBy(c => c[0]).ToList();
            //possibleWords = possibleWords.Where(w => w.Count(c => c == 'r') == 1).ToList();
            List<string> notExcludedWords = new List<string>();
            //possibleWords = possibleWords.Where(w => w.Substring(1, 2) == "ar").ToList();
            //possibleWords = possibleWords.Where(w => w.ToCharArray()[3] == 'r').ToList();
            //return;

            string byg1 = "";
            int byg1count = 1;
            List<string> byg1WordList = WordLists.Words;
            string byg2 = "";
            int byg2count = 1;
            List<string> byg2WordList = WordLists.Words;//.Where(w => !prevWords.Contains(w)).ToList();
            string byg3 = "";
            int byg3count = 1;
            List<string> byg3WordList = WordLists.Words;

            foreach (string word in yellow)
            {
                foreach (char letter in word)
                {
                    if (letter != ' ' && !charCounts.Contains(letter))
                    {
                        charCounts += letter;
                    }
                }
            }

            foreach (var word in possibleWords)
            {
                //Console.WriteLine(word);
                //count++;
                //if (word.Substring(1,3) == "ora")// && Words.Where(w => w != word && word.Substring(2,2) == w.Substring(2,2)).Count() >= 2 && Words.Any(w => w != word && w.Substring(3, 2) == word.Substring(3, 2) && word[0] != w[0] && word.Contains(w[0])) && Words.Any(w => w != word && !word.Contains(w[0]) && !word.Contains(w[1]) && !word.Contains(w[2]) && w[3] == word[4] && w[4] == word[3] && w[3] != w[4]))// && AllWords.Any(w => w != word && w.Substring(0, 4) == word.Substring(0, 4)))
                //{
                //    count++;
                //    Console.WriteLine(word);
                //}
                //if (word[0] != 's' && word[2] != 'l' && word[3] != 'o' && word.Contains('s') && word.Contains('l') && word.Contains('o'))// && word[1] != 'r' && word[2] != 'r' && word.Contains('r') && word.Contains('t') && !prevWords.Contains(word))
                //{
                //    Console.WriteLine(word);
                //    count++;
                int take = 18;
                string lambda1String = "";
                string lambda2String = "";
                string lambda3String = "";
                Func<string, string, bool> lambda1 = null;
                Func<string, string, bool> lambda2 = null;
                Func<string, string, bool> lambda3 = null;
                if (byg1.Length == 5)
                    lambda1 = GetLambda(byg1);//(w, word) => w != word && !prevWords.Contains(w) && w.Substring(2,2) != word.Substring(2,2) && w[0] != word[0] && w[1] == word[1] && w[4] == word[4] && w[0] != word[2] && w[0] != word[3] && w[2] != word[3];
                if (byg2.Length == 5)
                {
                    lambda2 = GetLambda(byg2);//(w, word) => w != word && !prevWords.Contains(w) && w[0] == word[0] && w.Substring(2, 3) == word.Substring(2, 3);
                    take = 9;
                }
                if (byg3.Length == 5)
                {
                    lambda3 = GetLambda(byg3);//(w, word) => w != word && !prevWords.Contains(w) && w[0] == word[0] && w.Substring(2, 3) == word.Substring(2, 3);
                    take = 6;
                }

                if (!prevWords.Contains(word))//!WordLists.Words.Contains(word) && 
                    if (CheckYellow(word, yellow) && CheckGreens(word, greens))//(word[0] == 'r' && word[1] != 'e' && word.Contains('e'))// && word[3] != 'u' && word.Contains('u'))// && word[1] != 't' && word[0] == 's' && word.Contains('t'))// && word[3] == 'e' && word[4] != 'r' && word.Contains('a') && word.Contains('r'))// && word[2] == 'g' && word.Contains('e'))
                        if ((lambda1 == null || (lambda1 != null && byg1WordList.Where(w => lambda1(w, word)).Count() >= byg1count)) &&
                            (lambda2 == null || (lambda2 != null && byg2WordList.Where(w => lambda2(w, word)).Count() >= byg2count)) &&
                            (lambda3 == null || (lambda3 != null && byg3WordList.Where(w => lambda3(w, word)).Count() >= byg3count)))
                        {
                            if (lambda1 != null)
                            {
                                lambda1String = ',' + string.Join(',', byg1WordList.Where(w => lambda1(w, word)).Take(take));
                            }
                            if (lambda2 != null)
                            {
                                lambda2String = '@' + string.Join(',', byg2WordList.Where(w => lambda2(w, word)).Take(take));
                            }
                            if (lambda3 != null)
                            {
                                lambda3String = '*' + string.Join(',', byg3WordList.Where(w => lambda3(w, word)).Take(take));
                            }
                            //Console.WriteLine(word);
                            notExcludedWords.Add(word);
                            Console.WriteLine(word + lambda1String + lambda2String + lambda3String);// !prevWords.Contains(w) && ////!= word && !prevWords.Contains(w) && w[0] != word[0] && w[1] != word[1] && w[2] == word[2] && w[3] != word[3] && w[4] == word[4] && w[0] != word[1] && w[0] != word[3])));
                            count++;
                            foreach (char charCount in charCounts)
                            {
                                if (!charsToEliminate.Contains(charCount))
                                {
                                    if (!charCountResults.ContainsKey(charCount))
                                    {
                                        charCountResults.Add(charCount, new int[5]);
                                    }
                                    charCountResults[charCount][0] += word[0] == charCount ? 1 : 0;
                                    charCountResults[charCount][1] += word[1] == charCount ? 1 : 0;
                                    charCountResults[charCount][2] += word[2] == charCount ? 1 : 0;
                                    charCountResults[charCount][3] += word[3] == charCount ? 1 : 0;
                                    charCountResults[charCount][4] += word[4] == charCount ? 1 : 0;
                                }
                            }
                            foreach (char letter in word)
                            {
                                if (!charCountAll.ContainsKey(letter))
                                {
                                    charCountAll.Add(letter, new int[5]);
                                }
                                charCountAll[letter][0] += word[0] == letter ? 1 : 0;
                                charCountAll[letter][1] += word[1] == letter ? 1 : 0;
                                charCountAll[letter][2] += word[2] == letter ? 1 : 0;
                                charCountAll[letter][3] += word[3] == letter ? 1 : 0;
                                charCountAll[letter][4] += word[4] == letter ? 1 : 0;
                            }
                        }

                //if (!containsKey.ContainsKey(word.Substring(0, 3)))
                //{
                //    containsKey.Add(word.Substring(0, 3), new List<string>() { word });
                //}
                //else
                //{
                //    containsKey[word.Substring(0, 3)].Add(word);
                //}
            }

            if (charCountResults.Count != 0)
            {
                foreach (KeyValuePair<char, int[]> charCount in charCountResults)
                {
                    Console.WriteLine($"Total characters for {charCount.Key}: {charCount.Value.Aggregate((sum, val) => sum + val)}");
                    Console.WriteLine($"Index 0: {charCount.Value[0]}");
                    Console.WriteLine($"Index 1: {charCount.Value[1]}");
                    Console.WriteLine($"Index 2: {charCount.Value[2]}");
                    Console.WriteLine($"Index 3: {charCount.Value[3]}");
                    Console.WriteLine($"Index 4: {charCount.Value[4]}");
                }
            }
            if (charCountAll.Count != 0)
            {
                Console.WriteLine("Most Common Characters:");
                foreach (KeyValuePair<char, int[]> charCount in charCountAll.OrderByDescending(entry => entry.Value.Sum()).Take(5))
                {
                    Console.WriteLine($"Total characters for {charCount.Key}: {charCount.Value.Aggregate((sum, val) => sum + val)}");
                    Console.WriteLine($"Index 0: {charCount.Value[0]}");
                    Console.WriteLine($"Index 1: {charCount.Value[1]}");
                    Console.WriteLine($"Index 2: {charCount.Value[2]}");
                    Console.WriteLine($"Index 3: {charCount.Value[3]}");
                    Console.WriteLine($"Index 4: {charCount.Value[4]}");
                }
                Console.WriteLine("Word With Most Common Characters: " + FindWordsWithCommonLetters(notExcludedWords, charCountAll.OrderByDescending(entry => entry.Value.Sum()).Take(5).Select(c => c.Key).ToList()));
            }
            Console.WriteLine("Total found: " + count.ToString());

            return;

            //foreach (var kv in containsKey)
            //{
            //    if (kv.Value.Count > 1)
            //    {
            //        //Console.WriteLine(kv.Key);
            //        foreach (var word in kv.Value)
            //        {
            //            if (kv.Value.Where(c => c[3] == word[4]).Count() > 0)
            //            {
            //                if (!containsKey2.ContainsKey(kv.Key))
            //                    containsKey2.Add(kv.Key, new List<string>());

            //                containsKey2[kv.Key].Add(word + ", " + kv.Value.Where(c => c[3] == word[4]).First());
            //            }
            //            //Console.WriteLine(word);
            //        }
            //    }
            //}


            //foreach (var kv in containsKey2)
            //{
            //    if (kv.Value.Count > 1)
            //    {
            //        Console.WriteLine(kv.Key);
            //        foreach (var word in kv.Value)
            //        {
            //            if (!prevWords.Contains(word.Split(",")[1].Trim()) && word.Split(",")[1].Trim().Contains("r") && word.Split(",")[1].Trim()[1] != 'r' && word.IndexOfAny("tied".ToCharArray()) == -1)
            //            {
            //                Console.WriteLine(word);
            //                count++;
            //            }
            //        }
            //    }
            //}

            //Console.WriteLine("Total found: " + count.ToString());
        }

        //static string FindWordWithCommonLetters(List<string> words, List<char> commonLetters)
        //{
        //    // Find the word with the most occurrences of common letters
        //    string resultWord = "";
        //    int maxLetterCount = 0;

        //    foreach (string word in words)
        //    {
        //        int currentLetterCount = word.Count(commonLetters.Contains);

        //        if (currentLetterCount > maxLetterCount)
        //        {
        //            resultWord = word;
        //            maxLetterCount = currentLetterCount;
        //        }
        //    }

        //    return resultWord;
        //}

        static string FindWordsWithCommonLetters(List<string> words, List<char> commonLetters)
        {
            // Find the word with the most occurrences of common letters
            List<string> resultWords = new List<string>();
            int maxLetterCount = 0;

            foreach (string word in words)
            {
                int currentLetterCount = commonLetters.Count(l => word.Contains(l)); //word.Count(commonLetters.Contains)

                if (currentLetterCount > maxLetterCount)
                {
                    resultWords = new List<string>();
                    maxLetterCount = currentLetterCount;
                }
                if (currentLetterCount == maxLetterCount)
                {
                    resultWords.Add(word);
                }
            }

            return string.Join(", ", resultWords);
        }

        public static bool CheckYellow(string word, string[] yellows)
        {
            //ParameterExpression w = Expression.Parameter(typeof(string), "w");

            if (yellows.Where(w => w != "     ").Count() == 0)
                return true;

            bool retVal = false;
            foreach (string containString in yellows)
            {
                if (containString == "     ")
                {
                    continue;
                }

                for (int i = 0; i < 5; i++)
                {
                    if (containString[i] == ' ')
                    {
                        continue;
                    }
                    if (word[i] == containString[i])
                    {
                        return false;
                    }
                    else
                    {
                        if (word.Contains(containString[i]))
                        {
                            retVal = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return retVal;
        }

        public static bool CheckGreens(string word, string[] greens)
        {
            //ParameterExpression w = Expression.Parameter(typeof(string), "w");

            if (greens.Where(w => w != "     ").Count() == 0)
                return true;

            bool retVal = false;
            foreach (string containString in greens)
            {
                if (containString == "     ")
                {
                    continue;
                }

                for (int i = 0; i < 5; i++)
                {
                    if (containString[i] == ' ')
                    {
                        continue;
                    }
                    if (word[i] == containString[i])
                    {
                        retVal = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return retVal;
        }

        public static Func<string, string, bool> GetLambda(string byg)
        {
            ParameterExpression w = Expression.Parameter(typeof(string), "w");
            ParameterExpression word = Expression.Parameter(typeof(string), "word");
            //BinaryExpression comparison = Expression.Equal(w, word);
            // Create a binary expression to check if param1 != param2
            BinaryExpression notEqualExpression = Expression.NotEqual(w, word);

            // Initialize a list to hold the comparison expressions
            var comparisonExpressions = new List<Expression>
            {
                notEqualExpression
            };

            for (int i = 0; i < 5; i++)
            {
                char condition = byg[i];
                Expression conditionExpression = null;

                switch (condition)
                {
                    case 'b':
                        // param1[i] != param2[x] where x is every location where conditions has a 'g'
                        var equalExpressions = new List<Expression>();
                        for (int j = i; j < 5; j++)
                        {
                            if (byg[j] != 'g')
                            {
                                equalExpressions.Add(
                                    Expression.NotEqual(
                                        Expression.MakeIndex(w, typeof(string).GetProperty("Chars"), new[] { Expression.Constant(i) }),
                                        Expression.MakeIndex(word, typeof(string).GetProperty("Chars"), new[] { Expression.Constant(j) })
                                    )
                                );
                            }
                        }
                        if (equalExpressions.Count() > 0)
                            conditionExpression = equalExpressions.Aggregate(Expression.AndAlso);
                        break;

                    case 'y':
                        // param1[i] = param2[x] where x is an index where conditions[x] is not 'g'
                        var notEqualExpressions = new List<Expression>();
                        for (int j = 0; j < 5; j++)
                        {
                            if (i != j && byg[j] != 'g')
                            {
                                notEqualExpressions.Add(
                                    Expression.Equal(
                                        Expression.MakeIndex(w, typeof(string).GetProperty("Chars"), new[] { Expression.Constant(i) }),
                                        Expression.MakeIndex(word, typeof(string).GetProperty("Chars"), new[] { Expression.Constant(j) })
                                    )
                                );
                            }
                        }

                        if (notEqualExpressions.Count() > 0)
                            conditionExpression = notEqualExpressions.Aggregate(Expression.OrElse);
                        break;

                    case 'g':
                        // param1 and param2 have the same character at position i
                        conditionExpression = Expression.Equal(
                            Expression.MakeIndex(w, typeof(string).GetProperty("Chars"), new[] { Expression.Constant(i) }),
                            Expression.MakeIndex(word, typeof(string).GetProperty("Chars"), new[] { Expression.Constant(i) })
                        );
                        break;

                    default:
                        throw new ArgumentException($"Invalid condition: {condition}");
                }

                if (conditionExpression != null)
                    comparisonExpressions.Add(conditionExpression);
            }

            // Combine all comparison expressions using AndAlso
            Expression finalExpression = comparisonExpressions.Aggregate(Expression.AndAlso);

            return Expression.Lambda<Func<string, string, bool>>(finalExpression, w, word).Compile();
        }

        //public static List<string> GetUsedWords(int daysToSubtract = 0)
        //{
        //    if (daysToSubtract > 0)
        //    {
        //        daysToSubtract *= -1;
        //    }
        //    string filePath = @".\previous_words.json"; // Set the file path
        //    List<string> previousWords = new List<string>(); // Create an empty list to hold the words

        //    // Open the file using a stream reader
        //    using (StreamReader sr = new StreamReader(filePath))
        //    {
        //        // Read the file line by line
        //        string line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            // Add each line to the list of previous words
        //            previousWords.Add(line);
        //        }
        //    }
        //    List<string> returnVal = previousWords.Take(previousWords.Count + daysToSubtract).ToList();//Should remove previous days if daysToSubtract is not 0
        //    bool hasResponse = true;
        //    DateTime date = DateTime.Today.AddDays(daysToSubtract).AddHours(-5);
        //    string url;
        //    HttpWebRequest request;
        //    JsonDto obj;

        //    using (StreamWriter sw = new StreamWriter(filePath, true))
        //    {
        //        do
        //        {
        //            date = date.AddDays(-1);
        //            url = $"https://www.nytimes.com/svc/wordle/v2/{date:yyyy-MM-dd}.json";
        //            try
        //            {
        //                request = (HttpWebRequest)WebRequest.Create(url);
        //                request.Method = WebRequestMethods.Http.Get;
        //                request.Accept = "application/json";

        //                using (var response = request.GetResponse())
        //                {
        //                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                    obj = JsonConvert.DeserializeObject<JsonDto>(responseString);
        //                }

        //                if (obj.Days_Since_Launch <= 1 || obj == null || returnVal.Contains(obj.Solution.ToLower()))
        //                {
        //                    hasResponse = false;
        //                }
        //                else
        //                {
        //                    returnVal.Add(obj.Solution.ToLower());
        //                    sw.WriteLine(obj.Solution.ToLower());
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //                return returnVal;
        //            }

        //        } while (hasResponse);
        //    }

        //    return returnVal;
        //}

        public static List<string> GetUsedWords(int daysToSubtract = 0)
        {
            if (daysToSubtract > 0)
            {
                daysToSubtract *= -1;
            }

            string filePath = @".\previous_words.json"; // Updated file path to a JSON file
            string filePathTxt = @".\previous_words.txt";
            List<JsonDto> previousWordsJson = new List<JsonDto>();
            List<string> previousWords = new List<string>();

            // Deserialize existing JSON content if the file exists
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        JsonDto dto = JsonConvert.DeserializeObject<JsonDto>(line);
                        if (dto != null)
                        {
                            previousWordsJson.Add(dto);
                        }
                    }
                }
            }

            bool hasResponse = true;
            DateTime date = DateTime.Now.AddHours(-5);
            string url;
            HttpWebRequest request;
            JsonDto obj;
            string todaysWord;

            //Get today's word and add it to WordLists.Words
            url = $"https://www.nytimes.com/svc/wordle/v2/{date:yyyy-MM-dd}.json";
            date = DateTime.Today.AddDays(daysToSubtract).AddHours(-5);
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            using (var response = request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                obj = JsonConvert.DeserializeObject<JsonDto>(responseString);
                todaysWord = obj.Solution.ToLower();
            }
            if (!WordLists.Words.Contains(todaysWord))
            {
                WordLists.Words.Add(todaysWord);
            }


            List<string> returnVal = previousWordsJson.Where(j => DateTime.Parse(j.Print_Date) < date).Select(j => j.Solution).ToList();

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                do
                {
                    date = date.AddDays(-1);
                    url = $"https://www.nytimes.com/svc/wordle/v2/{date:yyyy-MM-dd}.json";

                    try
                    {
                        request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = WebRequestMethods.Http.Get;
                        request.Accept = "application/json";

                        using (var response = request.GetResponse())
                        {
                            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                            obj = JsonConvert.DeserializeObject<JsonDto>(responseString);
                        }

                        if (obj.Days_Since_Launch <= 0 || obj == null || returnVal.Contains(obj.Solution.ToLower()))
                        {
                            if (!returnVal.Contains(obj.Solution.ToLower()))
                            {
                                returnVal.Add(obj.Solution.ToLower());
                                sw.WriteLine(JsonConvert.SerializeObject(obj)); // Write the JSON object to the file
                            }
                            hasResponse = false;
                        }
                        else
                        {
                            returnVal.Add(obj.Solution.ToLower());
                            sw.WriteLine(JsonConvert.SerializeObject(obj)); // Write the JSON object to the file
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return returnVal;
                    }
                } while (hasResponse);
            }

            if (File.Exists(filePathTxt))
            {
                using (StreamWriter sr = new StreamWriter(filePathTxt, false))
                {
                    foreach (string word in returnVal)
                    {
                        sr.WriteLine(word);
                    }
                }
            }

            if (returnVal.Contains(todaysWord))
            {
                returnVal.Remove(todaysWord);
            }

            return returnVal;
        }

        public class JsonDto
        {

            public int Id { get; set; }

            public string Solution { get; set; }

            public string Print_Date { get; set; }

            public int Days_Since_Launch { get; set; }

            public string Editor { get; set; }
        }
    }
}
