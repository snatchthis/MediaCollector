using MediaCollector.Data;

namespace MediaCollector.Platforms.Windows
{
    internal class FolderPicker : AbstractFolderPicker, IFolderPicker
    {
        public async Task<string> PickFolder()
        {
            return await Task.Run(GetFolderPath);
        }

        private string GetFolderPath()
        {
            var dialogResult = ShowDialog(IntPtr.Zero);
            if (dialogResult.HasValue && dialogResult.Value)
            {
                return ResultPath;
            }

            return string.Empty;
        }

        protected override bool? ShowDialog(nint owner, bool throwOnError = false)
        {
            return base.ShowDialog(IntPtr.Zero, throwOnError);
        }
    }
}
