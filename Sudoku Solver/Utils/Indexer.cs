using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver.Utils
{
	public class Indexer<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		#region Fields

		protected Func<TKey, TValue> _getter;
		protected Func<IEnumerable<TKey>> _keys;
		#endregion

		#region Properties

		public TValue this[TKey key] { get { return _getter(key); } }

		public IEnumerable<TKey> Keys { get { return _keys(); } }

		public IEnumerable<TValue> Values
		{
			get
			{
				foreach (var key in Keys)
				{
					yield return this[key];
				}
			}
		}
		#endregion

		#region Ctor

		public Indexer(Func<TKey, TValue> getter, Func<IEnumerable<TKey>> keys)
		{
			if (getter == null)
			{
				throw new ArgumentNullException("getter");
			}

			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}

			_getter = getter;
			_keys = keys;
		}
		#endregion

		#region Methods

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			foreach (var key in Keys)
			{
				yield return new KeyValuePair<TKey, TValue>(key, this[key]);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<KeyValuePair<TKey, TValue>>).GetEnumerator();
		}
		#endregion
	}

	public class RWIndexer<TKey, TValue> : Indexer<TKey, TValue>
	{
		#region Fields

		protected Action<TKey, TValue> _setter;
		#endregion

		#region Properties

		public new TValue this[TKey key]
		{
			get { return base[key]; }
			set
			{
				_setter(key, value);
			}
		}
		#endregion

		#region CTor

		public RWIndexer(
			Func<TKey, TValue> getter,
			Action<TKey, TValue> setter,
			Func<IEnumerable<TKey>> keys)
			: base(getter, keys)
		{
			if (setter == null)
			{
				throw new ArgumentNullException("setter");
			}

			_setter = setter;
		}
		#endregion
	}
}
