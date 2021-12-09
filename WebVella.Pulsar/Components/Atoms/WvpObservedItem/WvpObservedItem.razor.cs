using Microsoft.AspNetCore.Components;
using System;
using System.Timers;
using System.Diagnostics;
using WebVella.Pulsar.Models;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Services;


//Important: Do not foget to add @key to the loaded rows, as otherwise DOM events will not be initialized correctly

namespace WebVella.Pulsar.Components
{
	public partial class WvpObservedItem : WvpBase, IAsyncDisposable
	{
		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string ObserverTargetId { get; set; }
		[Parameter] public string ObserverViewportId { get; set; }
		[Parameter] public bool IsVisible { get; set; } = true;
		[Parameter] public EventCallback OnObservableTargetReached { get; set; }
		#endregion

		#region << Callbacks >>
		private bool _isVisible = false;
		private bool _isPerformingCheck = false;
		public async Task<bool> PerformVisibilityCheck()
		{
			if(_isPerformingCheck)
				return false;
			_isPerformingCheck = true;
			var jsSrv = new JsService(JSRuntime);
			//This fixes the case when even after the first intersect, the observed element is still visible and we need to force more callbacks
			var isVisible= await jsSrv.CheckIfElementIdVisible(ObserverTargetId);
			_isPerformingCheck = false;
			if (isVisible)
			{
				return true;
			}
			return false;
		}

		#endregion

		#region << Private properties >>

		private DotNetObjectReference<WvpObservedItem> _objectRef;

		private Guid _componentId = Guid.NewGuid();

        #endregion

        #region << Lifecycle methods >>
        protected override void OnInitialized()
        {
			_isVisible = IsVisible;

		}

        public async ValueTask DisposeAsync()
		{
			await new JsService(JSRuntime).DestroyObservedItem(_componentId);
			_objectRef?.Dispose();
		}

        protected override bool ShouldRender()
        {
            if (IsVisible != _isVisible)
            {
                _isVisible = IsVisible;
                return true;
            }
            return false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectRef = DotNetObjectReference.Create(this);
				//Render so Child content can be populated
				await InvokeAsync(StateHasChanged);
				await Task.Delay(1);
				await new JsService(JSRuntime).InitializeObservedItem(_componentId, _objectRef, ObserverTargetId, ObserverViewportId);
			}
		}
		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnIntersection()
		{
			System.Diagnostics.Debug.WriteLine($" ********** {ObserverTargetId} ***** OnIntersection");
			await OnObservableTargetReached.InvokeAsync();
		}
		#endregion
	}
}