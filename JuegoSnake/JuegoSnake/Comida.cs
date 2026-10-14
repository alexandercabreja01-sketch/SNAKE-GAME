using System;
using System.Drawing;

namespace juego_snake
{
    public class Comida
    {
        public Point Posicion { get; set; }
        public ConsoleColor Color { get; set; }
        public Ventana VentanaC { get; set; }

        public Comida(ConsoleColor color, Ventana ventanaC)
        {
            Color = color;
            VentanaC = ventanaC;
        }

        public void Dibujar()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Posicion.X, Posicion.Y);
            Console.WriteLine("█");
        }

        public bool GenerarComida(Snake snake)
        {
            int longitudSnake = snake.Cuerpo.Count + 1;
            if (VentanaC.Area - longitudSnake <= 0)
            {
                return false;
            }

            Random random = new Random();
            int x = random.Next(VentanaC.LimiteSuperior.X + 1, VentanaC.LimiteInferior.X);
            int y = random.Next(VentanaC.LimiteSuperior.Y + 1, VentanaC.LimiteInferior.Y);

            Posicion = new Point(x, y);

            foreach (Point parte in snake.Cuerpo)
            {
                if ((Posicion == parte) || (Posicion == snake.Cabeza))
                {
                    return GenerarComida(snake);
                }
            }

            Dibujar();
            return true;
        }
    }
}