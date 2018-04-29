// AUTHOR: Aylet Lopez
// FILENAME: EncryptWord.cs
// DATE: 4/14/2018
// REVISION HISTORY: Rev New

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ClassEncryption
{
    //Description: EncryptWord class encrypts a word from the user's keyboard, gets a randomly
    //generated shift factor to encrypt the word, and increases the count every time
    //the p1 driver enters the EncryptWord userQueries method.

    //invariant:
    //isEncryptionOn false state must be satisfied when creating a new  instance 

    //State Transition: 
    //isEncryptionOn is false then it's ready to be encrypted. 
    //isEncryptionOn is true then nothing else can be encrypted until the current word has been decrypted.

    //Illegal inputs:
    //special characters and numbers.
    //The randomly generated shift code has been limited to 26 ( range of
    //alphabet)

    //Assumptions:
    //word must be at least 4 characters long
    //cipher shift is only from 1 to 26. 
    //only alphabetical characters can be entered

    //Dependencies: None

    public class EncryptWord
    {
        static bool isEncryptionOn;  //sets on/off encryption
        static int shiftCode = 0;//random generated shift to be added to the ASCII of character
        const int MAXRANDOM = 26;//max number used to find the shift code
        const int MINRANDOM = 1;//minimum number used to find the shift code
        const int MINLOWER = 97;//min number of the ASCII code of upper case 'a'
        const int MINUPPER = 65;//min number of the ASCII code of upper case 'A'
        static int queries; //holds the count for every time they guess a shift
        static int highGuess;//holds count for when the guess is greater then shiftCode
        static int lowGuess;//holds count for when the guess is lower then shiftCode
        static int averageGuess;//sums all the user's guess
        static int finalGuess;//divides averageGuess/(queries count - correct shift)

        //Descrition:
        //the encryptWord constructor initializes isEncryptionOn to its default state. 
        //It generates a random shift from the shiftFactor method and initializes variables queries, 
        //highGuess, lowGuess, averageGuess, and finalGuess to 0. The intent of the constructor is 
        //to reinitialize every time the user decides they want to play again.

        
        public EncryptWord()
        {
            shiftCode = ShiftFactor();
            isEncryptionOn = false;
            queries = 0;
            highGuess = 0;
            lowGuess = 0;
            averageGuess = 0;
            finalGuess = 0;
        }

        //Description:
        //shiftFactor finds an integer that is randomly generated. The random number
        //generator ensures that it stays between the alphabetical range 1 through 26. 
        //The integer generated will act as cipher shift.
        //precondition(s): no preconditions
        //postcondition(s): no postconditions
        private int ShiftFactor()
        {
            Random rnd1 = new Random();
            int rInt = rnd1.Next(MINRANDOM, MAXRANDOM);//MINRANDOM = 1, MAXRANDOM = 26
            shiftCode = rInt + MINRANDOM; //to avoid any other ASCII code variable that's not a letter
            return shiftCode;
        }

        //Description:
        //encrypt method parameter, string, takes the user's input from keyboard and 
        //encrypts it by adding the randomly generated shift code to the ASCII code of
        //each character in the word.  The public method calls upon a private method 
        //which holds the encryption math equation to encrypt each character of the word.
        //shiftCode will stay same before and after this method is used. shiftCode is
        //only changed when a new game is started.
        //invariant: 
        //isEncryptionOn must always be false when entering the method. If not false the method will not encrypt.
        //precondition(s):
        //The word can only be encrypted when isEncryptionOn is set to false. If not, 
        //the method will not encrypt and will just return the original word. The 
        //encrypt method encrypts only lower or upper case characters. If a lower 
        //character is entered the method will encrypt it with another lower case
        //character, the same for when an upper case character is entered. 
        //postcondition(s):
        //Once a word has been encrypted based on each character, the isEncryptionOn is 
        //turned to true and you can access other methods. Once isEncryptionOn is set to
        //true you cannot encrypt again. 
        public string Encrypt(string word)
        {
            if (isEncryptionOn == false)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    char[] arr = word.ToCharArray();
                    arr[i] = ShiftLetter(arr[i], shiftCode);
                    word = new string(arr);
                }
                ChangeTheStates(word);
            }
            return word;
        }
 
        

        //Description:
        //takes the character and deciphers based on whether it is lower
        //or upper case.Since both lower and upper case have distinct locations on
        //the ASCII table we must use the following invariant variables to represent 
        //lower or upper case characters: MIN_UPPER and MIN_LOWER. These two variable 
        //will remain the same before and after method is used.
        //precondition(s):
        //a character must be entered and there must not be any space
        //between the characters - if there is space in the words entered by the user 
        //the main program needs to delete the space and merge the word.
        //postcondition(s): 
        //each character has been encrypted with a particular shift
        //code and the encryption is limited to alphabetical characters.. 
        private static char ShiftLetter(char letter, int shiftValue)
        {
            char answer;
            if (char.IsUpper(letter))
            {
                answer = Convert.ToChar((int)(letter + shiftValue - MINUPPER) % MAXRANDOM + MINUPPER);
            }
            else
            {
                answer = Convert.ToChar((int)(letter + shiftValue - MINLOWER) % MAXRANDOM + MINLOWER);
            }
            return answer;
        }

        //Description:
        //method's parameter is the user's guess for the correct shift. Each guess is 
        //either greater, lower, or equal to the shiftCode randomly generated. To get 
        //averageGuess there must be more than one guess and the denominator will
        //subtract the correct guess as the correct guess is not considered a "guess" 
        //since it's correct.
        //invariant:
        //isEncryptionOn must always be true when entering the method. If not true the
        //method will not increase the count of queries, lowGuess,highGuess, and
        //averageGuess.
        //precondition(s): 
        //When the method is entered isEncryptionOn has to be on since you can only
        //increment count if there is an encrypted word with a shift. 
        //postconditions(s):
        //postcondition(s): each of these changeable variables will no longer be 0.
        private static void UserQueries(int userGuess)
        {
            if (isEncryptionOn)
            {
                queries++;
                if (userGuess > shiftCode)
                {
                    highGuess++;
                    finalGuess += userGuess;
                }
                else if (userGuess < shiftCode)
                {
                    lowGuess++;
                    finalGuess += userGuess;
                }
                if (queries > 1)
                {
                    //doesn't take into account winning guess
                    averageGuess = finalGuess / (queries - 1);
                }
            }
        }

        //Description:
        //changeTheStates is used in the encryption method to change the state from off
        //to on if there was a word entered.
        //preconditions(s): 
        //if word must not be empty for the encrypt method to change isEncryptionOn
        //from off to on. 
        //postcondition(s): 
        //isEncryptionOn will go through a state change is a word was entered.
        private bool ChangeTheStates(string word)
        {
            if (word != "")
            {
                if (isEncryptionOn == false)
                {
                    isEncryptionOn = true;
                }
                else
                    isEncryptionOn = false;
            }
            return isEncryptionOn;
        }

        //Description:
        //reset is a precaution method. When the user wishes to end a game during their
        //guess this ensures that the encryption is set to off as the are no longer 
        //working with a encrypted word
        //precondition: the user has enter input to end current guessing game
        //postcondition: isEncryptionOn is set back to false. 
        public bool Reset()
        {

                isEncryptionOn = false;
                queries = 0;
                highGuess = 0;
                lowGuess = 0;
                averageGuess = 0;
                finalGuess = 0;
            

            return isEncryptionOn;
        }

        //Description:
        //setter for userQueries so that private UserQueries method can be used to 
        //gather the counts.
        //precondition(s): the user has entered an integer as a guess and encryption
        //is turned on
        //postcondition(s): queries will no longer be zero. If first guess is correct on
        //then highGuess, lowGuess and averageGuess will be 0. If not, then they will
        //no longer be zero.
        public  void SetUserGuess(int userGuess)
        {
            UserQueries(userGuess);
        }

        //Description: returns int queries since it's a private variable and we need
        //a way to access it
        //precondition(s): no preconditions
        //postcondition(s): no postconditions
        public  int GetQueries()
        {
            return queries;
        }

        //Description: returns int highGuess since it's a private variable and we need
        //a way to access it
        //precondition(s): no preconditions
        //postcondition(s): no postconditions
        public  int GetHighGuess()
        {
            return highGuess;
        }

        //Description: returns int lowGuess since it's a private variable and we need 
        //a way to access it
        //precondition(s): no preconditions
        //postcondition(s): no postconditions
        public int GetLowGuess()
        {
            return lowGuess;
        }

        //Description: returns int averageGuess since it's a private variable and we 
        //need a way to access it
        //precondition(s): no preconditions
        //postcondition(s): no postconditions
        public int getAverageGuess()
        {
            return averageGuess;
        }

        public int GetShiftCode()
        {
            return shiftCode;
        }


    }
}
