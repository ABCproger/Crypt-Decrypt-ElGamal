using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace El_Gamal
{
    internal class Program
    {
            static void Main(string[] args)
        {
            int g, x, p, k;
            List<int> cryptedText = new List<int>();
            string message;
            Random random = new Random();
            Console.WriteLine("Hi, its a ElGamal cryptology program ");
           
            Console.WriteLine("Please, enter a number");
            g = int.Parse(Console.ReadLine());
            Console.WriteLine("Please, enter a pow of number");
            x = int.Parse(Console.ReadLine());                  // must be rand 1<x<p!!!!!!!
            Console.WriteLine("Please, enter a mod");
            p = int.Parse(Console.ReadLine()); // має бути простим числом
            Console.WriteLine("Please, enter a message");
            message = Console.ReadLine();
            
            cryptedText = crypt(p,g,x,message);
            Console.WriteLine("Crypted text");

            for(int i = 1; i < cryptedText.Count; i++)
            {
                Console.Write(cryptedText[i] + " ");
            }
            Console.WriteLine();
            decrypt(cryptedText,p,x);
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
                    
                       // Console.Write(tempStr + " ");
                    
                    //Console.WriteLine("msg: " + tempStr);
                    b = multiplication(stepY, tempStr, p);     // b= y^k * msg mod p
                    //Console.WriteLine("TEST "+b);                            // Друга частина шифротексту
                    //Console.WriteLine("ENCRYPT TEST "+ a);                   // перша чатсина шифротексту
                    cryptedText.Add(b);
                }
            }
            return cryptedText;
        }

        static void decrypt(List<int> cryptedText, int p, int x)
        {
            int stepA, A,intMsg,B;
            char msg;
            A = cryptedText[0];
            //Console.WriteLine("A: " + A);
            stepA = powerModp(A,p-1-x,p);
            Console.WriteLine("DECRYPTED TEXT");
            for (int i = 1; i < cryptedText.Count; i++) 
            {
                B = cryptedText[i];
                intMsg = multiplication(B,stepA,p);
                msg = Convert.ToChar(intMsg);
                Console.Write(msg);
            }

        }
    }

}
