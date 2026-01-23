using System.IO.Pipes;
using System.Security.Cryptography.X509Certificates;

double presupuesto = 1000;
const int BLACKJACK = 21;
const int DEALER_LIMIT = 17;
// const string palos[] = ["♠", "♥", "♦", "♣"];
double apuesta;
List<int> cartasJugador = new List<int>();
List<int> cartasDealer = new List<int>();
string msg = "";
do
{
    mostrarPresupuesto();
    int optionSelected = mainMenu();
    switch (optionSelected - 1)
    {
        case (int)Options.verReglas:
            mostrarPresupuesto();
            Console.Clear();
            Console.WriteLine("¡Apuesta y Gana en el BlackJack!");
            Console.WriteLine("________________________________");
            Console.WriteLine("⚡ Reglas de Oro");
            Console.WriteLine("   - Tú vs. el Dealer: Gana quien tenga la puntuación más alta.");
            Console.WriteLine("   - El límite: Si sumas más de 21, ¡quedas fuera! Si el dealer se pasa, tú ganas.");
            Console.WriteLine("   - Cartas: Valen de 1 a 11 puntos.");
            Console.WriteLine("💰 El Botín");
            Console.WriteLine("   - Empate: Nadie pierde, recuperas tu apuesta.");
            Console.WriteLine("   - ¡Victoria!: Te llevas 1:1 lo que apostaste.\n     (Si ganas con 21 en la primera mano, te llevas x1.5 tu apuesta).");
            Console.WriteLine("\nPresione cualquier tecla para continuar...\n");
            Console.ReadKey();
            Console.CursorVisible = false;
            break;
        case (int)Options.jugar:
            mostrarPresupuesto();
            Jugar();
            break;
        case (int)Options.salir:
            presupuesto = 0;
            break;
        default:
            mostrarPresupuesto();
            Console.WriteLine("Opción no válida.");
            break;
    }
} while (presupuesto > 0);
Console.WriteLine($"\nParece que tu presupuesto se ha agotado. Gracias por visitar el casino.\n");

int mainMenu()
{
    Console.Clear();
    Console.WriteLine("\n===================================");
    Console.WriteLine("Bienvenido al juego de BlackJack");
    Console.WriteLine("--------------------------------");
    Console.WriteLine("  1. Ver reglas");
    Console.WriteLine("  2. Jugar");
    Console.WriteLine("  5. Salir");
    Console.WriteLine("--------------------------------");
    mostrarPresupuesto();
    Console.WriteLine("===================================\n");
    int optSel = int.TryParse(Console.ReadLine(), out int option) ? option : 0;
    return optSel;
}

void Jugar()
{
    if (
    IniciarPartida()
    )
    {
        Console.Clear();
        RepartirCartas();
        if (cartasJugador.Sum() > BLACKJACK)
        {
            Console.WriteLine($" El Dealer tiene: [ {cartasDealer.First()} , X ]\n Y tus cartas son: [ {string.Join(" , ", cartasJugador)} ], Total: {cartasJugador.Sum()}");
            msg = "¡Perdiste!";
        }
        else
        {
            if (cartasDealer.Sum() > BLACKJACK)
            {
                presupuesto = presupuesto + apuesta + 1 * apuesta;
                msg = "¡Ganaste!";
            }
            else if (cartasJugador.Sum() > cartasDealer.Sum())
            {
                if (cartasJugador.Count() == 2 && cartasJugador.Sum() == 21)
                {
                    presupuesto = presupuesto + apuesta + 1.5 * apuesta;
                    msg = "¡Espectacular! Ganaste de mano.";
                }
                else
                {
                    presupuesto = presupuesto + apuesta + 1 * apuesta;
                    msg = "¡Ganaste!";
                }
            }
            else if (cartasDealer.Sum() > cartasJugador.Sum())
            {
                msg = "¡Perdiste!";
            }
            else
            {
                msg = "¡Empate!";
                presupuesto += apuesta;
            }
            Console.WriteLine($"Tus cartas son:\n     [ {string.Join(" , ", cartasJugador)} ], Total: {cartasJugador.Sum()}.\nY las cartas del Dealer son:\n     [ {string.Join(" , ", cartasDealer)} ], Total: {cartasDealer.Sum()}");
        }
        Console.WriteLine($"\n {msg.ToUpper()}!!");
        Console.ReadKey();
    }
}

