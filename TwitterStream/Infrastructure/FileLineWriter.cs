using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace TwitterStream.Infrastructure
{
    class FileLineWriter : IDisposable
    {
        private FileStream fileStream;
        private GZipStream gzStream;
        private StreamWriter writer;
        private int lineCount = 1;
        
        public void WriteLine(string value) {
            if (lineCount == 1) CreateWriter();

            writer.WriteLine(value);
            lineCount++;
            
            if (lineCount >= 500) {
                Close();
                lineCount = 1;
            }
        }

        private void CreateWriter() {
            var fileName = string.Format("{0}.gz", DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"));
            Console.WriteLine("Create " + fileName + "...");
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            gzStream   = new GZipStream(fileStream, CompressionMode.Compress, true);
            writer     = new StreamWriter(gzStream, Encoding.UTF8);
        }

        public void Close() {
            if (writer != null) writer.Close();
            if (gzStream != null) gzStream.Close();
            if (fileStream != null) fileStream.Close();
            writer = null;
            gzStream = null;
            fileStream = null;
        }

        public void Dispose() {
            Close();
        }
    }
}
