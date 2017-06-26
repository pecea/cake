namespace Common
{
    using System;
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Isolated<T> : IDisposable
        where T : MarshalByRefObject
    {
        private AppDomain _domain;
        /// <summary>
        /// 
        /// </summary>
        public Isolated()
        {
            _domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

            var type = typeof(T);
            Value = (T)_domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }
        /// <summary>
        /// 
        /// </summary>
        public T Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_domain == null) return;
            AppDomain.Unload(_domain);
            _domain = null;
        }
    }
}