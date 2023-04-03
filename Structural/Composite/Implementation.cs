//Composite - The intent of this pattern is to compose object into tree structures to represent part-whole hierarchies. As such, it lets clients treat individual objects and compositions of objects uniformly: as if they all were individual objects.
//Use cases: provide a transparent, easy way to work with tree-hierarchy.

using System.Security.Cryptography.X509Certificates;

namespace Composite;

//Real life example

/// <summary>
/// Component
/// </summary>
public abstract class FileSystemItem
{
    public string Name { get; set; }
    public abstract long GetSize();
    public FileSystemItem(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// Leaf
    /// </summary>
    public class File : FileSystemItem 
    {
        private long _size;
        public File(string name, long size) : base(name)
        {
            _size = size;
        }
        public override long GetSize()
        {
            return _size;   
        }
    }

    /// <summary>
    /// Composite
    /// </summary>
    public class Directory : FileSystemItem
    {
        private List<FileSystemItem> _fileSystemItems /*{ get; set; }*/ = new();
        private long _size;
        public Directory(string name, long size) : base(name)
        {
            _size = size;
        }
        public void Add(FileSystemItem item)
        {
            _fileSystemItems.Add(item);
        }
        public void Remove(FileSystemItem item)
        {
            _fileSystemItems.Remove(item);
        }
        public override long GetSize()
        {
            var treeSize = _size;
            foreach(var fileSystemItem in _fileSystemItems)
            {
                treeSize += fileSystemItem.GetSize();
            }
            return treeSize;
        }
    }
}
