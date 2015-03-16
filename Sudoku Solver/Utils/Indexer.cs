using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver.Utils
{
	public abstract class IndexerBase<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		#region Fields

		protected readonly Func<TKey, TValue> getter;
		protected readonly Func<IEnumerable<TKey>> keys;
		#endregion

		#region Properties

		public TValue this[TKey key] => this.getter(key);

		public IEnumerable<TKey> Keys => this.keys();

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

		internal IndexerBase(Func<TKey, TValue> getter, Func<IEnumerable<TKey>> keys)
		{
			if (getter == null)
			{
				throw new ArgumentNullException(nameof(getter));
			}

			if (keys == null)
			{
				throw new ArgumentNullException(nameof(keys));
			}

			this.getter = getter;
			this.keys = keys;
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

		IEnumerator IEnumerable.GetEnumerator() =>
			(this as IEnumerable<KeyValuePair<TKey, TValue>>).GetEnumerator();
		#endregion
	}

	public sealed class Indexer<TKey, TValue> : IndexerBase<TKey, TValue>
	{
		#region Ctor

		public Indexer(Func<TKey, TValue> getter, Func<IEnumerable<TKey>> keys)
			: base(getter, keys) { }
		#endregion
	}

	public sealed class RWIndexer<TKey, TValue> : IndexerBase<TKey, TValue>
	{
		#region Fields

		private Action<TKey, TValue> setter;
		#endregion

		#region Properties

		public new TValue this[TKey key]
		{
			get { return base[key]; }
			set
			{
				this.setter(key, value);
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
				throw new ArgumentNullException(nameof(setter));
			}

			this.setter = setter;
		}
		#endregion
	}
}
