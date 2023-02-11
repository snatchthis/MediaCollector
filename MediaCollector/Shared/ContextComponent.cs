using System;
using MediaCollector.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MediaCollector.Shared
{
	public class ContextComponent<TViewModel> : ComponentBase where TViewModel : class, IViewModel
    {
		public ContextComponent()
		{
		}

		[Inject]
		protected TViewModel DataContext { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (DataContext != null)
			{
				DataContext.PropertyChanged += async (sender, e) =>
				{
					await InvokeAsync(() =>
					{
						StateHasChanged();
					}
					);
				};
			}

			await base.OnInitializedAsync();
		}
    }
}

