using Sudoku_Solver.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver.Utils
{
	public abstract class PropertyChangeEventArgsBase<T> : EventArgs
	{
		#region Properties

		public T OldValue { get; }
		public T NewValue { get; }
		#endregion

		#region Ctor

		internal PropertyChangeEventArgsBase(
			T oldValue,
			T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
		#endregion
	}

	public sealed class PropertyChangeEventArgs<T> : PropertyChangeEventArgsBase<T>
	{
		public PropertyChangeEventArgs(T oldValue, T newValue)
			: base(oldValue, newValue) { }
    }

	public sealed class CancelPropertyChangeEventArgs<T> : PropertyChangeEventArgsBase<T>
	{
		#region Properties

		public bool Cancel { get; set; } = false;
		#endregion

		#region Ctor

		public CancelPropertyChangeEventArgs(
			T oldValue,
			T newValue)
			: base(oldValue, newValue)
		{ }
		#endregion
	}

	public class CellEventArgs : EventArgs
	{
		public Cell Cell { get; }

		public CellEventArgs(Cell cell)
		{
			this.Cell = cell;
		}
	}
}
