﻿using SIS.HTTP;

namespace SIS.MvcFramework
{
	public abstract class HttpMethodAttribute : Attribute
	{
		protected HttpMethodAttribute()
		{

		}

		protected HttpMethodAttribute(string url)
		{
			this.Url = url;
		}

		public string Url { get; protected set; }

		public abstract HttpMethodType Type { get; }
	}
}
