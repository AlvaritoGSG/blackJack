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
            Console.Clear();
            mostrarPresupuesto();
            Console.WriteLine("Reglas del juego:");
            Console.WriteLine("_________________");
            Console.WriteLine("1. Realiza tu apuesta y juega.");
            Console.WriteLine("2. El objetivo es conseguir un total de cartas igual a 21 o lo más cercano posible sin pasarse.");
            Console.WriteLine("3. Cada carta tiene un valor numérico entre 1 y 11.");
            Console.WriteLine("4. Si te pasas de 21, pierdes automáticamente.");
            Console.WriteLine("5. Si el dealer se pasa de 21, ganas automáticamente.");
            Console.WriteLine("6. Si el dealer tiene más puntos que tú, pierdes.");
            Console.WriteLine("7. Si tienes más puntos que el dealer, ganas.");
            Console.WriteLine("8. Si tienes el mismo número de puntos que el dealer, es un empate.");
            Console.WriteLine("9. Al ganar o perder, se pagan las apuestas.");
            Console.WriteLine("10. El ganador recibe x1.5 su apuesta");
            Console.WriteLine("\nPresione cualquier tecla para continuar...\n");
            Console.ReadKey();
            Console.CursorVisible = false;
            break;
        case (int)Options.jugar:
            Jugar();
            Console.WriteLine("\nPresione cualquier tecla para continuar...\n");
            Console.ReadKey();
            break;
        case (int)Options.salir:
            presupuesto = 0;
            break;
        default:
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
        // Console.WriteLine($"Cartas del Dealer: [ {cartasDealer.First()}, X ]");
        // Console.WriteLine($"Tus cartas son: [ {string.Join(" - ", cartasJugador.GetRange(0, 2))} ]");
        // Console.WriteLine("¿Quieres pedir otra carta? (s/n)");
        // string input = Console.ReadLine();
        // if (input == "n" || input == "N")
        // {
        //     cartasJugador.RemoveRange(2, cartasJugador.Count - 2);
        // }
        // else if (true)
        // {

        // }
        if (cartasJugador.Sum() > BLACKJACK)
        {
            presupuesto = presupuesto + 1.5 * apuesta;
            msg = "¡Perdiste!";
        }
        else if (cartasDealer.Sum() > BLACKJACK)
        {
            msg = "¡Ganaste!";
        }
        else if (cartasJugador.Sum() > cartasDealer.Sum())
        {
            msg = "¡Ganaste!";
        }
        else if (cartasDealer.Sum() >= cartasJugador.Sum())
        {
            msg = "¡Perdiste!";
        }
        else
        {
            msg = "¡Empate!";
        }
        Console.WriteLine($"{msg} \n Tus cartas son: [{string.Join(", ", cartasJugador)}]\n y el Dealer tiene: [{string.Join(", ", cartasDealer)}]\n {msg}");
    }
}

Boolean IniciarPartida()
{
    mostrarPresupuesto();
    Console.WriteLine("¿De cuánto será tu apuesta?");
    apuesta = int.TryParse(Console.ReadLine(), out int apuestaValue) ? apuestaValue : 0;
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
    Random random = new Random();
    // (1*4) + (2*4) + (3*3) = 21 => 4+4+3 = máx 11 cartas
    List<int> cj = new List<int>();
    while (cartasDealer.Sum() < DEALER_LIMIT && cartasDealer.Count < 11)
    {
        cartasDealer.Add(random.Next(1, 11));
        // Console.WriteLine($"Carta Dealer: {cartasDealer.Last()}. Acumulado: {cartasDealer.Sum()}");
    }
    Console.WriteLine($"Cartas del Dealer: [ {cartasDealer.First()}, X ]");
    while (cartasJugador.Sum() <= 21 && cartasJugador.Count < 11)
    {
        cartasJugador.Add(random.Next(1, 11));
        if (cartasJugador.Count >= 2)
        {
            Console.WriteLine($"Tus cartas son: [ {string.Join(" - ", cartasJugador)} ]");
            Console.WriteLine("¿Quieres pedir otra carta? (s/n)");
            string input = Console.ReadLine();
            if (input == "n" || input == "N")
            {
                return;
            }
        }
        // Console.WriteLine($"Carta Jugador: {cartasJugador.Last()}. Acumulado: {cartasJugador.Sum()}");
    }
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
