/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2019 chibayuki@foxmail.com

Com.IO
Version 19.8.10.1600

This file is part of Com

Com is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Com
{
    /// <summary>
    /// 为文件操作提供静态方法。
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// 将源文件夹内的所有内容复制到目标文件夹内，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="sourceFolder">源文件夹。</param>
        /// <param name="destFolder">目标文件夹。</param>
        /// <param name="recursive">如果存在子文件夹，是否递归复制所有子文件夹内的内容。</param>
        /// <param name="merge">如果存在同名文件夹，是否合并文件夹内容。</param>
        /// <param name="overwrite">如果存在同名文件，是否覆盖目标文件。</param>
        /// <returns>布尔值，表示此操作是否成功。</returns>
        public static bool CopyFolder(string sourceFolder, string destFolder, bool recursive, bool merge, bool overwrite)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceFolder) || string.IsNullOrWhiteSpace(destFolder) || !Directory.Exists(sourceFolder))
                {
                    return false;
                }
                else
                {
                    if (!Directory.Exists(destFolder))
                    {
                        Directory.CreateDirectory(destFolder);
                    }
                    else if (!merge)
                    {
                        return true;
                    }

                    //

                    FileInfo[] ChildFiles = new DirectoryInfo(sourceFolder).GetFiles();

                    foreach (FileInfo FInfo in ChildFiles)
                    {
                        File.Copy(Path.Combine(sourceFolder, FInfo.Name), Path.Combine(destFolder, FInfo.Name), overwrite);
                    }

                    //

                    if (recursive)
                    {
                        DirectoryInfo[] ChildFolders = new DirectoryInfo(sourceFolder).GetDirectories();

                        foreach (DirectoryInfo DirInfo in ChildFolders)
                        {
                            if (!CopyFolder(Path.Combine(sourceFolder, DirInfo.Name), Path.Combine(destFolder, DirInfo.Name), true, merge, overwrite))
                            {
                                return false;
                            }
                        }
                    }

                    //

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将源文件夹内的所有内容复制到目标文件夹内，并返回表示此操作是否成功的布尔值。如果存在同名文件，将不覆盖目标文件。
        /// </summary>
        /// <param name="sourceFolder">源文件夹。</param>
        /// <param name="destFolder">目标文件夹。</param>
        /// <param name="recursive">如果存在子文件夹，是否递归复制所有子文件夹内的内容。</param>
        /// <param name="merge">如果存在同名文件夹，是否合并文件夹内容。</param>
        /// <returns>布尔值，表示此操作是否成功。</returns>
        public static bool CopyFolder(string sourceFolder, string destFolder, bool recursive, bool merge)
        {
            return CopyFolder(sourceFolder, destFolder, recursive, merge, false);
        }

        /// <summary>
        /// 将源文件夹内的所有内容复制到目标文件夹内，并返回表示此操作是否成功的布尔值。如果存在同名文件夹，将不合并文件夹内容。如果存在同名文件，将不覆盖目标文件。
        /// </summary>
        /// <param name="sourceFolder">源文件夹。</param>
        /// <param name="destFolder">目标文件夹。</param>
        /// <param name="recursive">如果存在子文件夹，是否递归复制所有子文件夹内的内容。</param>
        /// <returns>布尔值，表示此操作是否成功。</returns>
        public static bool CopyFolder(string sourceFolder, string destFolder, bool recursive)
        {
            return CopyFolder(sourceFolder, destFolder, recursive, false, false);
        }

        /// <summary>
        /// 将源文件夹内的所有内容复制到目标文件夹内，并返回表示此操作是否成功的布尔值。如果存在子文件夹，将不递归复制子文件夹内的内容。如果存在同名文件夹，将不合并文件夹内容。如果存在同名文件，将不覆盖目标文件。
        /// </summary>
        /// <param name="sourceFolder">源文件夹。</param>
        /// <param name="destFolder">目标文件夹。</param>
        /// <returns>布尔值，表示此操作是否成功。</returns>
        public static bool CopyFolder(string sourceFolder, string destFolder)
        {
            return CopyFolder(sourceFolder, destFolder, false, false, false);
        }
    }
}