Boolean IniciarPartida()
{
    mostrarPresupuesto();
    Console.WriteLine("¿De cuánto será tu apuesta?");
    while (!double.TryParse(Console.ReadLine(), out apuesta))
    {
        Console.WriteLine("Por favor, introduce un número natural.");
    }
    if (apuesta > presupuesto)
    {
        Console.WriteLine("No tienes suficiente presupuesto para esa apuesta.");
        Console.ReadKey();
        return false;
    }
    else if (apuesta <= 0)
    {
        Console.WriteLine("La apuesta debe ser mayor que cero.");
        Console.ReadKey();
        return false;
    }
    presupuesto -= apuesta;
    return true;
}

void RepartirCartas()
{
    cartasDealer.Clear();
    cartasJugador.Clear();
    Random random = new Random();
    List<int> cj = new List<int>();
    while (cartasDealer.Sum() < DEALER_LIMIT)
    {
        cartasDealer.Add(random.Next(1, 11));
        // Console.WriteLine($"Carta Dealer: {cartasDealer.Last()}. Acumulado: {cartasDealer.Sum()}");
    }
    Console.WriteLine($"Cartas del Dealer: [ {cartasDealer.First()} , X ]\n");
    cartasJugador.Add(random.Next(1, 11));
    cartasJugador.Add(random.Next(1, 11));
    Console.WriteLine($"Tus cartas son: [ {string.Join(" , ", cartasJugador)} ], Total: {cartasJugador.Sum()}\n");
    List<string> posiblesRespuestasAfirmativas = new List<string> { "S", "Sí", "Si", "Yes", "Y", "Oui", "Sipi", "Ya", "Símon", "Simon", "Ok", "Dame", "s", "sí", "si", "yes", "y", "oui", "sipi", "ya", "símon", "simon", "ok", "dame" };
    List<string> posiblesRespuestasNegativas = new List<string> { "N", "No", "Nope", "Nones", "Negativo", "Me planto!", "Paso", "Plantarme", "P", "Nada", "n", "no", "nope", "nones", "negativo", "me planto!", "paso", "plantarme", "p", "nada" };
    string respuesta;
    while (cartasJugador.Sum() < 21)
    {
        do
        {
            Console.WriteLine("¿Quieres pedir otra carta? (s/n)");
            respuesta = Console.ReadLine()?.Trim().ToLower() ?? "";
            if (string.IsNullOrWhiteSpace(respuesta))
            {
                Console.WriteLine($"¡La respuesta no puede estar vacía!");
            }
            else
            {
                Console.WriteLine($"Por favor, responde:\n ({string.Join(", ", posiblesRespuestasAfirmativas.GetRange(0, posiblesRespuestasAfirmativas.Count / 2))}) o:\n ({string.Join(", ", posiblesRespuestasNegativas.GetRange(0, posiblesRespuestasNegativas.Count / 2))}).");
            }
        } while (string.IsNullOrWhiteSpace(respuesta) ||
                (!posiblesRespuestasNegativas.Contains(respuesta) && !posiblesRespuestasAfirmativas.Contains(respuesta)));
        if (posiblesRespuestasAfirmativas.Contains(respuesta))
        {
            cartasJugador.Add(random.Next(1, 11));
            Console.Clear();
            Console.WriteLine($"Cartas del Dealer: [ {cartasDealer.First()} , X ]\n");
            Console.WriteLine($"Tus cartas son: [ {string.Join(" , ", cartasJugador)} ], Total: {cartasJugador.Sum()}\n");
        }
        else
        {
            break;
        }
    }
    Console.Clear();
}

void mostrarPresupuesto()
{
    Console.WriteLine($"\nTu presupuesto actual es: {presupuesto}\n");
}
enum Options
{
    verReglas,
    jugar,
    pedir,
    plantarse,
    salir
}
