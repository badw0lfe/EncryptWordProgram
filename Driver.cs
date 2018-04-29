// AUTHOR: Aylet Lopez
// FILENAME: Driver.cs
// DATE: 4/29/2018
// REVISION HISTORY: Rev NEW


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassEncryption;


//Description:
//The driver main processes are to take input from the user, validate the input
//meets the requirements - must be at least 4 characters long,encrypt validated
//word, guess the random shift used to encrypt the word, and print statistics 
//associated with user's guesses,and lastly, end the game. 
//There are a few global data structures which are used in multiple functions. The 
//boolean keepAsking ensures the game continues until guesses the shift correctly.
//The integer correctShift  holds the random shift. Since it can be used in multiple
//functions it avoids having to always bring it in as a parameter. The remaining
//data structures are int shift and string word which holds the users keyboard input.

//We assume the user has read the welcome message and understands the intent of
//the game. Other assumptions include the user only entering alphabetical char.
//Illegal inputs include special characters and numbers. These inputs will pass
//through the encryption method but will not encrypt since it does not meet the
//if conditions. As such, if illegal characters are entered they will decrypt 
//wrong. The randomly generated shift code has been limited to 26 ( range of
//alphabet) and it's assumed that the user will not enter an integer greater
//than 26. Assume the user will not enter any doubles or float numerical values.
//The size of word input entered by the user is not limited, however, if it's 
//multiple words it will be processed as one long word as the space will not be
//considered. 

//dependencies: the driver class depends on the EncryptWord.cs class to function
//as intended.

//State Transition: 
//isEncryptionOn is false then the user can enter a word
//isEncryptionOn is true then the user must guess correct shift before encrypting another word

namespace EncryptWordDriver
{
    class Driver
    {
        static bool keepAsking = true; // global var. stays true until user enters quit command
        static string encryptedWord; //holds the encrypted word throughout the program
        static int correctShift; //holds the correct shift throughout the program

        public Driver()
        {
            string word;
            welcome();
            do
            {
                EncryptWord encryptWord = new EncryptWord();//EncryptWord class object
                word = askUserInput(encryptWord);
                playGame(word, encryptWord);
            } while (keepAsking);
            goodBye();
        }

        //description: 
        //Method asks user to enter a word they will like to encrypt.
        //input: 
        //The input can be character in the ASCII table. However, to be able to
        //encrypt and decrypt properly we assume that the user will only enter lower or
        //upper case alphabetical char. The user can have as many words separated by 
        //a space, however, when it's passed to the encryptWord class the space will be
        //eliminated and it will be evaluated as one big word. 
        //processing: 
        //All characters entered by the user will be validated for length
        //and if it does not meet the minimum length the user will need to continue 
        //entering words till it's at least 4 characters. All words separated by a space
        //will be evaluated as one big word - spaces eliminated
        //output: 
        //The output will return the user's word as they wrote it. If the user
        //does not enter a word that meets 4 characters or more he/she must continue 
        //to enter inputs until a word that meets the requirement is entered. 

        static string askUserInput(EncryptWord A)
        {
            string word; //holds user's word
            string united;
            Console.WriteLine("Enter a word(lower or upper case or type quit to end): ");
            united = Console.ReadLine();
            word = united.Replace(" ", " ");
            if (word.Equals("quit"))
            {
                System.Environment.Exit(0);
            }
            else
            {
                word = validateLengthOfWord(word); //validates word
            }
            return word; //returns non encrypted word


        }

        //Description: 
        //function's parameter brings in the user's original word and 
        //ensures that it's at least 4 characters long.
        //input: 
        //The input can be character in the ASCII table. However, to be able to
        //encrypt and decrypt properly we assume that the user will only enter lower or
        //upper case alphabetical char. The user can have as many words separated by 
        //a space, however, when it's passed to the encryptWord class the space will be
        //eliminated and it will be evaluated as one big word. 
        //processing: 
        //if the word has less than 4 characters it will continue to ask the
        //user to enter another word. 
        //output: 
        //the output will be a word that has 4 or more characters. 
        static string validateLengthOfWord(string word)
        {
            const int LENGTH = 4; //invariant variable. This length will not be changed
            while (word.Length< LENGTH)
            {
                Console.WriteLine( "\nPlease enter a word greater than 4 characters: ");
                word = Console.ReadLine();//gets the line from user's console
                                          //erases the space between the word and the word will now encrypt as one
                word = word.Replace(" ", " ");
            }
            return word; //returns non-encrypted word
        }

        //Description: 
        //holds the guessing game. It encrypts the word and takes in the 
        //user's guess until they've guessed correctly. It also displays the statistics.
        //Input: 
        //first input is the user's choice of word to be encrypted. This word 
        //needs to be at least 4 words in length and must not be any numbers or special
        //characters. It's limited to a legal input of lower and upper case characters. 
        //multiple words are allowed but the space between them will not be recognized
        //so it will encrypt as one long word.The second input from the user is an int 
        //holding the guess shift of the user. We assume the user will not enter any 
        //doubles or float or letters in field as they will not be processed.  
        //processing: 
        //When the game is being played the word can only be encrypted is 
        //when entering EncryptWord class the boolean variable controlling on/off is
        //turned off. If encryption is turned on then one will not be able to encrypt
        //a word. The user's word is processed and validated to be 4 characters long.
        //Legal inputs are lower or upper case characters within the alphabet. Once word
        //is encrypted, the boolean controlling on/off is changed to on and the user
        //will continue to guess as long as it stay on. The user will enter only legal
        //inputs - integers. All other inputs are illegal and will not provide a correct
        //shift. Once correct shift is found the statistics are printed out and the
        //EncryptWord class is reset to allow the user to play again. The reset sets 
        //the on/off variable to off to allow a new encryption. The statistics will only
        //be displayed if during each guess the encryption is on - if not, it will not
        //process any of the counts. The method also saves the encryptedWord to be able
        //to use in other methods. 
        //output: 
        //The output of method is the encrypted word and "guess is too low" or "guess is
        //too high" if the user has not entered correct shift. If user enters -1 the 
        //output is the end of game as it acts as a reset within the game. If shift is 
        //guessed correctly it will display the statistics. AverageGuess is only 
        //displayed if more than 1 guess has been entered since correct guess is not 
        //considered a "guess". 

