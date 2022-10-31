
namespace WaterToolkit.Blackboards
{
	public interface IBlackboardUser
	{
		object[] dependencies { get; }

		void ResolveDependencies();

		bool AreDependenciesValid()
		{
			bool result = true;
			foreach(object dependency in dependencies) {
				if(dependency == null) { 
					result = false; 
					break; 
				}
			}
			return result;
		}
	}
}