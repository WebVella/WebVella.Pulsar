using Microsoft.AspNetCore.Components;
using System;
using System.Timers;
using System.Diagnostics;
using WebVella.Pulsar.Models;
using System.Threading.Tasks;

namespace WebVella.Pulsar.Components
{
	public partial class WvpTimer : WvpBase, IAsyncDisposable
	{
		#region << Parameters >>
		[Parameter] public int MaxTimeInSeconds { get; set; } = -1;
		[Parameter] public TimeSpan MaxTimeSpan { get; set; }
		[Parameter] public int StepInMs { get; set; } = 1000;
		[Parameter] public bool ShowTimer { get; set; } = false;
		[Parameter] public bool IsCountDown { get; set; } = false;
		[Parameter] public EventCallback Tick { get; set; }
		[Parameter] public EventCallback TimeCountFinished { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private Timer _timer;
		private Stopwatch _stpWatch;
		private TimeSpan? _max;

		public TimeSpan TimeElapsed
		{
			get
			{
				if (_max != null && IsCountDown)
					return (_max.Value.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) > 0 ? TimeSpan.FromMilliseconds(_max.Value.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) : TimeSpan.FromMilliseconds(0);

				//else
				return TimeSpan.FromMilliseconds(_stpWatch.ElapsedMilliseconds);
			}
		}

		private bool _mustStop
		{
			get
			{
				if (_max != null)
					return (_max.Value.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) < 0;
				else
					return false;
			}
		}

		public string TimeLeftStr
		{
			get
			{
				return TimeElapsed.ToString(@"hh\:mm\:ss");
			}
		}

		public string TimeLeftMsStr
		{
			get
			{
				return TimeElapsed.ToString("G");
			}
		}

		private volatile bool _requestStop = false;
		#endregion

		#region << Lifecycle methods >>
		public async ValueTask DisposeAsync()
		{
			//await Task.Delay(1);
			Stop();
		}

		protected override void OnInitialized()
		{
			_timer = new Timer();
			_timer.Elapsed += TimerTick;
			_timer.AutoReset = false;
			_stpWatch = new Stopwatch();
			base.OnInitialized();
		}
		protected override void OnParametersSet()
		{
			if (MaxTimeInSeconds > 0)
				MaxTimeSpan = TimeSpan.FromSeconds(MaxTimeInSeconds);

			_max = MaxTimeSpan;
			_timer.Interval = StepInMs;
			_requestStop = false;

			base.OnParametersSet();
		}

		protected override void OnAfterRender(bool firstRender)
		{
			if (firstRender && !_requestStop)
				Start();
		}
		#endregion

		#region << Private methods >>

		private void TimerTick(object sender, ElapsedEventArgs e)
		{
			InvokeAsync(() => Tick.InvokeAsync(null));

			if (!_requestStop && !_mustStop)
			{
				_timer.Start();//restart the timer
			}
			else
			{
				if (_mustStop)
					_ = InvokeAsync(() => TimeCountFinished.InvokeAsync(null));

				Stop();
			}

			InvokeAsync(StateHasChanged);
		}

		public void Start()
		{
			_timer.Start();
			_stpWatch.Start();
		}

		public void Pause()
		{
			_timer.Stop();
			_timer.Close();
			_stpWatch.Stop();
		}

		public void Stop()
		{
			_requestStop = true;
			Pause();
			//Reset();
		}

		public void Reset()
		{
			_stpWatch.Reset();
		}

		public void Restart()
		{
			_stpWatch.Reset();
			_timer.Start();
		}

		#endregion

		#region << UI Handlers >>

		#endregion

		#region << JS Callbacks methods >>
		#endregion
	}
}