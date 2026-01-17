int totalJugador;
int totalDealer;
string msg = "";
const int limit = 21;

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