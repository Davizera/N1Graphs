using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N1Graphs
{
	internal class AEstrela
	{
		/// <summary>
		/// Onde a matrix utilizada será guardada.
		/// </summary>
		private int[,] _mapa;

		/// <summary>
		/// Guarda o valor do <see cref="Node"/> inicial.
		/// </summary>
		private Node _noInicial;

		/// <summary>
		/// Mantém em rastreio os nós que ainda não foram visitados e são possíveis candidatos a serem o melhor caminho.
		/// </summary>
		private List<Node> _listaAberta;

		/// <summary>
		/// Mantém o rastreio do melhor caminho, ou seja, após um <see cref="Node"/> ser analisado, caso ele seja um bom caminho será adicionado a essa lista.
		/// </summary>
		private List<Node> _listaFechada;

		/// <summary>
		/// Guarda o valor do ponto final na matrix.
		/// </summary>
		private Coordenada _pontoFinal;

		/// <summary>
		/// Guarda o valor referente ao terreno, se ele é andavel ou não.
		/// </summary>
		/// <remarks>
		/// Esse valor pode ser invertido no momento de construção da classe, através do <argum
		/// </remarks>
		private TiposTerreno ehAndavel;

		/// <summary>
		/// Define se é possível ou não caminhar na diagonal.
		/// </summary>
		/// <remarks>
		/// Cuidado, o resultado pode diferir caso andar na diagonal não seja permitido!
		/// </remarks>
		private bool _podeDiagonal;

		/// <summary>
		/// Constrói a entidade com os argumentos necessário.
		/// </summary>
		/// <param name="mapa">Matrix utilizada no problema</param>
		/// <param name="inicial"><see cref="Coordenada"/> inicial, ou seja, <see cref="Node"/> de onde se sai.</param>
		/// <param name="final"><see cref="Coordenada"/>, ou seja, o <see cref="Node"/> final do problema.</param>
		/// <param name="podeDiagonal">Define se será possível se mover na diagonal.</param>
		/// <param name="andavel">Define qual valor será um obstáculo ou não. Você pode inverter, caso queira!</param>
		public AEstrela(int[,] mapa, Coordenada inicial, Coordenada final, bool podeDiagonal = false, TiposTerreno andavel = TiposTerreno.Andavel)
		{
			this._mapa = mapa;
			this._noInicial = new Node(inicial); //Set a posição do primeiro Node
			this._pontoFinal = final;
			this.ehAndavel = andavel;
			this._podeDiagonal = podeDiagonal;
		}

		/// <summary>
		/// Executa o algoritmo A*.
		/// </summary>
		/// <returns>Um <see cref="Node"/> caso ele seja alcançavel e <c>Null</c>, se contrário.</returns>
		public Node Executar()
		{
			this._listaAberta = new List<Node>();
			this._listaFechada = new List<Node>();

			//Outros valores não foram setados, pois já ficam com o tipo correto para o primeiro nó.
			//Exemplo: Node.Anterior = null para o primeiro nó e esse é o valor padrão para um objeto não referenciado.
			this._noInicial.CustoG = 0; //A distância dele pra ele mesmo é 0;
			this._noInicial.CustoH = ManhattanDistance(this._noInicial);
			this._noInicial.CustoF = this._noInicial.CustoG + this._noInicial.CustoH;
			this._listaAberta.Add(this._noInicial);

			while (_listaAberta.Count() > 0)
			{
				Node NoComMinCustoF = GetNoMinCustoF(this._listaAberta);
				Coordenada ponto = NoComMinCustoF.Ponto;

				if (this._pontoFinal.Equals(NoComMinCustoF.Ponto))
					return NoComMinCustoF;
				//Ao retornar esse nó teremos como fazer um traceback pra saber quais os nós anteriores, isso só é possível pela variável Anterior, declarada na classe Node.

				this._listaAberta.Remove(NoComMinCustoF);
				this._listaFechada.Add(NoComMinCustoF);

				AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x - 1, NoComMinCustoF.Ponto.y), NoComMinCustoF); //esquerda
				AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x + 1, NoComMinCustoF.Ponto.y), NoComMinCustoF); //direita
				AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x, NoComMinCustoF.Ponto.y - 1), NoComMinCustoF); //cima
				AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x, NoComMinCustoF.Ponto.y + 1), NoComMinCustoF); //baixo
				if (_podeDiagonal)
				{
					AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x - 1, NoComMinCustoF.Ponto.y - 1), NoComMinCustoF);
					AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x + 1, NoComMinCustoF.Ponto.y - 1), NoComMinCustoF);
					AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x - 1, NoComMinCustoF.Ponto.y + 1), NoComMinCustoF);
					AdicionaNaListaAberta(new Coordenada(NoComMinCustoF.Ponto.x + 1, NoComMinCustoF.Ponto.y + 1), NoComMinCustoF);
				}
			}
			return null;
		}

		private void AdicionaNaListaAberta(Coordenada ponto, Node anterior)
		{
			if (EhAndavel(ponto) && !this._listaFechada.Contains(new Node(ponto)))
			{
				var node = new Node(ponto, anterior);
				node.CustoG = anterior.CustoG + 1; //Definição cada nó está a 1 uni. de distância.
				node.CustoH = ManhattanDistance(node);
				node.CustoF = node.CustoG + node.CustoH;

				if (this._listaAberta.Contains(node))
				{
					if (node.CustoF < this._listaAberta.Where(no => no.Equals(node)).First().CustoF)
					{
						this._listaAberta.Where(no => no.Equals(node)).First().CustoF = node.CustoF;
					}
				}
				else
				{
					this._listaAberta.Add(node);
				}
			}
		}

		private bool EhAndavel(Coordenada ponto)
		{
			return this._mapa[ponto.y, ponto.x] == (int)this.ehAndavel;
		}

		/// <summary>
		/// Pega o <see cref="Node"/> com o menor custo de F.
		/// </summary>
		/// <param name="listaAberta"></param>
		/// <returns>O <see cref="Node"/> com o menor custo de F.</returns>
		private Node GetNoMinCustoF(List<Node> listaAberta)
		{
			Node minCustoF = null;

			foreach (var node in listaAberta)
			{
				if (minCustoF == null || node.CustoF < minCustoF.CustoF)
				{
					minCustoF = node;
				}
			}
			return minCustoF;
		}

		/// <summary>
		/// Calcula a distância usando a fórmula de Manhattan.
		/// Essa foi a heurística utilizada para esse algoritmo.
		/// </summary>
		/// <returns><see cref="Int"/> resultado da distância de Manhattan.</returns>
		private int ManhattanDistance(Node current)
		{
			return Math.Abs(current.Ponto.x - this._pontoFinal.x) + Math.Abs(current.Ponto.y - this._pontoFinal.y);
		}

		public static void MostrarCaminhos(Node noFinal, int[,] mapa, AEstrela config)
		{
			var mapaVisual = MontarCaminho(mapa);
			string andavel, obstaculo;

			if (config.ehAndavel == 0)
			{
				andavel = "O";
				obstaculo = "X";
			}
			else
			{
				andavel = "X";
				obstaculo = "O";
			}
			Console.WriteLine($"Camino do algoritmo é feito pelo *, espaços andavéis {andavel} e obstáculos {obstaculo}");

			while (noFinal != null)
			{
				//Console.WriteLine($"X:{noFinal.Ponto.x}; Y:{noFinal.Ponto.y}");
				mapaVisual[noFinal.Ponto.y, noFinal.Ponto.x] = "*";
				noFinal = noFinal.Anterior;
			}

			for (int i = 0; i < mapaVisual.GetLength(0); i++)
			{
				for (int j = 0; j < mapaVisual.GetLength(1); j++)
				{
					Console.Write($"{mapaVisual[i, j]} ");
				}
				Console.WriteLine();
			}
		}

		private static string[,] MontarCaminho(int[,] mapa)
		{
			string[,] mapaVisual = new string[mapa.GetLength(0), mapa.GetLength(1)];
			for (int i = 0; i < mapa.GetLength(0); i++)
			{
				for (int j = 0; j < mapa.GetLength(1); j++)
				{
					if (mapa[i, j] != 0)
					{
						mapaVisual[i, j] = "X";
					}
					else
					{
						mapaVisual[i, j] = "O";
					}
				}
			}
			return mapaVisual;
		}
	}

	/// <summary>
	/// Um nó é uma definição que usamos para definir um entidade com determinados valores.
	/// </summary>
	internal class Node
	{
		public Node(Coordenada locaizacao, Node anterior) : this(locaizacao)
		{
			this.Anterior = anterior;
		}

		public Node(Coordenada localizacao)
		{
			this.Ponto = localizacao;
		}

		public override bool Equals(Object obj)
		{
			if ((obj == null) || !this.GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				Node no = (Node)obj;
				return this.Ponto.Equals(no.Ponto);
			}
		}

		/// <summary>
		/// É a soma do <see cref="CustoG"/> com o <see cref="CustoH"/>, dessa forma o algoritmo consegue penalizar quem tem um maior custo.
		/// </summary>
		public int CustoF { get; set; }

		/// <summary>
		/// Custo do nó inicial até o que está sendo analisado
		/// </summary>
		public int CustoG { get; set; }

		/// <summary>
		/// Custo da Heurística utilizada
		/// </summary>
		/// <remarks>
		/// Existem diversar heurísticas, são alguns exemplos delas: distância euclidiana, distância de mahattan entre outros.
		/// </remarks>
		public int CustoH { get; set; }

		/// <summary>
		/// Guada o valor do <see cref="Node"/> anterior. Com isso nós conseguimos pegar o caminho percorrido pelo algorimto.
		/// </summary>
		public Node Anterior { get; set; }

		public Coordenada Ponto { get; set; }
	}

	/// <summary>
	/// Define a localização do ponto.
	/// </summary>
	internal class Coordenada
	{
		/// <summary>
		/// Váriveis no plano X e Y, respectivamente.
		/// </summary>
		public int x, y;

		public Coordenada(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Compara se um ponto é igual ao <paramref name="ponto"/>
		/// </summary>
		/// <param name="ponto"></param>
		/// <returns><c>True</c> se pontos forem iguais<c>False</c>, o contrário.</returns>
		public override bool Equals(Object ponto)
		{
			if ((ponto == null) || !this.GetType().Equals(ponto.GetType()))
			{
				return false;
			}
			else
			{
				Coordenada p = (Coordenada)ponto;
				return (x == p.x) && (y == p.y);
			}
		}
	}

	/// <summary>
	/// Define quais valores de terrenos temos.
	/// </summary>
	internal enum TiposTerreno
	{
		Andavel = 0,
		Obstaculo = 1
	}
}