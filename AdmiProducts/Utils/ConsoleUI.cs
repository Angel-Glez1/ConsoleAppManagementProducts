using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Utils
{
    public static class ConsoleUI
    {
        private const int Width = 50;

        public static void ShowHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', Width));
            Console.WriteLine(title.PadLeft((Width + title.Length) / 2));
            Console.WriteLine(new string('=', Width));
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void ShowMenu()
        {
            ShowHeader("SISTEMA DE INVENTARIO");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[1] Insertar producto");
            Console.WriteLine("[2] Consultar productos");
            Console.WriteLine("[3] Actualizar producto");
            Console.WriteLine("[4] Eliminar producto");
            Console.WriteLine("[5] Salir");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(new string('-', Width));
        }

        public static void Success(string message)
        {

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"✗ {message}");
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"➜ {message}");
            Console.ResetColor();
        }

        public static string ReadText(string label)
        {
            Console.Write($"{label}");
            return Console.ReadLine()!;
        }

        public static int ReadInt(string label)
        {
            int value;

            while (true)
            {
                Console.Write($"{label}");

                if (int.TryParse(Console.ReadLine(), out value))
                    return value;

                Error("Debe ingresar un número válido.");
            }
        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Presione cualquier tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void Clear()
        {
            Console.ResetColor();
            Console.Clear();
        }

        public static void Text(string text)
        {
            Console.WriteLine(text);
        }
    }

}