        static void playGame(string word, EncryptWord encryptWord)
        {
            encryptedWord = encryptWord.Encrypt(word); //variable collects console input
            Console.WriteLine("\nThe encrypted word is: ");
            Console.WriteLine(encryptedWord);
            correctShift = encryptWord.GetShiftCode();
            guess(correctShift, encryptWord); //method will compare user's guess to the
                                              //correct shift
        }

        //Description: 
        //Asks the user to guess the correct shift that encrypted the word. If correct
        //it will provide the user with statistics of the game.
        //Input: legal inputs are only considered integers - no words, special characters, 
        //doubles, floats, etc. will be allowed. The user can input any integer they 
        //choose, however, the range of the random generator is limited to 26. Even 
        //though all integers are okay - the range will be positive so we assume the 
        //user will only enter positive numbers except for -1 which will be used to 
        //reset game.
        //processing: if a legal input is entered it will compare it to the correct
        //shift code. If there is a match it will exit the do-while loop and display the
        //statistics of the game. If -1 is entered it will exit encryption (reset) and 
        //asks the user what they will like to do. If they decide to play again they 
        //may choose to do so as everything will be reset. 
        //output: with the input being the user's choice of integer the output will be
        //"too low" or "too high" depending on what they have answered. These will 
        //continue to display until the correct shift has been entered.  if -1 is the 
        //user's input then the output states game has been reset and asks the user what
        //they want to do next. Only if one has guessed correctly will the statistics 
        //display on the screen.
        static void guess(int correctShift, EncryptWord encryptWord)
        {
            string userInput;
            int shift; //holds user's guess - The guess can only be integers
            do
            {
                Console.WriteLine("\nGuess shift between 1 thru 26(Enter -1 to exit current encryption): ");
                userInput = Console.ReadLine();
                shift = Convert.ToInt32(userInput);
                //shift = Console.Read(); //user cannot input doubles/float/words/negative numbers
                                       //other than -1
                Console.WriteLine(shift);
                if (shift > correctShift)
                {
                    Console.WriteLine("\nguess is too high");
                }
                else if (shift < correctShift && shift != -1)
                {
                    Console.WriteLine("\nguess is too low");                 
                }
                encryptWord.SetUserGuess(shift); //every guess is brought to a method 
                                                 //that increases count based on guess
            } while (shift != correctShift && shift != -1);
            if (shift == -1)
            {
                encryptWord.Reset(); // encryption is turned off 
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine( "You've guessed correct!");
                printStats(encryptWord);
                encryptWord.Reset();

            }


        }

        //Description: 
        //displays the statistics for current round of guessing game.
        //input:T
        //he function calls upon the EncryptWord class and prints out the count
        //of the statistical values. There's no input as it calls upon the following
        //EncryptWord class getter variables: queries, lowGuess, highGuess, 
        //averageGuess.
        //processing: 
        //the function only prints out the getters from the EncryptWord 
        //class userQueries method. It's this method that processes the count and the 
        //printStats function in the driver class displays the output. The avearageGuess 
        //is only processed if more than one guess has been provided. If the only guess
        //by the user is the correct one then the averageGuess will not display because
        //the correct answer is not counted in averageGuess.
        //output:
        //prints the count of EncryptWord class queries, lowGuess, highGuess, 
        //and averageGuess variables. 
        static void printStats(EncryptWord A)
        {
            Console.WriteLine("\nYou had this many guesses: ");
            Console.WriteLine(A.GetQueries());
            
            if (A.getAverageGuess() > 0){
                Console.WriteLine("Your average guess factor is: ");
                Console.WriteLine(A.getAverageGuess());              
            }
            Console.WriteLine("You had this many high guesses: ");
            Console.WriteLine(A.GetHighGuess());
            Console.WriteLine("You had this many low guesses: ");
            Console.WriteLine(A.GetLowGuess());
        }

        // Description: welcomes the user and explains the game.
        //input: no input needed from user as it's the first thing printed out.
        //processing and output: welcomes the user and explains game.

        static void welcome()
        {
            Console.WriteLine( "Welcome to the caesar cipher guessing game!");
            Console.WriteLine( "The goal of the game is to enter a word that will become\n" +
                "encrypted with a shift generated by a random number generator.\n");
            Console.WriteLine("Your goal is to guess the correct shift that encrypts the word.\n");
        }

        // Description: thanks the user for playing 
        //input: no input needed from user as it's the last thing printed out 
        //automatically 
        //processing and output: welcomes the user and explains game.

        static void goodBye()
        {
            Console.WriteLine("\nThank you for playing the game! Have a great time");
        }
    }
}

