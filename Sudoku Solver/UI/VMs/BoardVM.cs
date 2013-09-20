using Sudoku_Solver.Board;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MatrixCell = Sudoku_Solver.Board.Cell;
using C = Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.UI.VMs
{
	internal class BoardVM : INotifyPropertyChanged
	{
		#region Fields

		private BoardRowVM[] rows;
		private GameMatrix board;
		#endregion

		#region Properties

		public GameMatrix Board
		{
			get { return this.board; }
			set
			{
				if (this.board != value)
				{
					this.board = value;
					LoadRows();
					OnPropertyChanged();
				}
			}
		}

		public BoardRowVM[] Rows
		{
			get { return this.rows; }
			private set
			{
				if (this.rows != value)
				{
					this.rows = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };
		#endregion

		#region Ctors

		public BoardVM() : this(new GameMatrix()) { }

		public BoardVM(GameMatrix board)
		{
			this.Board = board;
		}
		#endregion

		#region Methods

		private void LoadRows()
		{
			BoardRowVM[] rows = new BoardRowVM[C.BOARD_HEIGHT / C.SUB_MAT_HEIGHT];

			for (int rowIndex = 0; rowIndex < C.BOARD_HEIGHT / C.SUB_MAT_HEIGHT; rowIndex++)
			{
				rows[rowIndex] = new BoardRowVM(this, rowIndex);
			}

			this.Rows = rows;
		}

		protected void OnPropertyChanged(
			[CallerMemberName]string propertyName = null)
		{
			if (propertyName != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
	}
}
