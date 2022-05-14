using System;
using System.Collections.Generic;
using System.Threading;

namespace Framework.DynamicProxy
{
    internal sealed class SynchronizedDictionary<TKey, TValue> : IDisposable
    {
		private Dictionary<TKey, TValue> items;

		/// <summary>
		/// 读写锁 单线程写入,多线程读取,比传统lock monitor性能更好(不管读写,都采用线程独占)
		/// </summary>
		private ReaderWriterLockSlim itemsLock;

		public SynchronizedDictionary()
		{
			items = new Dictionary<TKey, TValue>();
			itemsLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
		}

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
		{
			TValue value;
			itemsLock.EnterReadLock();
			try
			{
				if (items.TryGetValue(key,out value))
				{
					return value;
				}
			}
			finally
			{
				itemsLock.ExitReadLock();
			}

			itemsLock.EnterUpgradeableReadLock();

			try
			{
				if (items.TryGetValue(key, out value))
				{
					return value;
				}
				else
				{
					itemsLock.EnterWriteLock();

					try
					{
						value = valueFactory.Invoke(key);

						items.Add(key, value);
					}
					finally
					{
						itemsLock.ExitWriteLock();
					}


				}
			}
			finally
			{
				itemsLock.ExitUpgradeableReadLock();
			}
			return value;
		}

		public void Dispose()
		{
			itemsLock.Dispose();
		}
	}
}
