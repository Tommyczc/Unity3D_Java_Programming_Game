using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace javaCompiler
{
    public static class java_highlighting
    {
        public static string[] keyWords = {"public","static","int","class","new","void","private",";","null","for","if","while","try","catch","true","false" };
        public static string[] integerWords = {"0","1","2","3", "4", "5", "6", "7", "8", "9" };
        public static string keyWordColor = "<color=orange>";//FF8C00
        public static string IntegerColor = "<color=blue>";
        public static string colorFinishWord = "</color>";
        public static int wordHighLight = 0;
        public static int integerHighLight = 0;



        public static string highlighting(string input) {
            
            string current = getPureText(input);
            foreach (string keyword in keyWords)
            {
                current=current.Replace(keyword,keyWordColor+keyword+colorFinishWord);
            }
            foreach (string integer in integerWords) {
                current = current.Replace(integer, IntegerColor + integer + colorFinishWord);
            }
            return current;
        }

        public static void updateDuplicateTime(string input) {
            //input = getPureText(input);
            wordHighLight = checkDuplicateString(input,keyWordColor);
            integerHighLight = checkDuplicateString(input,IntegerColor);
        }

        public static int getKeyWordDuplicateTime(string input) {
            int time = 0;
            foreach (string key in keyWords) {
                time+=checkDuplicateString(input,key);
            }
            return time;
        }
        public static int getIntegerDuplicateTime(string input)
        {
            int time = 0;
            foreach (string integer in integerWords)
            {
                time += checkDuplicateString(input, integer);
            }
            return time;
        }

        public static int checkDuplicateString(string str,string checkStr) {
            return (str.Length - str.Replace(checkStr, string.Empty).Length)/checkStr.Length;
        }

        public static string getPureText(string input) {
            input = input.Replace(keyWordColor,"");
            input = input.Replace(IntegerColor, "");
            input = input.Replace(colorFinishWord, "");
            return input;
        }

        private static bool isNum(char input) {
            if (char.IsDigit(input)) { return true; }
            else { return false; }
        }

        private static bool isLetter(char input) {
            if (char.IsLetter(input)) { return true; }
            else { return false; }
        }

        private static bool matchkeyWord(string input) {
            bool match=false;
            foreach (string keyword in keyWords) {
                if (input==keyword) {
                    match=true;
                }
            }
            return match;
        }










        /**
        public static string input_javaCode(string input)
        {
            string current = getPureText(input);
            string output = " ";

            while (current.Length > 0)
            {
                int theKeyWordIndex = 0;
                if (isLetter(current[0]))
                {// this part is to check key words
                    int i = 0;
                    while (isLetter(current[i]) && i < current.Length - 1)
                    {
                        i++;
                        theKeyWordIndex++;
                    }
                    Debug.Log("i: " + i + " leng:" + current.Length);

                    string theKeyWord = current.Substring(0, theKeyWordIndex);
                    current = current.Substring(theKeyWordIndex);
                    if (matchkeyWord(theKeyWord))
                    {
                        output += keyWordColor + theKeyWord + colorFinishWord;
                    }
                    else { output += theKeyWord; }

                }
                else if (isNum(current[0]))
                {
                    int i = 0;
                    while (isNum(current[i]) && i < current.Length)
                    {
                        i++;
                        theKeyWordIndex++;
                    }
                    string theKeyWord = current.Substring(0, theKeyWordIndex);
                    current = current.Substring(theKeyWordIndex);
                    output += IntegerColor + theKeyWord + colorFinishWord;
                }

                else
                {
                    string theKeyWord = current.Substring(0, 1);
                    current = current.Substring(1);
                    output += theKeyWord;
                }
            }
            return output;
        }
        **/
    }
}