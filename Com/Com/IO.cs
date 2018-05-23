/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2013-2018 chibayuki@foxmail.com

Com.IO
Version 18.5.23.0000

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
        /// 将源文件夹中的所有文件与文件夹（不包含源文件夹）复制到目标文件夹，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="sourceFolder">源文件夹。</param>
        /// <param name="destFolder">目标文件夹。</param>
        public static bool CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                if (Directory.Exists(sourceFolder))
                {
                    if (!Directory.Exists(destFolder))
                    {
                        Directory.CreateDirectory(destFolder);
                    }

                    //

                    FileInfo[] ChildFiles = new DirectoryInfo(sourceFolder).GetFiles();

                    for (int i = 0; i < ChildFiles.Length; i++)
                    {
                        File.Copy(sourceFolder + "\\" + ChildFiles[i].Name, destFolder + "\\" + ChildFiles[i].Name, true);
                    }

                    //

                    DirectoryInfo[] ChildFolders = new DirectoryInfo(sourceFolder).GetDirectories();

                    for (int i = 0; i < ChildFolders.Length; i++)
                    {
                        CopyFolder(sourceFolder + "\\" + ChildFolders[i].Name, destFolder + "\\" + ChildFolders[i].Name);
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除源文件夹以及源文件夹中的所有文件与文件夹，并返回表示此操作是否成功的布尔值。
        /// </summary>
        /// <param name="sourceFolder">源文件夹。</param>
        public static bool DeleteFolder(string sourceFolder)
        {
            try
            {
                if (Directory.Exists(sourceFolder))
                {
                    Directory.Delete(sourceFolder, true);

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}