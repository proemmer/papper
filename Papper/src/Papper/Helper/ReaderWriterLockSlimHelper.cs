using System;
using System.Threading;

namespace Papper.Helper
{
    public class ReaderGuard : IDisposable
    {
        private readonly ReaderWriterLockSlim _readerWriterLock;
        public ReaderGuard(ReaderWriterLockSlim readerWriterLock)
        {
            _readerWriterLock = readerWriterLock;
            _readerWriterLock.EnterReadLock();
        }
        public void Dispose()
        {
            _readerWriterLock.ExitReadLock();
        }
    }


    public class WriterGuard : IDisposable
    {
        private ReaderWriterLockSlim _readerWriterLock;
        private bool IsDisposed { get { return _readerWriterLock == null; } }
        public WriterGuard(ReaderWriterLockSlim readerWriterLock)
        {
            _readerWriterLock = readerWriterLock;
            _readerWriterLock.EnterWriteLock();
        }
        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.ToString());
            _readerWriterLock.ExitWriteLock();
            _readerWriterLock = null;
        }
    }


    public class UpgradeableGuard : IDisposable
    {

        private class UpgradedGuard : IDisposable
        {
            private UpgradeableGuard _parentGuard;
            private WriterGuard _writerLock;
            public UpgradedGuard(UpgradeableGuard parentGuard)
            {
                _parentGuard = parentGuard;
                _writerLock = new WriterGuard(_parentGuard._readerWriterLock);
            }
            public void Dispose()
            {
                _writerLock.Dispose();
                _parentGuard._upgradedLock = null;
            }
        }

        private readonly ReaderWriterLockSlim _readerWriterLock;
        private UpgradedGuard _upgradedLock;
        public UpgradeableGuard(ReaderWriterLockSlim readerWriterLock)
        {
            _readerWriterLock = readerWriterLock;
            _readerWriterLock.EnterUpgradeableReadLock();
        }
        public IDisposable UpgradeToWriterLock()
        {
            if (_upgradedLock == null)
            {
                _upgradedLock = new UpgradedGuard(this);
            }
            return _upgradedLock;
        }
        public void Dispose()
        {
            if (_upgradedLock != null)
            {
                _upgradedLock.Dispose();
            }
            _readerWriterLock.ExitUpgradeableReadLock();
        }
    }


}
