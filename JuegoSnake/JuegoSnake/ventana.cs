using System;
using System.Drawing;

namespace juego_snake
{
    public class Ventana
    {
        public string Titulo { get; set; }
        public int Ancho { get; set; }
        public int Altura { get; set; }
        public ConsoleColor ColorFondo { get; set; }
        public ConsoleColor ColorLetra { get; set; }
        public Point LimiteSuperior { get; set; }
        public Point LimiteInferior { get; set; }
        public int Area { get; set; }
        public Snake SnakeC { get; set; }

        public Ventana(string titulo, int ancho, int altura, ConsoleColor colorFondo, ConsoleColor colorLetra, Point limiteSuperior, Point limiteInferior)
        {
            Titulo = titulo;
            Ancho = ancho;
            Altura = altura;
            ColorFondo = colorFondo;
            ColorLetra = colorLetra;
            LimiteSuperior = limiteSuperior;
            LimiteInferior = limiteInferior;
            Area = (LimiteInferior.X - LimiteSuperior.X - 1) * (LimiteInferior.Y - LimiteSuperior.Y - 1);

            Init();
        }

        private void Init()
        {
            Console.SetWindowSize(Ancho, Altura);
            Console.Title = Titulo;
            Console.CursorVisible = false;
            Console.BackgroundColor = ColorFondo;
            Console.Clear();
        }

        public void DibujarMarco()
        {
            Console.ForegroundColor = ColorLetra;

            // Parte Superior
            for (int x = LimiteSuperior.X; x < LimiteInferior.X; x++)
            {
                Console.SetCursorPosition(x, LimiteSuperior.Y);
                Console.Write('═');
            }

            // Parte Inferior
            for (int x = LimiteSuperior.X; x < LimiteInferior.X; x++)
            {
                Console.SetCursorPosition(x, LimiteInferior.Y);
                Console.Write('═');
            }

            // Lado Izquierdo y Derecho
            for (int y = LimiteSuperior.Y; y < LimiteInferior.Y; y++)
            {
                Console.SetCursorPosition(LimiteSuperior.X, y);
                Console.Write('{');
                Console.SetCursorPosition(LimiteInferior.X, y);
                Console.Write('}');
            }

            // Esquinas
            Console.SetCursorPosition(LimiteSuperior.X, LimiteSuperior.Y);
            Console.Write('*');
            Console.SetCursorPosition(LimiteSuperior.X, LimiteInferior.Y);
            Console.Write('*');
            Console.SetCursorPosition(LimiteInferior.X, LimiteSuperior.Y);
            Console.Write('*');
            Console.SetCursorPosition(LimiteInferior.X, LimiteInferior.Y);
            Console.Write('*');
        }

        public void Menu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            int centroX = (LimiteSuperior.X + LimiteInferior.X) / 2;
            int centroY = (LimiteSuperior.Y + LimiteInferior.Y) / 2;

            Console.SetCursorPosition(centroX - 12, centroY - 4);
            Console.Write("JUEGO SNAKE");

            Console.SetCursorPosition(centroX - 8, centroY - 2);
            Console.Write("ENTER - Jugar");

            Console.SetCursorPosition(centroX - 8, centroY - 1);
            Console.Write("ESCAPE - Salir");

            
            SnakeC = new Snake(new Point(centroX, LimiteInferior.Y - 3), ConsoleColor.Magenta, ConsoleColor.White, this);
            SnakeC.IniciarCuerpo(4);
        }

        public void Teclado(ref bool ejecucion, ref bool jugar, Snake snake)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    jugar = true;
                    Console.Clear();
                    DibujarMarco();
                    snake.Init();
                }
                else if (tecla.Key == ConsoleKey.Escape)
                {
                    ejecucion = false;
                }
                else if (tecla.Key == ConsoleKey.RightArrow && snake.DireccionActual != Direccion.Izquierda)
                {
                    snake.DireccionActual = Direccion.Derecha;
                }
                else if (tecla.Key == ConsoleKey.LeftArrow && snake.DireccionActual != Direccion.Derecha)
                {
                    snake.DireccionActual = Direccion.Izquierda;
                }
                else if (tecla.Key == ConsoleKey.UpArrow && snake.DireccionActual != Direccion.Abajo)
                {
                    snake.DireccionActual = Direccion.Arriba;
                }
                else if (tecla.Key == ConsoleKey.DownArrow && snake.DireccionActual != Direccion.Arriba)
                {
                    snake.DireccionActual = Direccion.Abajo;
                }
            }
        }

        public void GameOver(string texto)
        {
            Console.Clear();
            DibujarMarco();
            int centroX = (LimiteSuperior.X + LimiteInferior.X) / 2;
            int centroY = (LimiteSuperior.Y + LimiteInferior.Y) / 2;

            Console.SetCursorPosition(centroX - 10, centroY - 2);
            Console.Write(texto);
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
            DibujarMarco();
        }
    }
}