using System;
using System.Threading;

namespace HTTPlease.Core.Utilities
{
	/// <summary>
    ///		A base class for disposable objects.
    /// </summary>
    public abstract class DisposableObject
        : IDisposable
    {
		/// <summary>
		///		Is the object being disposed?
		/// </summary>
		/// <remarks>
		///		1, if the object is being disposed; otherwise, 0.
		/// </remarks>
		int		_isDisposing;

        /// <summary>
        ///		Has the object been disposed?
        /// </summary>
        bool	_isDisposed;

        /// <summary>
        ///		Create a new disposable object.
        /// </summary>
        protected DisposableObject()
        {
        }

        /// <summary>
        ///		Finaliser for <see cref="DisposableObject"/>.
        /// </summary>
		~DisposableObject()
        {
			// Don't call disposal implementation more than once.
	        if (_isDisposed)
		        return;

			int wasDisposing = Interlocked.Exchange(ref _isDisposing, 1);
			if (wasDisposing == 1)
				return;

	        try
	        {
				Dispose(false);
	        }
	        finally
	        {
		        _isDisposed = true;
		        _isDisposing = 0;
	        }
        }

        /// <summary>
        ///		Dispose of resources being used by the object.
        /// </summary>
        public void Dispose()
        {
			// Don't call disposal implementation more than once.
			if (_isDisposed)
				return;

	        int wasDisposing = Interlocked.Exchange(ref _isDisposing, 1);
	        if (wasDisposing == 1)
		        return;

	        try
	        {
		        Dispose(true);
		        GC.SuppressFinalize(this);
	        }
	        finally
	        {
				_isDisposed = true;
		        _isDisposing = 0;
	        }
        }

        /// <summary>
        ///		Dispose of resources being used by the object.
        /// </summary>
        /// <param name="disposing">
        ///		Explicit disposal?
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
        }

	    /// <summary>
	    ///		Has the object been disposed?
	    /// </summary>
	    protected bool IsDisposed => _isDisposed;

		/// <summary>
        ///		Check if the object has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        ///		The object has been disposed.
        /// </exception>
        protected void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

		/// <summary>
		///		Determine if a <see cref="DisposableObject"/> has been disposed.
		/// </summary>
		/// <param name="disposableObject">
		///		The <see cref="DisposableObject"/> to examine.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the object has been disposed; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="disposableObject"/> is <c>null</c>.
		/// </exception>
		public static bool IsObjectDisposed(DisposableObject disposableObject)
		{
			if (disposableObject == null)
				throw new ArgumentNullException(nameof(disposableObject));

			return disposableObject.IsDisposed;
		}
    }
}