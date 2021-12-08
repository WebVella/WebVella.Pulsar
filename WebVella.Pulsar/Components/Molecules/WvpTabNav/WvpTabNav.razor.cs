using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;

namespace WebVella.Pulsar.Components
{
	public partial class WvpTabNav : WvpBase,IAsyncDisposable
	{

		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public RenderFragment<WvpTabPage> WvpTabText { get; set; }

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>
		private WvpTabPage _activePage = null;

		private DotNetObjectReference<WvpTabNav> _objectReference;

		private List<WvpTabPage> _pages = new List<WvpTabPage>();
		#endregion

		#region << Store properties >>

		internal WvpTabPage StoreActivePage { get { return _activePage; } }

		internal DotNetObjectReference<WvpTabNav> StoreObjectReference { get { return _objectReference; } }

		internal List<WvpTabPage> StorePages { get { return _pages; } }

		#endregion

		#region << Lifecycle methods >>

		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender)
				_objectReference = DotNetObjectReference.Create(this);
		}

#pragma warning disable 1998
		public async ValueTask DisposeAsync()
		{
			_objectReference?.Dispose();
		}
#pragma warning restore 1998

		#endregion

		#region << Private methods >>
		//All names should start with _

		#endregion

		#region << Store methods >>
		internal Task StoreAddTabPage(WvpTabPage tabPage)
		{
			_pages.Add(tabPage);

			if (_pages.Count == 1)
				_activePage = tabPage;

			InvokeAsync(StateHasChanged);
			return Task.CompletedTask;
		}

		internal Task StoreSetActiveTabPage(WvpTabPage tabPage)
		{
			_activePage = tabPage;
			InvokeAsync(StateHasChanged);
			return Task.CompletedTask;
		}

		#endregion

		#region << Ui handlers >>
		//All names should start with _
		private void _tabClickHandler(WvpTabPage page)
		{
			_activePage = page;
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}