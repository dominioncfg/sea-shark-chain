namespace SeaSharkChain.Core
{
    public class TransactionPool<T>
    {
        private readonly Queue<ITransaction<T>> _queue;

        public TransactionPool() => _queue = new Queue<ITransaction<T>>();

        public void AddTransaction(ITransaction<T> transaction) => _queue.Enqueue(transaction);

        public ITransaction<T> GetTransaction() => _queue.Dequeue();
    }
}
