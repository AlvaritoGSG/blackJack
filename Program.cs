using System.Security.Cryptography.X509Certificates;

int totalJugador;
int totalDealer;
string msg = "";
const int limit = 21;
int presupuesto = 1000;

do
{
    int optionSelected = mainMenu();
    switch (optionSelected - 1)
    {
        case (int)Options.verReglas:
            // MostrarReglas();
            Console.WriteLine("Reglas del juego:");
            Console.WriteLine("1. El objetivo es conseguir un total de cartas igual a 21 o lo más cercano posible sin pasarse.");
            Console.WriteLine("2. Cada carta tiene un valor numérico entre 1 y 11.");
            Console.WriteLine("3. Si te pasas de 21, pierdes automáticamente.");
            Console.WriteLine("4. Si el dealer se pasa de 21, ganas automáticamente.");
            Console.WriteLine("5. Si el dealer tiene más puntos que tú, pierdes.");
            Console.WriteLine("6. Si tienes más puntos que el dealer, ganas.");
            break;
        case (int)Options.jugar:
            Jugar();
            break;
        case (int)Options.salir:
            presupuesto = 0;
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
} while (presupuesto > 0);
Console.WriteLine($"Parece que tu presupuesto se ha agotado. Gracias por visitar el casino.");
Console.WriteLine($"Parece que tu presupuesto se ha agotado. Gracias por visitar el casino.");
Console.WriteLine($"Parece que tu presupuesto se ha agotado. Gracias por visitar el casino.");

int mainMenu()
{
    Console.WriteLine("================================\n");
    Console.WriteLine("Bienvenido al juego de BlackJack\n");
    Console.WriteLine("1. Ver reglas");
    Console.WriteLine("2. Jugar");
    Console.WriteLine("5. Salir");
    Console.WriteLine("================================\n");
    int optionSelected = int.TryParse(Console.ReadLine(), out int option) ? option : 0;
    return optionSelected;
}

void Jugar()
{
    Random random = new Random();
    int card1Player = random.Next(1, 11);
    int card2Player = random.Next(1, 11);
    int card1Dealer = random.Next(1, 11);
    int card2Dealer = random.Next(1, 11);

    totalJugador = card1Player + card2Player;
    totalDealer = card1Dealer + card2Dealer;

    if (totalJugador > limit)
    {
        msg = "¡Perdiste!";
    }
    else if (totalDealer > limit)
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



enum Options
{
    verReglas,
    jugar,
    pedir,
    plantarse,
    salir
}
