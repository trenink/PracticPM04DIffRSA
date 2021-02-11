using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JLM.NetSocket
{
    public class Diffy_Hellmen
    {

        public static int b;
        public static int x;
        public static int y;
        public static int kb;
        public static Random rnd = new Random();    
        public static int G = 16;
        public static int P = 40;
        public static int Power(int a, int b, int p)
        {
            if (b == 1)
            {
                return a;
            }
            else
            {
                return ((int)Math.Pow(a, b) % p);
            }
        }
        public void ListenClient(string keyB, ListBox lbData)
        {
            lbData.Items.Add($"Приватный ключ b: {keyB}");
            if (int.TryParse(keyB, out int bnew))
            {
                b = bnew;
                y = Power(G, b, P);
            }

            lbData.Items.Add($"G = {G}, P = {P} :");
            lbData.Items.Add($"Клиент Y = {y} :");

        }
        public void ConnectClient(ListBox lbData, string serverX)
        {
            try
            {
                lbData.Items.Add($"X:{serverX} ");
                if (int.TryParse(serverX, out int xnew))
                {
                    x = xnew;
                    kb = Power(x, b, P);

                }
                lbData.Items.Add($"G = {G}, P = {P}");
                lbData.Items.Add($"Клиент Y = {y} ");
                lbData.Items.Add($"Клиент kb = {kb} ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public class ServerObject
    {
        public static Random rnd = new Random();
        public static int G =16;
        public static int P = 40;
        public int a;
        public int x;
        public int y;
        public int ka;
        public static int Power(int a, int b, int p)
        {
            if (b == 1)
            {
                return a;
            }
            else
            {
                return ((int)Math.Pow(a, b) % p);
            }
        }
        public void ListenServer(string keyA, ListBox lbData)
        {
            try
            {
                lbData.Items.Add($"Приватный ключ a: {keyA}");
                if (int.TryParse(keyA, out int anew))
                {
                    a = anew;
                    x = Power(G, a, P);
                }
                lbData.Items.Add($"G = {G}, P = {P} :");
                lbData.Items.Add($"Сервер X = {x} :");
            }
            catch (Exception ex)
            {
                lbData.Items.Add(ex.Message);
            }
        }
    }
}


