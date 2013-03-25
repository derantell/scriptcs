using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ScriptCs {
    public class MarkdownAwareFileSystem : IFileSystem{
        public MarkdownAwareFileSystem(IFileSystem wrappee) {
            _wrappedFileSystem = wrappee;
        }


        public IEnumerable<string> EnumerateFiles(string dir, string search) {
            return _wrappedFileSystem.EnumerateFiles(dir, search);
        }


        public void Copy(string source, string dest, bool overwrite) {
            _wrappedFileSystem.Copy(source, dest, overwrite);
        }


        public bool DirectoryExists(string path) {
            return _wrappedFileSystem.DirectoryExists(path);
        }


        public void CreateDirectory(string path) {
            _wrappedFileSystem.CreateDirectory(path);
        }


        public string ReadFile(string path) {
            if (!LiteralScriptCsFilter.ShouldHandle(path)) {
                return _wrappedFileSystem.ReadFile(path);
            }
            return string.Join(NewLine, ReadFileLines(path));
        }


        public string[] ReadFileLines(string path) {
            var fileLines = _wrappedFileSystem.ReadFileLines(path);
            
            if (!LiteralScriptCsFilter.ShouldHandle(path)) {
                return fileLines;
            }

            return LiteralScriptCsFilter.FilterLines(fileLines);
        }


        public DateTime GetLastWriteTime(string file) {
            return _wrappedFileSystem.GetLastWriteTime(file);
        }


        public bool IsPathRooted(string path) {
            return _wrappedFileSystem.IsPathRooted(path);
        }


        public string CurrentDirectory { 
            get { return _wrappedFileSystem.CurrentDirectory; } 
        }

        public string NewLine { get { return _wrappedFileSystem.NewLine; } }

        public string GetWorkingDirectory(string path) {
            return _wrappedFileSystem.GetWorkingDirectory(path);
        }


        public void Move(string source, string dest) {
            _wrappedFileSystem.Move(source,dest);
        }


        public bool FileExists(string path) {
            return _wrappedFileSystem.FileExists(path);
        }


        public Stream CreateFileStream(string filePath, FileMode mode) {
            return _wrappedFileSystem.CreateFileStream(filePath, mode);
        }


        private readonly IFileSystem _wrappedFileSystem;
    }

    public class LiteralScriptCsFilter {
        public static bool ShouldHandle(string path) 
        {
            return LiterateScriptCsExtensions.Contains(Path.GetExtension(path) ?? "");
        }

        public static string[] FilterLines(IEnumerable<string> fileLines) 
        {
            var isInsideFence = false;
            var csLines = new List<string>();

            foreach (var line in fileLines)
            {
                if (Regex.IsMatch(line, @"^```(?:csharp\b|\s*$)"))
                {
                    isInsideFence = !isInsideFence;
                    continue;
                }

                if (isInsideFence || Regex.IsMatch(line, @"^(?:\t| {4})"))
                {
                    csLines.Add(line);
                }
            }

            return csLines.ToArray();
        }

        private const string LiterateScriptCsExtensions = ".litcsx.md.mdown.markdown";
    }
}