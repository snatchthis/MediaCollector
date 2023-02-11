using System;
namespace MediaCollector.Services
{
	public class TarFilePickerService : AbstractFilePickerService
	{
		public TarFilePickerService()
		{
		}

		protected override string[] Extensions => new[] {"tar"};

        protected override PickOptions Options => new PickOptions()
		{
			PickerTitle = "Select *.tar archive"
		};
    }
}

