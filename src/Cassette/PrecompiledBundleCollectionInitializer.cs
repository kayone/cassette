using Cassette.IO;
using Cassette.Manifests;

namespace Cassette
{
    class PrecompiledBundleCollectionInitializer
    {
        readonly IFile precompiledManifestFile;
        readonly IUrlModifier urlModifier;

        public PrecompiledBundleCollectionInitializer(IFile precompiledManifestFile, IUrlModifier urlModifier)
        {
            this.precompiledManifestFile = precompiledManifestFile;
            this.urlModifier = urlModifier;
        }

        public void Initialize(BundleCollection bundles)
        {
            using (bundles.GetWriteLock())
            {
                var manifest = ReadManifest();
                var createdBundles = manifest.CreateBundles(urlModifier);
                bundles.AddRange(createdBundles);
                bundles.BuildReferences();
            }
        }

        CassetteManifest ReadManifest()
        {
            using (var stream = precompiledManifestFile.OpenRead())
            {
                var reader = new CassetteManifestReader(stream);
                return reader.Read();
            }
        }
    }
}