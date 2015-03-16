using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.UI.VMs
{
	internal sealed class SubMatrixRowVM
	{
		#region Fields

		private readonly BoardVM board;
		private readonly int rowIndex;
		private readonly int subMatIndex;
		private readonly int subMatRowIndex;
		private CellVM[] cells; 
		#endregion

		#region Properties

		public CellVM[] Cells
		{
			get
			{
				if (this.cells == null)
				{
					this.cells = LoadCells();
				}

				return this.cells;
			}
		}
		#endregion

		#region Ctor

		public SubMatrixRowVM(BoardVM board, int rowIndex, int subMatIndex, int subMatRowIndex)
		{
			this.board = board;
			this.rowIndex = rowIndex;
			this.subMatIndex = subMatIndex;
			this.subMatRowIndex = subMatRowIndex;
		} 
		#endregion

		#region Methods

		private CellVM[] LoadCells()
		{
			var cells = new CellVM[SUB_MAT_WIDTH];

			for (int cellIndex = 0; cellIndex < SUB_MAT_WIDTH; cellIndex++)
			{
				cells[cellIndex] = new CellVM(
					this.board,
					this.rowIndex,
					this.subMatIndex,
					this.subMatRowIndex,
					cellIndex);
			}

			return cells;
		}
		#endregion
	}
}
