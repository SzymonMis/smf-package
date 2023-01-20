namespace SMF.Core
{
	public interface IFixedUpdateInvoker
	{
		void OnFixedUpdate(float fixedDeltaTime);
	}

	public interface IUpdateInvoker
	{
		void OnUpdate(float deltaTime);
	}

	public interface ILateUpdateInvoker
	{
		void OnLateUpdate(float deltaTime);
	}
}