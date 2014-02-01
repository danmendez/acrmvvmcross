﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Acr.MvvmCross.Plugins.Storage.Impl {
    
    public class DirectoryInfoImpl : IDirectoryInfo {
        private readonly DirectoryInfo directory;
        private readonly Lazy<IDirectoryInfo> parent;
        private readonly Lazy<IDirectoryInfo> root; 


        public DirectoryInfoImpl(DirectoryInfo directory) {
            this.directory = directory;
            this.parent = new Lazy<IDirectoryInfo>(() => new DirectoryInfoImpl(this.directory.Parent));
            this.root = new Lazy<IDirectoryInfo>(() => new DirectoryInfoImpl(this.directory.Root));
        }


        #region IDirectoryInfo Members

        public string Name {
            get { return this.directory.Name; }
        }


        public string FullName {
            get { return this.directory.FullName; }
        }


        public IDirectoryInfo Parent {
            get { return this.parent.Value; }
        }
        

        public IDirectoryInfo Root {
            get { return this.root.Value; }
        }


        public DateTime LastAccessTime {
            get {  return this.directory.LastAccessTime; }
        }


        public DateTime LastAccessTimeUtc {
            get { return this.directory.LastAccessTimeUtc; }
        }


        public DateTime LastWriteTime {
            get { return this.directory.LastWriteTime; }
        }


        public DateTime LastWriteTimeUtc {
            get { return this.directory.LastWriteTimeUtc; }
        }


        public bool Exists {
            get { return this.directory.Exists; }
        }


        public IEnumerable<IFileInfo> GetFiles(string searchPattern, bool recursive) {
            var search = (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return this.directory
                .GetFiles(searchPattern, search)
                .Select(x => new FileInfoImpl(x));
        }


        public IEnumerable<IDirectoryInfo> GetSubDirectories(string searchPattern, bool recursive) {
            var search = (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return this.directory
                .GetDirectories(searchPattern, search)
                .Select(x => new DirectoryInfoImpl(x));
        }


        public void CreateSubDirectory(string name) {
            this.directory.CreateSubdirectory(name);
        }


        public void Delete() {
            this.directory.Delete(true);
        }


        public void Create() {
            this.directory.Create();
        }

        #endregion
    }
}