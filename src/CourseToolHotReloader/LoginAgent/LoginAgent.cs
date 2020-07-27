﻿using System.Net;
using System.Threading.Tasks;
using CourseToolHotReloader.ApiClient;
using CourseToolHotReloader.Dtos;
using CourseToolHotReloader.Log;

namespace CourseToolHotReloader.LoginAgent
{
	public interface ILoginAgent
	{
		Task<ShortUserInfo> SignIn();
	}

	public class LoginAgent : ILoginAgent
	{
		private readonly IConfig config;
		private readonly IUlearnApiClient ulearnApiClient;

		public LoginAgent(IConfig config, IUlearnApiClient ulearnApiClient)
		{
			this.config = config;
			this.ulearnApiClient = ulearnApiClient;
		}

		public async Task<ShortUserInfo> SignIn()
		{
			var isSignInSuccess = await TryLoginByConfig()
				|| await TryLoginByConsole();
			return isSignInSuccess ? await ulearnApiClient.GetShortUserInfo() : null;
		}

		private async Task<bool> TryLoginByConsole()
		{
			ConsoleWorker.WriteLine($"Войдите на {config.BaseUrl}");
			var login = ConsoleWorker.GetLogin();
			var password = new NetworkCredential(string.Empty, ConsoleWorker.GetPassword()).Password;

			var jwtToken = await ulearnApiClient.Login(login, password);

			return TrySetJwtTokenInConfig(jwtToken, login);
		}

		private async Task<bool> TryLoginByConfig()
		{
			if (config.JwtToken is null)
				return false;

			var jwtToken = await ulearnApiClient.RenewToken();

			return TrySetJwtTokenInConfig(jwtToken, config.Login);
		}

		private bool TrySetJwtTokenInConfig(string jwtToken, string login)
		{
			if (jwtToken is null)
				return false;

			config.JwtToken = jwtToken;
			config.Login = login;
			config.Flush();

			return true;
		}
	}
}