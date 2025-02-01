#nullable disable

namespace BuilderGenerator
{
    /// <summary>Base class for object builder classes.</summary>
    /// <typeparam name="T">The type of the objects built by this builder.</typeparam>
    public abstract class Builder<T> where T : class
    {
        /// <summary>Builds the object instance.</summary>
        /// <returns>The constructed object.</returns>
        public abstract T Build();

        protected virtual void PostProcess(T value)
        {
        }
    }
}
