using System;
using System.Drawing;

namespace juego_snake
{
    class Program
    { 
        static void Main(string[] args)
        {
            Ventana ventana = new Ventana("Snake", 65, 20, ConsoleColor.Black, ConsoleColor.White, new Point(5, 3), new Point(59, 18));
            Snake snake = new Snake(new  Point(8, 5), ConsoleColor.Red, ConsoleColor.White, ventana);

            bool ejecucion = true;
            bool jugar = false;

            while (ejecucion)
            {
                ventana.Menu();
                jugar = false;

                while (ejecucion && !jugar)
                {
                    ventana.Teclado(ref ejecucion, ref jugar, snake);
                    ventana.SnakeC.MoverMenu();
                    System.Threading.Thread.Sleep(100);
                }

                while (ejecucion && jugar && snake.Vivo)
                {
               
                    snake.Mover();
                    snake.Informacion(0, 34);
                    System.Threading.Thread.Sleep(100);
                }

                if (!snake.Vivo)
                {
                    snake.Puntaje = 0;
                }
            }
        }
    }
}