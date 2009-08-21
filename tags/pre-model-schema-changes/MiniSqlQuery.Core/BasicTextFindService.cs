﻿using System;

namespace MiniSqlQuery.Core
{
	/// <summary>
	/// A simple text finding service. Currently supports forward only text matching.
	/// </summary>
	public class BasicTextFindService : ITextFindService
	{
		private readonly IApplicationServices _services;

		/// <summary>
		/// Creates a new text find service.
		/// </summary>
		/// <param name="applicationServices">A reference to the application services.</param>
		public BasicTextFindService(IApplicationServices applicationServices)
		{
			_services = applicationServices;
		}

		#region ITextFindService Members

		/// <summary>
		/// Looks for the next match depending on the settings in the <paramref name="request"/>.
		/// </summary>
		/// <param name="request">The text find request.</param>
		/// <returns>An updated request with the relevent values adjusted (namely position).</returns>
		public FindTextRequest FindNext(FindTextRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}

			int pos = -1;

			//if (request.SearchUp)
			//{
			//    //todo - I think its the TextProvider's job...?
			//}
			//else // search down.
			//{
			pos = request.TextProvider.FindString(request.SearchValue, request.Position, request.StringComparison);
			//    //pos = request.TextProvider.FindString(request);
			//}

			if (pos > -1)
			{
				// the editor will highlight the find
				request.Position = pos;
			}
			else
			{
				// reset to start of buffer.
				request.Position = 0;

				// todo - notify, beep etc
			}

			return request;
		}

		#endregion
	}
}