using Sudoku_Solver.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static Sudoku_Solver.Utils.GlobalConsts;
using static System.Linq.Enumerable;

namespace Sudoku_Solver.Board
{
	public sealed class Cell : INotifyPropertyChanged
	{
		#region Fields

		private int? value;
		#endregion

		#region Properties

		public bool HasValue => this.value.HasValue;

		public int? Value
		{
			get { return this.value; }
			set
			{
				if (this.value.HasValue &&
					((this.value < MIN_CELL_VALUE) || (this.value > MAX_CELL_VALUE)))
				{
					throw new ArgumentOutOfRangeException(nameof(Value));
				}

				if (this.value != value)
				{
					CancelPropertyChangeEventArgs<int?> e = new CancelPropertyChangeEventArgs<int?>(this.value, value);

					this.ValueChanging(this, e);

					if (!e.Cancel)
					{
						this.value = value;

						bool oldIsNull = !this.value.HasValue;

						this.value = value;

						this.ValueChanged(this, e);

						if (oldIsNull == value.HasValue)
						{
							this.HasValueChanged(
								this,
								new PropertyChangeEventArgs<bool>(!value.HasValue, value.HasValue));
						}
					}
				}
			}
		}

		public GameMatrix Board { get; }
		public int X { get; }
		public int Y { get; }

		public int SubMatrix
		{
			get
			{
				int i = (this.Y / SUB_MAT_HEIGHT) * SUB_MAT_HEIGHT; ;

				i += this.X / 3;

				return i;
			}
		}

		public IEnumerable<int> AvailableValues
		{
			get
			{
				IEnumerable<int> values = Range(MIN_CELL_VALUE, MAX_CELL_VALUE - MIN_CELL_VALUE + 1)
										 .Except(this.Board.Columns[this.X]);

				if (values.Any())
				{
					values = values.Except(this.Board.Rows[this.Y]);

					if (values.Any())
					{
						values = values.Except(this.Board.SubMatrixes[this.SubMatrix]);  
					}
				}

				return values.ToArray();
			}
		}
		#endregion

		#region Events

		public event EventHandler<PropertyChangeEventArgsBase<int?>> ValueChanged = (s, e) => { };
		public event EventHandler<PropertyChangeEventArgsBase<bool>> HasValueChanged = (s, e) => { };
		public event EventHandler<CancelPropertyChangeEventArgs<int?>> ValueChanging = (s, e) => { };
		public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };
		#endregion

		#region Ctor

		public Cell(GameMatrix board, int x, int y, int? value)
		{
			if (board == null)
			{
				throw new NullReferenceException(nameof(board));
			}

			if ((x < 0) || (x >= BOARD_WIDTH))
			{
				throw new ArgumentOutOfRangeException(nameof(x));
			}

			if ((y < 0) || (y >= BOARD_HEIGHT))
			{
				throw new ArgumentOutOfRangeException(nameof(y));
			}

			this.Board = board;
			this.X = x;
			this.Y = y;

			this.Value = value;
		}

		public Cell(GameMatrix board, int x, int y) 
			: this(board, x, y, null) { }
		#endregion

		#region Methods

		public override string ToString()
		{
			if (this.Value.HasValue)
			{
				return this.Value.ToString();
			}
			else
			{
				return "";
			}
		}
		#endregion
	}
}
