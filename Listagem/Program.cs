using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MultiFolderSelector
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var selectedFolders = args.Length > 0 ? new List<string>(args[0].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) : OpenFolderDialog();

                // Copiar os nomes das pastas e arquivos selecionados para a área de transferência
                if (selectedFolders.Count > 0)
                {
                    List<string> outputLines = [];

                    foreach (var folderPath in selectedFolders)
                    {
                        if (string.IsNullOrEmpty(folderPath))
                            continue;
                        string normalizedFolderPath = folderPath.Trim();
                        if (string.IsNullOrWhiteSpace(normalizedFolderPath) || !Directory.Exists(normalizedFolderPath))
                        {
                            Console.WriteLine($"Caminho inválido ou inexistente: {folderPath}");
                            Console.ReadKey();
                            continue;
                        } 
                        else
                        {
                            AddFolderContents(normalizedFolderPath, outputLines, 0);
                        }
                    }

                    string outputString = string.Join(Environment.NewLine, outputLines);
                    Clipboard.SetText(outputString);
                    Console.WriteLine("Os nomes das pastas e arquivos selecionados foram copiados para a área de transferência:");
                    Console.WriteLine(outputString);
                }
                else
                {
                    Console.WriteLine("Nenhuma pasta foi selecionada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro no Programa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void AddFolderContents(string folderPath, List<string> outputLines, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 2); // Indentação para subpastas
            
            string folderName = Path.GetFileName(folderPath.TrimEnd(Path.DirectorySeparatorChar));
            outputLines.Add(indent + folderName);

            // Adicionar os nomes dos arquivos dentro da pasta
            string[] files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                outputLines.Add(indent + "  " + Path.GetFileName(file));
            }

            // Adicionar os nomes das subpastas e seus conteúdos
            string[] subfolders = Directory.GetDirectories(folderPath);
            foreach (var subfolder in subfolders)
            {
                AddFolderContents(subfolder, outputLines, indentLevel + 1);
                outputLines.Add(""); // Adiciona uma linha em branco após cada subpasta para separação
            }
        }

        public static List<string> OpenFolderDialog()
        {
            var folderPaths = new List<string>();

            var dialog = (IFileOpenDialog)new FileOpenDialog();
            dialog.SetOptions(FOS.FOS_PICKFOLDERS | FOS.FOS_ALLOWMULTISELECT);

            const int S_OK = 0;
            if (dialog.Show(IntPtr.Zero) == S_OK)
            {
                dialog.GetResults(out IShellItemArray results);
                results.GetCount(out uint count);

                for (uint i = 0; i < count; i++)
                {
                    results.GetItemAt(i, out IShellItem item);
                    item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out IntPtr pathPtr);
                    string path = Marshal.PtrToStringAuto(pathPtr);
                    Marshal.FreeCoTaskMem(pathPtr);

                    if (!string.IsNullOrEmpty(path))
                    {
                        folderPaths.Add(path);
                    }
                }
            }

            return folderPaths;
        }

        [ComImport]
        [Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileOpenDialog
        {
            int Show(IntPtr parent);
            void SetFileTypes(uint cFileTypes, [MarshalAs(UnmanagedType.LPArray)] COMDLG_FILTERSPEC[] rgFilterSpec);
            void SetFileTypeIndex(uint iFileType);
            void GetFileTypeIndex(out uint piFileType);
            void Advise(IntPtr pfde, out uint pdwCookie);
            void Unadvise(uint dwCookie);
            void SetOptions(FOS fos);
            void GetOptions(out FOS pfos);
            void SetDefaultFolder(IShellItem psi);
            void SetFolder(IShellItem psi);
            void GetFolder(out IShellItem ppsi);
            void GetCurrentSelection(out IShellItem ppsi);
            void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetFileName(out IntPtr pszName);
            void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
            void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            void GetResult(out IShellItem ppsi);
            void AddPlace(IShellItem psi, int alignment);
            void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            void Close(int hr);
            void SetClientGuid(ref Guid guid);
            void ClearClientData();
            void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
            void GetResults(out IShellItemArray ppenum);
            void GetSelectedItems(out IShellItemArray ppsai);
        }

        [ComImport]
        [Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
        private class FileOpenDialog { }

        [ComImport]
        [Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItemArray
        {
            void BindToHandler(IntPtr pbc, ref Guid bhid, ref Guid riid, out IntPtr ppvOut);
            void GetPropertyStore(int flags, ref Guid riid, out IntPtr ppv);
            void GetPropertyDescriptionList(ref Guid keyType, ref Guid riid, out IntPtr ppv);
            void GetAttributes(int AttribFlags, out uint sfgao);
            void GetCount(out uint pdwNumItems);
            void GetItemAt(uint dwIndex, out IShellItem ppsi);
        }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            void BindToHandler(IntPtr pbc, ref Guid bhid, ref Guid riid, out IntPtr ppvOut);
            void GetParent(out IShellItem ppsi);
            void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
            void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
            void Compare(IShellItem psi, uint hint, out int piOrder);
        }

        private enum SIGDN : uint
        {
            SIGDN_FILESYSPATH = 0x80058000,
        }

        [Flags]
        private enum FOS : uint
        {
            FOS_PICKFOLDERS = 0x00000020,
            FOS_ALLOWMULTISELECT = 0x00000200,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct COMDLG_FILTERSPEC
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszSpec;
        }
    }
}
