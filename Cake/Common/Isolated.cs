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
            Logger.Log(LogLevel.Trace, "Creating app domain ...");

            _domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

            var type = typeof(T);
            Value = (T)_domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
            Logger.Log(LogLevel.Trace, "App domain created");

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
            Logger.Log(LogLevel.Trace, "Disposing of app domain ...");
            if (_domain == null) return;
            AppDomain.Unload(_domain);
            _domain = null;

            Logger.Log(LogLevel.Trace, "App domain disposed");
        }
    }
}