using AdmiProducts.Models;
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
            Console.Clear();
            ShowHeader("SISTEMA DE INVENTARIO");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[1] Consultar productos");
            Console.WriteLine("[2] Insertar producto");
            Console.WriteLine("[3] Actualizar producto");
            Console.WriteLine("[4] Eliminar producto");
            Console.WriteLine("[5] Bitacora de un premio");
            Console.WriteLine("[6] Salir");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(new string('-', Width));
        }

        public static void Success(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"[OK]::{message}".Trim());
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[ERROR]::{message}".Trim());
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"[INFO]::{message}".Trim());
            Console.ResetColor();
        }

        public static void MsgUser(string text)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
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


        // En ConsoleUI.cs
        public static void ShowTable<T>(IEnumerable<T> items, params (string Header, Func<T, object> Selector)[] columns)
        {
            // Transforma el IEnumerable en una lista
            var lista = items.ToList();

            // Valida si la lista esta vacia
            if (!lista.Any())
            {
                Console.WriteLine($"No hay registros para mostrar");
            }

            // Calcular el ancho de cada columna: el mayor entre el header y los valores
            var anchos = columns.Select(col =>
            {
                int maxValor = lista.Max(item => col.Selector(item)?.ToString()?.Length ?? 0);
                return Math.Max(col.Header.Length, maxValor);
            }).ToArray();


            Console.WriteLine(new string('-', anchos.Sum() + (columns.Length * 3)));

            // Imprimir encabezado
            for (int i = 0; i < columns.Length; i++)
            {
                Console.Write($"{columns[i].Header.PadRight(anchos[i])} | ");
            }
            Console.WriteLine();

            // Línea separadora
            Console.WriteLine(new string('-', anchos.Sum() + (columns.Length * 3)));

            // Imprimir filas
            foreach (var item in lista)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    var valor = columns[i].Selector(item)?.ToString() ?? "";
                    Console.Write($"{valor.PadRight(anchos[i])} | ");
                }
                Console.WriteLine();
            }
        }
    }

}
