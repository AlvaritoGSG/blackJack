using System.IO.Pipes;
using System.Security.Cryptography.X509Certificates;

int totalJugador;
int totalDealer;
string msg = "";
const int BLACKJACK = 21;
const int DEALER_LIMIT = 17;
double presupuesto = 1000;
double apuesta;
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
        Random random = new Random();
        int card1Player = random.Next(1, 11);
        int card2Player = random.Next(1, 11);
        int card1Dealer = random.Next(1, 11);
        int card2Dealer = random.Next(1, 11);

        totalJugador = card1Player + card2Player;
        totalDealer = card1Dealer + card2Dealer;

        if (totalJugador > BLACKJACK)
        {
            presupuesto = presupuesto + 1.5 * apuesta;
            msg = "¡Perdiste!";
        }
        else if (totalDealer > BLACKJACK)
        {
            msg = "¡Ganaste!";
        }
        else if (totalJugador > totalDealer)
        {
            msg = "¡Ganaste!";
        }
        else if (totalDealer >= totalJugador)
        {
            msg = "¡Perdiste!";
        }
        else
        {
            msg = "¡Empate!";
        }
        Console.WriteLine($"{msg} \n Tus cartas son: {card1Player} y {card2Player}\n y el dealer tiene: {card1Dealer} y {card2Dealer}\n {msg}");
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
