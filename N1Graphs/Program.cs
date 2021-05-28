using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N1Graphs
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			int[,] map =
			{        //0 1 2 3 4 5 6 7
								{1,1,1,1,1,1,1,1 },//0
								{1,0,0,0,1,1,1,1 },//1
								{1,1,1,0,1,1,1,1 },//2
								{1,1,0,0,1,0,0,1 },//3
								{1,1,0,0,0,0,0,1 },//4
								{1,1,1,1,1,1,1,1 },//5
						};

			AEstrela init = new AEstrela(map, new Coordenada(1, 1), new Coordenada(6, 4), podeDiagonal: true);
			var node = init.Executar();
			AEstrela.MostrarCaminhos(node);
		}
	}
}