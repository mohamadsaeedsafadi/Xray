using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace Csharp
{

    public class GZipCompressor
    {
        public static byte[] CompressBytes(byte[] bytes)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(outputStream, CompressionLevel.Optimal))
                {
                    compressionStream.Write(bytes, 0, bytes.Length);
                }
                return outputStream.ToArray();
            }
        }

        public static byte[] DecompressBytes(byte[] bytes)
        {
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var compressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        compressionStream.CopyTo(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

    }
}

