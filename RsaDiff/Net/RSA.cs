using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JLM.NetSocket
{
    public class RSA
    {
        public static int[] crypted = new int[150];
        public static int[] encrypted = new int[150];
        public static int mlen = 0;
        private bool isPrime(int x)
        {
            if (x < 3)
                return false;
            for (int i = x - 1; i > 1; i--)
            {
                if (x % i == 0)
                    return false;
            }
            return true;
        }
        private int findPrime(int x = 1)
        {
            int myPrime = 2;
            while (!isPrime(myPrime) || myPrime > 100)
            {
                Random rnd = new Random();
                myPrime = rnd.Next(100);
                if (myPrime == x)
                    myPrime = 2;
            }

            return myPrime;
        }

        private int findQ_n(int first, int sec)
        {
            return (first - 1) * (sec - 1);
        }
        private int findN(int first, int sec)
        {
            return (first) * (sec);
        }
        private bool Coprime(int first, int sec)
        {
            int mlen = 0;
            if (first < sec)
                mlen = first;
            else
                mlen = sec;
            for (int i = 2; i < mlen; i++)
            {
                if (first % i == 0 && sec % i == 0)
                    return false;
            }
            return true;
        }
        private int findEncKey(int Q_n)
        {
            int mKey = 2;
            while (true)
            {
                Random rnd = new Random();
                mKey = (rnd.Next(Q_n - 2)) + 2;
                if (Coprime(mKey, Q_n))
                    break;
            }
            return mKey;
        }
        private int findD(int EncKey, int Q_n)
        {
            for (int d = 1; true; d++)
            {
                if ((d * EncKey) % Q_n == 1)
                    return d;
            }
        }
        private int c_dmodn(int c, int d, int n)
        {
            int value = 1;
            while (d > 0)
            {
                value *= c;
                value %= n;
                d--;
            }
            return value;
        }
        private int crypteWord(int m, int mKey, int myN)
        {
            return c_dmodn(m, mKey, myN);
        }
        private int EncrypteWord(int c, int myD, int myN)
        {
            return c_dmodn(c, myD, myN);
        }
        public string Encrypt(int prime1, int prime2, string message)
        {
            int first = prime1;
            int sec = prime2;
            int Q_n = findQ_n(first, sec);
            int myN = findN(first, sec);
            int mKey = findEncKey(Q_n);
            int myD = findD(mKey, Q_n);
            int x, sifre, cozum;
            string res = "";
            string mWord = message;

            for (int i = 0; i < mWord.Length; i++)
            {
                x = (int)mWord[i];
                sifre = crypteWord(x, mKey, myN);
                cozum = EncrypteWord(sifre, myD, myN);
                crypted[i] = sifre;
                encrypted[i] = cozum;

                mlen = i + 1;
            }
            for (int i = 0; i < mlen; i++)
            {
                res += (((char)crypted[i]).ToString());
            }

            return res;
        }
        public string Decrypt(string res, string decryptWord)
        {
            for (int i = 0; i < res.Length; i++)
            {
                decryptWord += (((char)encrypted[i]).ToString());
            }
            return decryptWord;
        }
    }
}
