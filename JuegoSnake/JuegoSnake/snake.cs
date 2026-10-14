using System;
using System.Collections.Generic;
using System.Drawing;

namespace juego_snake
{
    public enum Direccion
    {
        Derecha,
        Izquierda,
        Arriba,
        Abajo
    }

    public class Snake
    {
        public bool Vivo { get; set; }
        public ConsoleColor ColorCabeza { get; set; }
        public ConsoleColor ColorCuerpo { get; set; }
        public Ventana VentanaC { get; set; }
        public List<Point> Cuerpo { get; set; }
        public Point Cabeza { get; set; }
        public Direccion DireccionActual { get; set; }
        public Point PosicionInicial { get; set; }
        public Comida ComidaC { get; set; }
        public bool Comiendo { get; set; }
        public int Puntaje { get; set; }
        public int PuntajeMaximo { get; set; }

        public Snake(Point posicionInicial, ConsoleColor colorCabeza, ConsoleColor colorCuerpo, Ventana ventanaC)
        {
            PosicionInicial = posicionInicial;
            ColorCabeza = colorCabeza;
            ColorCuerpo = colorCuerpo;
            VentanaC = ventanaC;
            Cuerpo = new List<Point>();
            Puntaje = 0;
            PuntajeMaximo = 0;
            Init();
        }

        public void Init()
        {
            Console.Clear();
            Cabeza = PosicionInicial;
            Cuerpo.Clear();
            IniciarCuerpo(2);
            Vivo = true;
            DireccionActual = Direccion.Derecha;
            ComidaC = new Comida(ConsoleColor.Green, VentanaC);
            ComidaC.GenerarComida(this);
        }

        public void IniciarCuerpo(int cantidadPartes)
        {
            int x = Cabeza.X - 1;
            for (int i = 0; i < cantidadPartes; i++)
            {
                Console.SetCursorPosition(x, Cabeza.Y);
                Console.WriteLine("▓");
                Cuerpo.Add(new Point(x, Cabeza.Y));
                x--;
            }
        }

        public void Mover()
        {
            Teclado();
            Point posicionCabezaAnterior = Cabeza;
            MoverCabeza(); 
            ColisionesComida();
            MoverCuerpo(posicionCabezaAnterior);
            if (ColisionesCuerpo())
            {
                VentanaC.GameOver("GAME OVER");
                Vivo = false;
            }
        }

        public void MoverMenu()
        {
            Point posicionCabezaAnterior = Cabeza;
            MoverCabeza();
            ColisionesMarco();
            MoverCuerpo(posicionCabezaAnterior);
        }

        private void MoverCabeza()
        {
            Console.ForegroundColor = ColorCabeza;
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.WriteLine(" ");

            switch (DireccionActual)
            {
                case Direccion.Derecha:
                    Cabeza = new Point(Cabeza.X + 1, Cabeza.Y);
                    break;
                case Direccion.Izquierda:
                    Cabeza = new Point(Cabeza.X - 1, Cabeza.Y);
                    break;
                case Direccion.Abajo:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y + 1);
                    break;
                case Direccion.Arriba:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y - 1);
                    break;
            }

        
            ColisionesMarco();

            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.WriteLine("█");
        }

        private void MoverCuerpo(Point posicionCabezaAnterior)
        {
            Console.ForegroundColor = ColorCuerpo;
            Console.SetCursorPosition(posicionCabezaAnterior.X, posicionCabezaAnterior.Y);
            Console.WriteLine("▓");
            Cuerpo.Insert(0, posicionCabezaAnterior);

            if (Comiendo)
            {
                Comiendo = false;
                return;
            }

            Point ultimaPosicion = Cuerpo[Cuerpo.Count - 1];
            Console.SetCursorPosition(ultimaPosicion.X, ultimaPosicion.Y);
            Console.WriteLine(" ");
            Cuerpo.RemoveAt(Cuerpo.Count - 1);
        }
        private void Teclado()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.RightArrow && DireccionActual != Direccion.Izquierda)
                {
                    DireccionActual = Direccion.Derecha;
                }
                if (tecla.Key == ConsoleKey.LeftArrow && DireccionActual != Direccion.Derecha)
                {
                    DireccionActual = Direccion.Izquierda;
                }
                if (tecla.Key == ConsoleKey.UpArrow && DireccionActual != Direccion.Abajo)
                {
                    DireccionActual = Direccion.Arriba;
                }
                if (tecla.Key == ConsoleKey.DownArrow && DireccionActual != Direccion.Arriba)
                {
                    DireccionActual = Direccion.Abajo;
                }
            }
        }
        private void ColisionesMarco()
        {
            if (Cabeza.X <= VentanaC.LimiteSuperior.X)
            {
                Cabeza = new Point(VentanaC.LimiteInferior.X - 1, Cabeza.Y);
            }
            else if (Cabeza.X >= VentanaC.LimiteInferior.X)
            {
                Cabeza = new Point(VentanaC.LimiteSuperior.X + 1, Cabeza.Y);
            }
            else if (Cabeza.Y <= VentanaC.LimiteSuperior.Y)
            {
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteInferior.Y - 1);
            }
            else if (Cabeza.Y >= VentanaC.LimiteInferior.Y)
            {
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteSuperior.Y + 1);
            }
        }

        private void ColisionesComida()
        {
            if (Cabeza == ComidaC.Posicion)
            {
                if (!ComidaC.GenerarComida(this))
                {
                    VentanaC.GameOver("JUEGO COMPLETADO");
                    Vivo = false;
                    return;
                }
                Comiendo = true;
                Puntaje++;
                if (Puntaje > PuntajeMaximo)
                {
                    PuntajeMaximo = Puntaje;
                }
            }
        }

        private bool ColisionesCuerpo()
        {
            foreach (Point parte in Cuerpo)
            {
                if (Cabeza == parte)
                {
                    AnimacionMuerte();
                    return true;
                }
            }
            return false;
        }

        private void AnimacionMuerte()
        {
            Console.ForegroundColor = ColorCuerpo;
            foreach (Point parte in Cuerpo)
            {
                if (parte != Cabeza)
                {
                    Console.SetCursorPosition(parte.X, parte.Y);
                    Console.WriteLine("░");
                    System.Threading.Thread.Sleep(120);
                }
            }
        }

        public void Informacion(int distancia1, int distancia2)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(VentanaC.LimiteSuperior.X + distancia1, VentanaC.LimiteSuperior.Y - 1);
            Console.WriteLine($"Puntaje: {Puntaje}  ");

            Console.SetCursorPosition(VentanaC.LimiteSuperior.X + distancia2, VentanaC.LimiteSuperior.Y - 1);
            Console.WriteLine($"Puntaje Máximo: {PuntajeMaximo}  ");
        }
    }
}