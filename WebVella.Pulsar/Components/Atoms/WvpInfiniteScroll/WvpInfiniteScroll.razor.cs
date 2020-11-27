using Microsoft.AspNetCore.Components;
using System;
using System.Timers;
using System.Diagnostics;
using WebVella.Pulsar.Models;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Services;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInfiniteScroll : WvpBase, IDisposable
	{
		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string ObserverTargetId { get; set; }

		[Parameter] public EventCallback<bool> OnObservableTargetReached { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private DotNetObjectReference<WvpInfiniteScroll> _objectRef;

		private Guid _componentId = Guid.NewGuid();

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			Task.Run(async () =>
			{
				await new JsService(JSRuntime).DestroyInfiniteScroll(_componentId);
			})
			.ContinueWith((r) =>	{if (r.Exception != null){ throw r.Exception.InnerException;}});
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectRef = DotNetObjectReference.Create(this);
				await Task.Delay(1);
				await new JsService(JSRuntime).InitializeInfiniteScroll(_componentId, _objectRef, ObserverTargetId);
			}

			base.OnAfterRender(firstRender);
		}
		#endregion

		#region << Private methods >>
		private async Task _invokeCallback(){
			await OnObservableTargetReached.InvokeAsync(true);
			var jsSrv = new JsService(JSRuntime);
			//This fixes the case when even after the first intersect, the observed element is still visible and we need to force more callbacks
			var elementIsVisible = true;
			while(elementIsVisible){
				await Task.Delay(100);
				Debug.WriteLine("called");
				elementIsVisible = await jsSrv.CheckIfElementIdVisible(ObserverTargetId);
				if(elementIsVisible)
				{
					await OnObservableTargetReached.InvokeAsync(true);				
				}
			}
		}

		#endregion

		#region << UI Handlers >>

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnIntersection()
		{
			await _invokeCallback();
		}
		#endregion
	}
}