using Sudoku_Solver.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver.Utils
{
	public class PropertyChangeEventArgs<T> : EventArgs
	{
		#region Properties

		public T OldValue { get; private set; }
		public T NewValue { get; private set; }
		#endregion

		#region Ctor

		public PropertyChangeEventArgs(
			T oldValue,
			T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
		#endregion
	}

	public class CancelPropertyChangeEventArgs<T> : PropertyChangeEventArgs<T>
	{
		#region Properties

		public bool Cancel { get; set; }
		#endregion

		#region Ctor

		public CancelPropertyChangeEventArgs(
			T oldValue,
			T newValue)
			: base(oldValue, newValue)
		{
			Cancel = false;
		}
		#endregion
	}

	public class CellEventArgs : EventArgs
	{
		public Cell Cell { get; set; }
	}
}
