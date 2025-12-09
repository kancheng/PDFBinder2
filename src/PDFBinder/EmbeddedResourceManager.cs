
using System;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.ComponentModel;
using System.Threading;

namespace PDFBinder
{
    /// <summary>
    /// Custom ResourceManager that loads resources from embedded resources in the main assembly
    /// instead of satellite assemblies, allowing for a single executable file.
    /// </summary>
    public class EmbeddedResourceManager : ComponentResourceManager
    {
        private readonly Type _baseType;
        private readonly System.Reflection.Assembly _assembly;
        private ResourceSet _defaultResourceSet;
        private ResourceSet _currentResourceSet;
        private CultureInfo _currentCulture;
        private ResourceSet _pendingDisposal; // ResourceSet waiting to be disposed
        private readonly object _lockObject = new object(); // Lock for thread safety

        public EmbeddedResourceManager(Type baseType) : base(baseType)
        {
            _baseType = baseType;
            _assembly = baseType.Assembly;
            _currentCulture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Debug method to list all available manifest resources (for troubleshooting)
        /// </summary>
        public static string[] GetAllManifestResources(Type baseType)
        {
            return baseType.Assembly.GetManifestResourceNames();
        }

        /// <summary>
        /// Gets the current resource set for debugging
        /// </summary>
        public ResourceSet GetCurrentResourceSet()
        {
            return _currentResourceSet;
        }

        private ResourceSet GetResourceSetInternal(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            if (culture == null)
            {
                culture = CultureInfo.InvariantCulture;
            }

            // Normalize culture code for resource name
            string cultureName = NormalizeCultureName(culture.Name);

            // Check if we need to reload (culture changed)
            bool cultureChanged = _currentCulture == null ||
                                  !_currentCulture.Name.Equals(culture.Name, StringComparison.OrdinalIgnoreCase);

            // If requesting the same culture and we have a cached resource set, return it
            // But first check if it's still valid (not disposed)
            if (!cultureChanged && _currentResourceSet != null)
            {
                try
                {
                    var _ = _currentResourceSet.GetEnumerator();
                    return _currentResourceSet;
                }
                catch (ObjectDisposedException)
                {
                    // Resource set was disposed, clear it and reload
                    _currentResourceSet = null;
                }
            }

            // Try to load the resource set for the requested culture
            ResourceSet resourceSet = LoadResourceSet(cultureName);

            if (resourceSet == null && tryParents && culture.Parent != null &&
                !culture.Parent.Equals(CultureInfo.InvariantCulture))
            {
                // Try parent culture (recursive call without lock to avoid deadlock)
                resourceSet = GetResourceSetInternal(culture.Parent, createIfNotExists, tryParents);
            }

            if (resourceSet == null && _defaultResourceSet == null)
            {
                // Load default resource set (no culture suffix)
                _defaultResourceSet = LoadResourceSet(null);
                resourceSet = _defaultResourceSet;
            }

            if (resourceSet == null && _defaultResourceSet != null)
            {
                resourceSet = _defaultResourceSet;
            }

            // Only dispose old resource set after new one is successfully loaded
            if (resourceSet != null && cultureChanged && _currentResourceSet != null)
            {
                // Mark old resource set for delayed disposal
                if (_pendingDisposal != null)
                {
                    try
                    {
                        _pendingDisposal.Dispose();
                    }
                    catch
                    {
                        // Ignore disposal errors
                    }
                }

                _pendingDisposal = _currentResourceSet;
            }

            if (resourceSet != null)
            {
                _currentResourceSet = resourceSet;
                _currentCulture = culture;
            }

            return resourceSet;
        }

        public override ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            lock (_lockObject)
            {
                return GetResourceSetInternal(culture, createIfNotExists, tryParents);
            }
        }

        private string NormalizeCultureName(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
                return null;

            // Map culture variants to their resource file names
            // These must match the LogicalName in the .csproj file
            switch (cultureName.ToLowerInvariant())
            {
                case "en":
                case "en-us":
                    return "en";
                case "de":
                case "de-de":
                    return "de";
                case "ja":
                case "ja-jp":
                    return "ja";
                case "fr":
                case "fr-fr":
                    return "fr";
                case "zh-cn":
                    return "zh-CN";
                case "zh-hans":
                    return "zh-Hans";
                case "zh":
                case "zh-tw":
                case "zh-hant":
                    return "zh";
                default:
                    return cultureName;
            }
        }

