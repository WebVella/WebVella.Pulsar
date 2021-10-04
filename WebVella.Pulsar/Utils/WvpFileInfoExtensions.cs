using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebVella.Pulsar.Models
{
	public static class WvpFileInfoExtensions
	{
		public static byte[] GetTempFileBytes(this WvpFileInfo fileInfo)
		{
			string tmpFilePath = fileInfo.ServerTempPath;
			if (string.IsNullOrWhiteSpace(tmpFilePath))
				throw new Exception("ServerTempPath is null or empty");
			if (!File.Exists(tmpFilePath))
				throw new Exception($"{tmpFilePath} does not exist.");

			byte[] result;
			using (FileStream stream = File.OpenRead(tmpFilePath))
			{
				result = new byte[stream.Length];
				stream.Read(result, 0, (int)stream.Length);
			}

			return result;
		}


		public static Stream GetTempFileStream(this WvpFileInfo fileInfo)
		{
			string tmpFilePath = fileInfo.ServerTempPath;
			if (string.IsNullOrWhiteSpace(tmpFilePath))
				throw new Exception("ServerTempPath is null or empty");
			if (!File.Exists(tmpFilePath))
				throw new Exception($"{tmpFilePath} does not exist.");

			return File.OpenRead(tmpFilePath);
		}


		public static void DeleteTempFile(this WvpFileInfo fileInfo)
		{
			string tmpFilePath = fileInfo.ServerTempPath;
			if (string.IsNullOrWhiteSpace(tmpFilePath))
				throw new Exception("ServerTempPath is null or empty");
			if (!File.Exists(tmpFilePath))
				throw new Exception($"{tmpFilePath} does not exist.");

			File.Delete(tmpFilePath);
		}

		public static async Task WriteTempFileAsync(this WvpFileInfo fileInfo, IJSRuntime JSRuntime, ElementReference elementRef, Func<WvpFileInfo, Task> UpdateProgressCallback)
		{
			string tmpFilePath = Path.GetRandomFileName();
			using (Stream fileStream = File.OpenWrite(tmpFilePath))
			{
				using (Stream stream = new MemoryStream())
				{
					await WriteToStreamAsync(fileInfo, stream, JSRuntime, elementRef, UpdateProgressCallback);

					stream.CopyTo(fileStream);
				}
			}

			fileInfo.ServerTempPath = tmpFilePath;
		}

		private static async Task UpdateProgressAsync(WvpFileInfo fileInfo, long progressValue, long progressMax, string status, Func<WvpFileInfo, Task> UpdateProgressCallback)
		{
			fileInfo.ProgressMax = progressMax;
			fileInfo.ProgressValue = progressValue;
			fileInfo.Status = status;
			await UpdateProgressCallback.Invoke(fileInfo);
		}

		private static async Task WriteToStreamAsync(WvpFileInfo fileInfo, Stream stream, IJSRuntime JSRuntime, ElementReference elementRef, Func<WvpFileInfo, Task> UpdateProgressCallback)
		{
			var MaxMessageLength = 3;
			int MaxMessageSize = 20 * 1024;//<20KB

			CancellationToken cancellationToken = CancellationToken.None;

			await Task.Run(async () =>
			{
				await UpdateProgressAsync(fileInfo, 0, fileInfo.Size, "0%", UpdateProgressCallback);
				var position = 0;
				long qPosition = 0;

				try
				{
					if (!string.IsNullOrWhiteSpace(fileInfo.ServerTempPath))
					{
						using (FileStream fs = File.OpenRead(fileInfo.ServerTempPath))
						{
							byte[] b = new byte[1024];
							while (fs.Read(b, 0, b.Length) > 0)
							{
								await stream.WriteAsync(b, cancellationToken);
							}
						}

						File.Delete(fileInfo.ServerTempPath);
						fileInfo.ServerTempPath = string.Empty;
					}
					else
					{

						var q = new Queue<ValueTask<string>>();

						while (position < fileInfo.Size)
						{
							while (q.Count < MaxMessageLength && qPosition < fileInfo.Size)
							{
								cancellationToken.ThrowIfCancellationRequested();
								var taskPosition = qPosition;
								var taskSize = Math.Min(MaxMessageSize, (fileInfo.Size - qPosition));

								var task = JSRuntime.InvokeAsync<string>("WebVellaPulsar.readFileData",
									cancellationToken,
									elementRef,
									fileInfo.Id, taskPosition, taskSize);
								q.Enqueue(task);
								qPosition += taskSize;
							}

							if (q.Count == 0)
								continue;

							var task2 = q.Dequeue();

							int q1;
							int q2;

							ThreadPool.GetAvailableThreads(out q1, out q2);
							var base64 = await task2.ConfigureAwait(true);
							var buffer2 = Convert.FromBase64String(base64);
							await stream.WriteAsync(buffer2, cancellationToken);
							position += buffer2.Length;
							await UpdateProgressAsync(fileInfo, position, fileInfo.Size, $"{(position / fileInfo.Size) * 100}%", UpdateProgressCallback);
						}
					}

					stream.Seek(0, SeekOrigin.Begin);
				}
				catch
				{
					//Ignore
				}
				finally
				{
					await UpdateProgressAsync(fileInfo, 0, 100, "", UpdateProgressCallback);
				}

			});
		}



	}
}
