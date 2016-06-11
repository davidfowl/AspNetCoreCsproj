using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MyWebApplication
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<RazorViewEngineOptions>(r => r.CompilationCallback = c =>
            {
                // Add a reference to this assembly
                c.Compilation = c.Compilation.AddReferences(MetadataReference.CreateFromFile(typeof(Startup).Assembly.Location));

                // We'll make Razor link against the same set of assemblies this project was linked to.
                // That list is available in references.txt.
                // Either the reference was copied locally (say, Microsoft.AspNetCore.Razor.dll), or
                // it was a Reference Assembly (say, mscorlib.dll).
                string outputDir = Path.GetDirectoryName(typeof(Startup).Assembly.Location);
                string references = Path.Combine(outputDir, @"references.txt");

                // https://blogs.msdn.microsoft.com/msbuild/2007/04/12/new-reference-assemblies-location/
                string referenceAssembliesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\");

                foreach (var reference in File.ReadAllLines(references))
                {
                    string copyLocalPath = Path.Combine(outputDir, reference);
                    string referenceAssemblyPath = Path.Combine(referenceAssembliesFolder, reference);

                    if (File.Exists(copyLocalPath))
                    {
                        c.Compilation = c.Compilation.AddReferences(MetadataReference.CreateFromFile(copyLocalPath));
                    }
                    else if (File.Exists(referenceAssemblyPath))
                    {
                        c.Compilation = c.Compilation.AddReferences(MetadataReference.CreateFromFile(referenceAssemblyPath));
                    }
                }
            });
        }
    }
}
