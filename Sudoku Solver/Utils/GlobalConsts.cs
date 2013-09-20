using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver.Utils
{
	internal static class GlobalConsts
	{
		public const int MIN_CELL_VALUE = 1;
		public const int MAX_CELL_VALUE = 9;

		public const int BOARD_HEIGHT = 9;
		public const int BOARD_WIDTH = 9;

		public const int SUB_MAT_COUNT = 9;
		public const int SUB_MAT_HEIGHT = 3;
		public const int SUB_MAT_WIDTH = 3;
	}
}
