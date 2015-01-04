namespace Cake.Tests
{
    using System;

    public class Isolated<T> : IDisposable 
        where T : MarshalByRefObject
    {
        private AppDomain domain;

        private readonly T value;

        public Isolated()
        {
            domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

            var type = typeof(T);
            value = (T)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Value
        {
            get { return value; }
        }

        public void Dispose()
        {
            if (domain != null)
            {
                AppDomain.Unload(domain);
                domain = null;
            }
        }
    }
}