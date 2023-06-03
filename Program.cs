using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace El_Gamal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int g, x, p;
            List<int> cryptedText = new List<int>();
            Random random = new Random();
            Console.WriteLine("Hi, its a ElGamal cryptology program \n");

            int chooseOperationNumb = chooseOperation();
            if (chooseOperationNumb == 1)
            {
                Console.Write("Please, enter a way to file which you want to crypt: ");
                string pathToCrypt = Console.ReadLine();
                FileInfo fileInf = new FileInfo("pathToCrypt");
                string messageFromFile = File.ReadAllText(pathToCrypt); // Зчитування тексту з файлу
                Console.WriteLine("Succesful! text has been read from file, your text: ");
                Console.WriteLine(messageFromFile);
                do
                {
                    p = random.Next(2000);
                } while (IsPrimeNumber(p) != true);
                g = random.Next(1, p);
                x = random.Next(1, p);
                cryptedText = crypt(p, g, x, messageFromFile);
                Console.WriteLine("Crypted text");

                for (int i = 0; i < cryptedText.Count; i++)
                {
                    Console.Write(cryptedText[i] + " ");
                }
                Console.WriteLine();
                bool ind = true;
                while (ind)
                {
                    Console.Write("Do you want decrypt this crypted text?[Y/N]: ");
                    string answer = Console.ReadLine();

                    if (answer == "Y" || answer == "y")
                    {
                        decrypt(cryptedText, p, x);
                        ind = false;
                    }
                    else if (answer == "N" || answer == "n")
                    {
                        Console.WriteLine();
                        ind = false;
                    }
                    else ind = true;
                }
            }
            else if (chooseOperationNumb == 2)
            {

                Console.Write("Please, enter a way to file which you want to decrypt: ");
                string pathToDecrypt = Console.ReadLine();
                Console.WriteLine(pathToDecrypt);
                using (StreamReader sr = new StreamReader(pathToDecrypt))
                {
                    Console.WriteLine("Succesfull! cypted text has been read from file ");
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] numberStrings = line.Split(' '); // розділити рядок за допомогою пропуску

                        foreach (string numberString in numberStrings)
                        {
                            int number = int.Parse(numberString); // конвертувати окреме число в тип int
                            cryptedText.Add(number); // додати число до списку
                        }
                    }

                    Console.Write("Please, enter a secret key: ");
                    int secretKeyX = int.Parse(Console.ReadLine());
                    Console.Write("Please, enter a mod: ");
                    int modOfNumb = int.Parse(Console.ReadLine());
                    decrypt(cryptedText, modOfNumb, secretKeyX);
                }
            }
        }
        static int chooseOperation()
        {
            int chooseOperationNumb = 0;
            bool ind = true;
            while (ind)
            {
                Console.WriteLine("Please, choose a function what do you want to do: \n");
                Console.WriteLine("1. Crypt your text file: \n2. Decrypt your text file:");
                chooseOperationNumb = int.Parse(Console.ReadLine());
                if (chooseOperationNumb < 1 || chooseOperationNumb > 2)
                {
                    Console.WriteLine("Error number of operation lets try now ");
                    ind = true;
                }
                else ind = false;
            }
            return chooseOperationNumb;
        }

        public static bool IsPrimeNumber(int n)
        {
            var result = true;

            if (n > 1)
            {
                for (var i = 2; i < n; i++)
                {
                    if (n % i == 0)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        static int powerModp(int x, int y, int n) // x^y mod n піднесення основи а до степеня b за модулем N
        {
            if (y == 0) return 1;
            int z = powerModp(x, y / 2, n);
            if (y % 2 == 0)
                return (z * z) % n;
            else
                return (x * z * z) % n;
        }

        static int multiplication(int a, int b, int n) // a*b mod n - множення a на b за модулю n
        {
            int sum = 0;

            for (int i = 0; i < b; i++)
            {
                sum += a;
                if (sum >= n)
                {
                    sum -= n;
                }
            }
            return sum;
        }

        static List<int> crypt(int p, int g, int x, string message)
        {
            int y, a, b, stepY;
            Random random = new Random();
            List<int> cryptedText = new List<int>();
            int k = random.Next() % (p - 2) + 1;            // k must be (1<k<p-1)
            Console.WriteLine("k: " + k);
            y = powerModp(g, x, p);
            Console.WriteLine("Open key(p,g,y): " + p + "  " + g + " " + y);
            Console.WriteLine("Close key (x)" + x); 
            if (message.Length > 0)
            {
                char[] temp = new char[message.Length - 1];
                temp = message.ToCharArray();
                a = powerModp(g, k, p);                                 // A=g^k mod p
                cryptedText.Add(a);
                stepY = powerModp(y, k, p);                       // y^k
                for (int i = 0; i <= message.Length - 1; i++)
                {
                    int tempStr = Convert.ToInt32(temp[i]);
                    b = multiplication(stepY, tempStr, p);     // b= y^k * msg mod p
                    cryptedText.Add(b);
                }
            }
            return cryptedText;
        }

        static void decrypt(List<int> cryptedText, int p, int x)
        {
            int stepA, A, intMsg, B;
            char msg;
            A = cryptedText[0];
            stepA = powerModp(A, p - 1 - x, p);
            Console.WriteLine("DECRYPTED TEXT");
            for (int i = 1; i < cryptedText.Count; i++)
            {
                B = cryptedText[i];
                intMsg = multiplication(B, stepA, p);
                msg = Convert.ToChar(intMsg);
                Console.Write(msg);
            }
        }
    }

}
