using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PdfSharpCore.Fonts;
using System;
using System.IO;

namespace TSWebApplication.Web.MiddleWares
{
    public class FontResolver : IFontResolver
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string fontPath;
        private readonly byte[] _fontBytes;
        public FontResolver(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            fontPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "Font", "MS Gothic Regular.ttf");
            _fontBytes = File.ReadAllBytes(fontPath);
        }
       
        public byte[] GetFont(string faceName)
        {
            return _fontBytes;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("MS Gothic", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("MS Gothic");
            }

            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        public string DefaultFontName => "MS Gothic";
    }
}