using System;
using System.Collections.Generic;
using System.Linq;

// Classe representando uma cartela de bingo
public class BingoCard
{
    public int[,] Card { get; private set; } // Matriz para armazenar os números da cartela
    public bool[,] Marks { get; private set; } // Matriz para armazenar as marcações da cartela

    public BingoCard()
    {
        Card = new int[5, 5]; // Inicializa a matriz de números
        Marks = new bool[5, 5]; // Inicializa a matriz de marcações
        GenerateCard(); // Gera a cartela de bingo com números aleatórios
    }

    // Método para gerar a cartela de bingo
    private void GenerateCard()
    {
        Random random = new Random();
        for (int col = 0; col < 5; col++)
        {
            // Gera uma lista de números aleatórios para cada coluna
            List<int> columnNumbers = Enumerable.Range(1 + col * 15, 15).OrderBy(x => random.Next()).Take(5).ToList();
            for (int row = 0; row < 5; row++)
            {
                Card[row, col] = columnNumbers[row]; // Atribui o número à posição na cartela
            }
        }
        Card[2, 2] = 0; // Espaço livre no centro da cartela
        Marks[2, 2] = true; // Marca o espaço livre no centro
    }

    // Método para marcar um número na cartela
    public void MarkNumber(int number)
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (Card[row, col] == number) // Se o número estiver na cartela
                {
                    Marks[row, col] = true; // Marca o número
                }
            }
        }
    }

    // Método para verificar se a cartela tem bingo
    public bool IsBingo()
    {
        // Verifica linhas e colunas
        for (int i = 0; i < 5; i++)
        {
            if (Marks.Cast<bool>().Skip(i * 5).Take(5).All(m => m) || Marks.Cast<bool>().Where((_, index) => index % 5 == i).All(m => m))
            {
                return true;
            }
        }

        // Verifica diagonais
        if (Enumerable.Range(0, 5).All(i => Marks[i, i]) || Enumerable.Range(0, 5).All(i => Marks[i, 4 - i]))
        {
            return true;
        }

        return false;
    }

    // Método para imprimir a cartela no console
    public void PrintCard()
    {
        Console.WriteLine("B  I  N  G  O");
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (Marks[row, col])
                {
                    Console.Write("X "); // Marcações são exibidas como "X"
                }
                else
                {
                    Console.Write(Card[row, col].ToString("D2") + " "); // Exibe o número com dois dígitos
                }
            }
            Console.WriteLine();
        }
    }

}

// Classe representando o jogo de bingo
public class BingoGame
{
    public List<BingoCard> Cards { get; private set; } // Lista de cartelas no jogo
    public List<int> CalledNumbers { get; private set; } // Lista de números já chamados
    public Random RandomGenerator { get; private set; } // Gerador de números aleatórios

    public BingoGame(int numberOfPlayers)
    {
        Cards = new List<BingoCard>(); // Inicializa a lista de cartelas
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Cards.Add(new BingoCard()); // Adiciona uma nova cartela para cada jogador
        }
        CalledNumbers = new List<int>(); // Inicializa a lista de números chamados
        RandomGenerator = new Random(); // Inicializa o gerador de números aleatórios
    }

    // Método para chamar um novo número
    public void CallNumber()
    {
        int number;
        do
        {
            number = RandomGenerator.Next(1, 76); // Gera um número aleatório entre 1 e 75
        } while (CalledNumbers.Contains(number)); // Garante que o número não foi chamado antes

        CalledNumbers.Add(number); // Adiciona o número à lista de chamados
        Console.WriteLine("Número chamado: " + number); // Imprime o número chamado

        // Marca o número em todas as cartelas
        foreach (var card in Cards)
        {
            card.MarkNumber(number);
        }
    }

    // Método para verificar se há um vencedor
    public bool CheckForWinner()
    {
        foreach (var card in Cards)
        {
            if (card.IsBingo()) // Se alguma cartela tiver bingo
            {
                return true; // Há um vencedor
            }
        }
        return false; // Não há vencedor ainda
    }

    // Método para executar o jogo
    public void PlayGame()
    {
        PrintAllCards(); // Imprime as cartelas antes de iniciar o jogo
         
        while (!CheckForWinner()) // Continua chamando números até haver um vencedor
        {
            Console.WriteLine("Aperte Enter para sortear um número...");
            Console.ReadLine(); // Espera o usuário pressionar Enter
            CallNumber();
        }

        Console.WriteLine("Bingo!"); // Declara o vencedor
    }
    public void PrintAllCards()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Console.WriteLine($"Cartão {i + 1}:");
            Cards[i].PrintCard();
            Console.WriteLine();
        }
    }
}

// Classe principal para execução do programa
public class Program
{
    public static void Main()
    {
        BingoGame game = new BingoGame(2); // Cria um novo jogo de bingo com 2 jogadores
        game.PlayGame(); // Inicia o jogo
    }
}
