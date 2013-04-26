using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Moq;

namespace Minify
{
    public class BundleService
    {
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleService"/> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        public BundleService(IFileService fileService)
        {
            if (fileService == null)
            {
                throw new ArgumentNullException("fileService");
            }
            _fileService = fileService;
        }

        /// <summary>
        /// Bundles the specified files.
        /// </summary>
        /// <param name="outputFile">The output file.</param>
        /// <param name="sourceFiles">The source files.</param>
        /// <param name="type">The type.</param>
        public void Bundle(string outputFile, IEnumerable<string> sourceFiles, BundleType type)
        {
            if (String.IsNullOrEmpty(outputFile))
            {
                throw new ArgumentException("Destination is required", "outputFile");
            }
            if (sourceFiles == null || !sourceFiles.Any())
            {
                throw new ArgumentException("sourcePaths is required.", "sourceFiles");
            }

            if (type == BundleType.None)
            {
                if (outputFile.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
                {
                    type = BundleType.Script;
                }
                else if (outputFile.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
                {
                    type = BundleType.Style;
                }
            }

            if (type == BundleType.None)
            {
                throw new ArgumentException("Type is required.", "type");
            }

            BundleTable.MapPathMethod = MapPath;

            string virtualPath = "~/" + outputFile;
            Bundle bundle = GetBundle(type, virtualPath);
            RegisterBundles(bundle, sourceFiles);

            WriteCombinedScript(bundle, outputFile);
        }

        private static Bundle GetBundle(BundleType type, string virtualPath)
        {
            Bundle bundle;
            if (type == BundleType.Js || type == BundleType.Script)
            {
                bundle = new ScriptBundle(virtualPath);
            }
            else
            {
                bundle = new StyleBundle(virtualPath);
            }

            return bundle;
        }

        private void RegisterBundles(Bundle bundle, IEnumerable<string> sourcePaths)
        {
            foreach (var sourcePath in sourcePaths)
            {
                if (!_fileService.Exists(sourcePath))
                {
                    throw new InvalidOperationException("Source does not exist: " + sourcePath);
                }
                bundle.Include("~/" + sourcePath);
            }
        }

        private void WriteCombinedScript(Bundle bundle, string outputPath)
        {
            // mock the HttpContext since BundleContext requires it, but it doesn't need to do anything ...
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            HttpContextBase httpContext = mockHttpContext.Object;

            BundleCollection bundles = new BundleCollection {bundle};
            BundleContext bundleContext = new BundleContext(httpContext, bundles, bundle.Path);

            IBundleTransform minify = bundle is ScriptBundle ? (IBundleTransform) new JsMinify() : new CssMinify();
            BundleResponse response = bundle.GenerateBundleResponse(bundleContext);
            minify.Process(bundleContext, response);

            Console.WriteLine("Saved bundle to: " + outputPath);
#if DEBUG
            Console.WriteLine(response.Content);
#endif

            _fileService.Save(outputPath, response.Content);
        }

        private static string MapPath(string virtualPath)
        {
            return virtualPath == null ? null : virtualPath.Replace("~/", "./").Replace("/", "\\");
        }
    }
}