        private ResourceSet LoadResourceSet(string cultureName)
        {
            string[] resourceNamesToTry;

            if (!string.IsNullOrEmpty(cultureName))
            {
                // Try with LogicalName format (as specified in .csproj)
                resourceNamesToTry = new string[]
                {
                    "PDFBinder.MainForm." + cultureName + ".resources",      // LogicalName format
                    _baseType.FullName + "." + cultureName + ".resources",   // FullName format
                    _baseType.Namespace + ".MainForm." + cultureName + ".resources" // Namespace format
                };
            }
            else
            {
                // Default resource (no culture)
                resourceNamesToTry = new string[]
                {
                    "PDFBinder.MainForm.resources",                 // LogicalName format
                    _baseType.FullName + ".resources",             // FullName format
                    _baseType.Namespace + ".MainForm.resources"    // Namespace format
                };
            }

            foreach (string resourceName in resourceNamesToTry)
            {
                try
                {
                    var stream = _assembly.GetManifestResourceStream(resourceName);
                    if (stream != null)
                    {
                        // Create ResourceSet from stream
                        var resourceSet = new ResourceSet(stream);
                        return resourceSet;
                    }
                }
                catch
                {
                    // Continue to next name
                }
            }

            // For zh-Hans, also try zh-CN as fallback
            if (cultureName == "zh-Hans")
            {
                resourceNamesToTry = new string[]
                {
                    "PDFBinder.MainForm.zh-CN.resources",
                    _baseType.FullName + ".zh-CN.resources",
                    _baseType.Namespace + ".MainForm.zh-CN.resources"
                };

                foreach (string resourceName in resourceNamesToTry)
                {
                    try
                    {
                        var stream = _assembly.GetManifestResourceStream(resourceName);
                        if (stream != null)
                        {
                            return new ResourceSet(stream);
                        }
                    }
                    catch
                    {
                        // Continue
                    }
                }
            }

            return null;
        }

        public override object GetObject(string name)
        {
            // Use current UI culture instead of cached culture
            return GetObject(name, Thread.CurrentThread.CurrentUICulture);
        }

        public override object GetObject(string name, CultureInfo culture)
        {
            lock (_lockObject)
            {
                var resourceSet = GetResourceSetInternal(culture, true, true);
                if (resourceSet != null)
                {
                    try
                    {
                        return resourceSet.GetObject(name);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Resource set was disposed, reload it
                        resourceSet = GetResourceSetInternal(culture, true, true);
                        if (resourceSet != null)
                        {
                            return resourceSet.GetObject(name);
                        }
                    }
                }

                // Fallback to default if not found
                if (_defaultResourceSet != null)
                {
                    try
                    {
                        return _defaultResourceSet.GetObject(name);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Reload default resource set
                        _defaultResourceSet = LoadResourceSet(null);
                        if (_defaultResourceSet != null)
                        {
                            return _defaultResourceSet.GetObject(name);
                        }
                    }
                }

                return null;
            }
        }

        public override string GetString(string name)
        {
            // Use current UI culture instead of cached culture
            return GetString(name, Thread.CurrentThread.CurrentUICulture);
        }

        public override string GetString(string name, CultureInfo culture)
        {
            lock (_lockObject)
            {
                var resourceSet = GetResourceSetInternal(culture, true, true);
                if (resourceSet != null)
                {
                    try
                    {
                        return resourceSet.GetString(name);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Resource set was disposed, reload it
                        resourceSet = GetResourceSetInternal(culture, true, true);
                        if (resourceSet != null)
                        {
                            return resourceSet.GetString(name);
                        }
                    }
                }

                // Fallback to default if not found
                if (_defaultResourceSet != null)
                {
                    try
                    {
                        return _defaultResourceSet.GetString(name);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Reload default resource set
                        _defaultResourceSet = LoadResourceSet(null);
                        if (_defaultResourceSet != null)
                        {
                            return _defaultResourceSet.GetString(name);
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Releases all resources used by this EmbeddedResourceManager.
        /// 建議只在程式真的要結束時呼叫。
        /// </summary>
        public new void ReleaseAllResources()
        {
            lock (_lockObject)
            {
                _currentResourceSet?.Dispose();
                _defaultResourceSet?.Dispose();
                _pendingDisposal?.Dispose();

                _currentResourceSet = null;
                _defaultResourceSet = null;
                _pendingDisposal = null;
            }
        }
    }
}
