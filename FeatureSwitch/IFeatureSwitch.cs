namespace FS 
{
    /// <summary>
    ///     This is the object that checks weather a feature is 
    ///     available or not.
    /// </summary>
    public interface IFeatureSwitch
    {
        /// <summary>
        ///     Returns true the given key is an active feature.  False otherwise.
        /// </summary>
        bool this[string key] { get; }
    }
}
