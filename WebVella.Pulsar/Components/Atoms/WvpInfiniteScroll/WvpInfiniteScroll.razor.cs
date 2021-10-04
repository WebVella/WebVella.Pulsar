﻿using Microsoft.AspNetCore.Components;
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
	public partial class WvpInfiniteScroll : WvpBase, IDisposable
	{
		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string ObserverTargetId { get; set; }
		[Parameter] public bool ObserverViewportVisible { get; set; } = false;
		[Parameter] public string ObserverViewportId { get; set; }
		[Parameter] public EventCallback OnObservableTargetReached { get; set; }
		#endregion

		#region << Callbacks >>

		public async Task PerformVisibilityCheck()
		{
			await Task.Delay(100);
			var jsSrv = new JsService(JSRuntime);
			//This fixes the case when even after the first intersect, the observed element is still visible and we need to force more callbacks
			var isVisible= await jsSrv.CheckIfElementIdVisible(ObserverTargetId);
			if (isVisible)
			{
				await OnObservableTargetReached.InvokeAsync();
			}
		}

		#endregion

		#region << Private properties >>

		private DotNetObjectReference<WvpInfiniteScroll> _objectRef;

		private Guid _componentId = Guid.NewGuid();

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).DestroyInfiniteScroll(_componentId);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectRef = DotNetObjectReference.Create(this);
				if (String.IsNullOrWhiteSpace(ObserverTargetId))
				{
					ObserverTargetId = $"wvp-loading-bar-{Id}";
					ObserverViewportVisible = true;
				}
				await InvokeAsync(StateHasChanged);
				await Task.Delay(0);
				await new JsService(JSRuntime).InitializeInfiniteScroll(_componentId, _objectRef, ObserverTargetId, ObserverViewportId);
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
			await OnObservableTargetReached.InvokeAsync();
		}
		#endregion
	}
}