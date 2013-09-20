using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C = Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.UI.VMs
{
	class SubMatrixVM
	{
		#region Fields

		private BoardVM board;
		private int rowIndex;
		private int subMatIndex;
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
			// TODO: Complete member initialization
			this.board = board;
			this.rowIndex = rowIndex;
			this.subMatIndex = subMatIndex;
		} 
		#endregion

		#region Methods

		private SubMatrixRowVM[] LoadRows()
		{
			SubMatrixRowVM[] rows = new SubMatrixRowVM[C.SUB_MAT_HEIGHT];

			for (int rowIndex = 0; rowIndex < C.SUB_MAT_HEIGHT; rowIndex++)
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
