using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.UI.VMs
{
	internal sealed class SubMatrixVM
	{
		#region Fields

		private readonly BoardVM board;
		private readonly int rowIndex;
		private readonly int subMatIndex;
		private SubMatrixRowVM[] subMatrixRows; 
		#endregion

		#region Properties

		public SubMatrixRowVM[] SubMatrixRows
		{
			get
			{
				if (this.subMatrixRows == null)
				{
					this.subMatrixRows = LoadRows();
				}

				return this.subMatrixRows;
			}
		}
		#endregion

		#region Ctor

		public SubMatrixVM(BoardVM board, int rowIndex, int subMatIndex)
		{
			this.board = board;
			this.rowIndex = rowIndex;
			this.subMatIndex = subMatIndex;
		} 
		#endregion

		#region Methods

		private SubMatrixRowVM[] LoadRows()
		{
			var rows = new SubMatrixRowVM[SUB_MAT_HEIGHT];

			for (int rowIndex = 0; rowIndex < SUB_MAT_HEIGHT; rowIndex++)
			{
				rows[rowIndex] = new SubMatrixRowVM(
					this.board,
					this.rowIndex,
					this.subMatIndex,
					rowIndex);
			}

			return rows;
		}
		#endregion
	}
}
