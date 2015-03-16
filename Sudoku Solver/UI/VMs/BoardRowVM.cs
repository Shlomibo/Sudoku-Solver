using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.UI.VMs
{
	internal sealed class BoardRowVM
	{
		#region Fields

		private readonly BoardVM board;
		private readonly int rowIndex;
		private SubMatrixVM[] subMatrix;
		#endregion

		#region Properties

		public SubMatrixVM[] SubMatrix
		{
			get
			{
				if (this.subMatrix == null)
				{
					this.subMatrix = LoadSubMatrix();
				}

				return this.subMatrix;
			}
		}
		#endregion

		#region Ctor

		public BoardRowVM(BoardVM boardVM, int rowIndex)
		{
			if (boardVM == null)
			{
				throw new NullReferenceException(nameof(boardVM));
			}

			if ((rowIndex < 0) || (rowIndex >= BOARD_HEIGHT))
			{
				throw new ArgumentOutOfRangeException(nameof(rowIndex));
			}

			this.board = boardVM;
			this.rowIndex = rowIndex;
		} 
		#endregion

		#region Methods

		private SubMatrixVM[] LoadSubMatrix()
		{
			var subMarices = new SubMatrixVM[BOARD_WIDTH / SUB_MAT_WIDTH];

			for (int subMatIndex = 0; subMatIndex < BOARD_WIDTH / SUB_MAT_WIDTH; subMatIndex++)
			{
				subMarices[subMatIndex] = new SubMatrixVM(this.board, this.rowIndex, subMatIndex);
			}

			return subMarices;
		} 
		#endregion
	}
}
