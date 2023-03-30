namespace SIS.HTTP
{
	public enum HttpResponseCode
	{
		Ok = 200,
		MovedPermanently = 301,
		TemporaryRedirect = 307, 
		Unauthorized = 401, 
		Forbiden = 403,
		NotFound = 404,
		InternalServerError = 500,
	}
}